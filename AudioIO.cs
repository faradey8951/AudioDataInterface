﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;
using System.Threading;
using NAudio.CoreAudioApi;

namespace AudioDataInterface
{
    /// <summary>
    /// I/O Звуковой карты
    /// </summary>
    public class AudioIO
    {
        //NAudio
        //Префикс "naudio_"
        //////////////////////////////////////////////////////////////////////////////////////
        static WaveIn naudio_graphWaveIn = null;                                            //Поток аудиозахвата для сигналограммы
        static WaveIn naudio_signalWaveIn = null;                                               //Поток аудиозахвата сигнала
        public static WasapiOut naudio_wasapiOut = new WasapiOut();                                    //Вывод звука для mp3
        static AudioFileReader naudio_audioFileReader = null;                                   //Читалка аудио файлов для встроенного плеера
        public static MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        public static MMDeviceCollection mm_dev = null;
        public static WasapiOut naudio_playDeviceWasapiOut = null;

        public static WasapiCapture waveLoop = null;
        //WaveFormat fmt = waveLoop.WaveFormat;

        //////////////////////////////////////////////////////////////////////////////////////

        //Буферы
        //Префикс "buff_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static List<short> buff_graphSamples = new List<short>();                    //Аудиосэмплы для сигналограммы
        public static readonly List<byte> buff_signalBytes = new List<byte>();
        public static readonly List<short> buff_signalSamples = new List<short>();                   //Аудиосэмплы сигнала
        public static readonly List<short> buff_signalSamplesL = new List<short>();                   //Аудиосэмплы сигнала
        public static readonly List<short> buff_signalSamplesR = new List<short>();                   //Аудиосэмплы сигнала
        public static double[] buff_fftSamples;                                              //Аудиосэмплы воспроизводимого звука для БПФ
        public static double[] buff_fftValues;
        //////////////////////////////////////////////////////////////////////////////////////

        //Ключи
        //Префикс "key_"
        //////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////

        public static bool isSignalSamplesBusy = false;

        //Общие настройки
        //Префикс "audio_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static int audio_recDeviceId = 0;                                                //ID текущего устройства записи
        public static int audio_playDeviceId = 0;                                               //ID текущего устройства воспроизведения
        public static int audio_tapePlayDeviceId = 0;
        public static bool audio_tapePlayStopped = true;
        public static int audio_signalHeight = 500;                                               //Высота сигнала
        public static bool audio_invertSignal = true;                                          //Инверсия сигнала
        public static double audio_signalGain = 7;
        public static double audio_signalGainL = 4;
        public static double audio_signalGainR = 4;
        public static short audio_maxAmplitude = 0;
        //////////////////////////////////////////////////////////////////////////////////////

        public static Thread thread_signalAutoGainControl = null;

        /// <summary>
        /// Запрашивает список устройств записи
        /// </summary>
        /// <returns></returns>
        public static string[] GetRecDevices()
        {
            string[] recDevices = new string[WaveIn.DeviceCount];
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                recDevices[i] = deviceInfo.ProductName;
            }

            return recDevices;
        }

        public static string[] GetPlayDevices()
        {
            string[] playDevices = null;
            
            mm_dev = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            playDevices = new string[mm_dev.Count];
            for (int i = 0; i < playDevices.Length; i++) playDevices[i] = mm_dev[i].DeviceFriendlyName;
            return playDevices;
        }

        /// <summary>
        /// Инициализация захвата звука для сигналограммы
        /// </summary>
        public static void GraphCaptureInit()
        {
            try
            {
                if (naudio_graphWaveIn != null)
                    naudio_graphWaveIn.Dispose();
                naudio_graphWaveIn = new WaveIn();
                naudio_graphWaveIn.DeviceNumber = audio_recDeviceId;
                naudio_graphWaveIn.WaveFormat = new NAudio.Wave.WaveFormat(96000, 1);
                naudio_graphWaveIn.DataAvailable += new EventHandler<WaveInEventArgs>(Graph_DataAvailable);
                naudio_graphWaveIn.StartRecording();
            }
            catch { }
        }

        /// <summary>
        /// Закрытие захвата звука для сигналограммы
        /// </summary>
        public static void GraphCaptureClose()
        {
            try
            {
                if (naudio_graphWaveIn != null)
                    naudio_graphWaveIn.Dispose();
                buff_graphSamples.Clear();
            }
            catch { }
        }

        public static void PlayWavFile(string file)
        {
            if (naudio_playDeviceWasapiOut != null) naudio_playDeviceWasapiOut.Stop();
            naudio_audioFileReader = new AudioFileReader(file);
            var device = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[AudioIO.audio_tapePlayDeviceId];       
            naudio_playDeviceWasapiOut = new WasapiOut(device, AudioClientShareMode.Shared, true, 50);
            naudio_playDeviceWasapiOut.PlaybackStopped += OnPlaybackStopped;
            naudio_playDeviceWasapiOut.Init(naudio_audioFileReader);
            audio_tapePlayStopped = false;
            naudio_playDeviceWasapiOut.Play();
        }

