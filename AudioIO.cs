using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;
using System.Threading;

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
        //////////////////////////////////////////////////////////////////////////////////////

        //Буферы
        //Префикс "buff_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static List<short> buff_graphSamples = new List<short>();                    //Аудиосэмплы для сигналограммы
        public static readonly List<byte> buff_signalBytes = new List<byte>();
        public static readonly List<short> buff_signalSamples = new List<short>();                   //Аудиосэмплы сигнала
        public static readonly List<short> buff_signalSamplesL = new List<short>();                   //Аудиосэмплы сигнала
        public static readonly List<short> buff_signalSamplesR = new List<short>();                   //Аудиосэмплы сигнала
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
        public static int audio_signalHeight = 0;                                               //Высота сигнала
        public static bool audio_invertSignal = true;                                          //Инверсия сигнала
        public static double audio_signalGain = 1;
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

        static void Graph_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            for (int i = 0; i < e.Buffer.Length / 2; i += 2)
            {
                if (!audio_invertSignal)
                    buff_graphSamples.Add((short)(audio_signalGain * BitConverter.ToInt16(new byte[2] { e.Buffer[i], e.Buffer[i + 1] }, 0) + (short)audio_signalHeight));
                else
                    buff_graphSamples.Add((short)(-audio_signalGain * BitConverter.ToInt16(new byte[2] { e.Buffer[i], e.Buffer[i + 1] }, 0) + (short)audio_signalHeight));
            }
            form_main.window_main.DrawWaveGraphFrame();
        }

        /// <summary>
        /// Инициализация захвата сигнала
        /// </summary>
        public static void SignalCaptureInit()
        {
            try
            {
                if (naudio_signalWaveIn != null) naudio_signalWaveIn.Dispose();
                naudio_signalWaveIn = new WaveIn();
                naudio_signalWaveIn.DeviceNumber = audio_recDeviceId;
                naudio_signalWaveIn.WaveFormat = new NAudio.Wave.WaveFormat(128000, 2);
                naudio_signalWaveIn.DataAvailable += new EventHandler<WaveInEventArgs>(Signal_DataAvailable);
                naudio_signalWaveIn.StartRecording();
            }
            catch { }
        }

        /// <summary>
        /// Закрытие захвата сигнала
        /// </summary>
        public static void SignalCaptureClose()
        {
            try
            {
                if (naudio_signalWaveIn != null) naudio_signalWaveIn.Dispose();
                buff_signalSamples.Clear();
            }
            catch { }
        }

        /// <summary>
        /// Автоматический регулятор усиления входного сигнала
        /// </summary>
        public static void SignalAutoGainControll()
        {
            while (true)
            {
                //if (Decoder.maxAmplitude < 25000 && audio_signalGain < 50) audio_signalGain += Math.Log10(1 + 5*audio_signalGain);
                //if (Decoder.maxAmplitude > 32000 && audio_signalGain > 1) audio_signalGain -= Math.Log10(1 + 5*audio_signalGain);
                audio_signalGain = 9;
                Thread.Sleep(100);
            }
        }

        static void Signal_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            lock (buff_signalBytes) buff_signalBytes.AddRange(e.Buffer);
        }
    }
}