        private static void OnPlaybackStopped(object sender, NAudio.Wave.StoppedEventArgs args)
        {
            naudio_playDeviceWasapiOut.Dispose();
            naudio_playDeviceWasapiOut = null;
            naudio_audioFileReader.Dispose();
            naudio_audioFileReader = null;
            audio_tapePlayStopped = true;
        }

        static void Graph_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            for (int i = 0; i < e.Buffer.Length / 2; i += 2)
            {
                if (!audio_invertSignal)
                    buff_graphSamples.Add((short)(audio_signalGainL * BitConverter.ToInt16(new byte[2] { e.Buffer[i], e.Buffer[i + 1] }, 0) + (short)audio_signalHeight));
                else
                    buff_graphSamples.Add((short)(-audio_signalGainL * BitConverter.ToInt16(new byte[2] { e.Buffer[i], e.Buffer[i + 1] }, 0) + (short)audio_signalHeight));
            }
            form_main.window_main.DrawWaveGraphFrame();
        }

        public static void MPSAudioOutputCaptureInit()
        {
            if (waveLoop != null) waveLoop.Dispose();
            waveLoop = new WasapiLoopbackCapture(mm_dev[audio_playDeviceId]);
            waveLoop.DataAvailable += new EventHandler<WaveInEventArgs>(MPS_DataAvailable);
            waveLoop.StartRecording();
        }

        public static void MPSAudioOutputCaptureClose()
        {
            if (waveLoop != null) waveLoop.Dispose();
        }

        /// <summary>
        /// Инициализация захвата сигнала
        /// </summary>
        public static void SignalCaptureInit()
        {
            //try
            //{
            if (naudio_signalWaveIn != null) naudio_signalWaveIn.Dispose();
            naudio_signalWaveIn = new WaveIn();
            naudio_signalWaveIn.DeviceNumber = audio_recDeviceId;
            naudio_signalWaveIn.WaveFormat = new NAudio.Wave.WaveFormat(96000, 2);
            naudio_signalWaveIn.DataAvailable += new EventHandler<WaveInEventArgs>(Signal_DataAvailable);
            naudio_signalWaveIn.StartRecording();
        }

        /// <summary>
        /// Закрытие захвата сигнала
        /// </summary>
        public static void SignalCaptureClose()
        {
            if (naudio_signalWaveIn != null) { naudio_signalWaveIn.Dispose(); naudio_signalWaveIn = null; }
                
            //buff_signalSamples.Clear();
        }

        static void Signal_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            lock (Decoder.bytesLocker) buff_signalBytes.AddRange(e.Buffer);
        }

        static void MPS_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            buff_fftSamples = new double[form_main.mpsPlayer_fftSize];
            int bytesPerSamplePerChannel = waveLoop.WaveFormat.BitsPerSample / 8;
            int bytesPerSample = bytesPerSamplePerChannel * waveLoop.WaveFormat.Channels;
            int bufferSampleCount = buff_fftSamples.Length;

            //Выбор режима декодирования
            if (bytesPerSamplePerChannel == 2 && waveLoop.WaveFormat.Encoding == WaveFormatEncoding.Pcm) //Преобразование байт в 16-битные сэмплы
            {
                for (int i = 0; i < bufferSampleCount; i++)
                    buff_fftSamples[i] = BitConverter.ToInt16(e.Buffer, i * bytesPerSample);
            }
            else if (bytesPerSamplePerChannel == 4 && waveLoop.WaveFormat.Encoding == WaveFormatEncoding.Pcm) //Преобразование байт в 32-битные сэмплы и конвертация в 16-битные
            {
                for (int i = 0; i < bufferSampleCount; i++)
                    buff_fftSamples[i] = Math.Round(BitConverter.ToInt32(e.Buffer, i * bytesPerSample) / (double)65536);
            }
            else if (bytesPerSamplePerChannel == 4 && waveLoop.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat) //Преобразование байт в 32-битные сэмплы с плавающей точкой в 16-битные
            {
                for (int i = 0; i < bufferSampleCount; i++)
                    buff_fftSamples[i] = Math.Round(BitConverter.ToSingle(e.Buffer, i * bytesPerSample) * (double)32768);
            }
            
            double[] paddedAudio = FftSharp.Pad.ZeroPad(buff_fftSamples);
            double[] fftMag = FftSharp.Transform.FFTpower(paddedAudio);
            buff_fftValues = new double[fftMag.Length];
        }

    }
}
