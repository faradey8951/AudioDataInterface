using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using OpusDotNet;
using System.Data;
using FftSharp;
//using Concentus;
//using Concentus.Celt;
//using Concentus.Common.CPlusPlus;
//using Concentus.Enums;
//using Concentus.Structs;

namespace AudioDataInterface
{
    public class DataHandler
    {
        public static MemoryStream ms;
        public static List<byte> packet = new List<byte>(); //Буфер пакета фреймов OPUS
        public static int packetSize = 0; //Размер полученного пакета фреймов
        public static RawSourceWaveStream rawSourceWaveStream = null;
        public static int mp3_buffSize = 0;
        public static List<string[]> mp3Buffer = new List<string[]>();
        public static Thread thread_bufferMp3;
        public static Thread thread_playMp3;
        public static bool subcodeSync = false;
        public static bool subcodeSyncError = false;
        public static bool subcodeTimecode = false;
        public static bool subcodeTOC = false;
        public static bool interpolation = false;
        public static bool mute = false;

        FileStream fs = null;

        static long i = 0;
        //Процесс буферизации данных для MP3 плеера
        public static void BufferMp3()
        {
            ms = new MemoryStream();
            int pos = 0;
            List<byte> outputPCMBytes = new List<byte>(); //Выходной буфер байт декодированного PCM аудиопотока
            bool dropout = false; //Наличие выпадения фрейма
            int dropoutFramesCount = 0; //Количество выпавших фреймов
            byte[] framePCMBytes1 = new byte[40 * (48000 / 1000) * 2]; //Буфер байт PCM аудиопотока фрейма до выпадения
            byte[] framePCMBytes2 = new byte[40 * (48000 / 1000) * 2]; //Буфер байт PCM аудиопотока фрейма после выпадения
            OpusDecoder decoder = new OpusDecoder(48000, 1); //Декодер OPUS фреймов
            while (Decoder.decoderActive)
            {
                mp3Buffer.Clear();
                while (Decoder.buff_decodedData.Count < i + 128 || ms.Length - ms.Position > 96000) Thread.Sleep(10); //Ожидание наполнения данных + задержка буферизации для синхронизации таймкода               
                lock (Decoder.decodedDataLocker)
                {
                    string[] decodedBlock = null;                   
                    for (; i < 128; i++) { decodedBlock = Decoder.buff_decodedData[(int)i]; if (Convert.ToInt16(decodedBlock[6]) >= 80 && decodedBlock[4] != "01010101010101010101010101010101") mp3Buffer.Add(decodedBlock); }
                    Decoder.buff_decodedData.RemoveRange(0, (int)i);
                }
                for (int p = 0; p < mp3Buffer.Count; p++)
                {
                    if (mp3Buffer[p][3][38] == '1')
                    {
                        packet.Add(Convert.ToByte(mp3Buffer[p][4].Substring(0, 8), 2));
                        packet.Add(Convert.ToByte(mp3Buffer[p][4].Substring(8, 8), 2));
                        packet.Add(Convert.ToByte(mp3Buffer[p][4].Substring(16, 8), 2));
                        packet.Add(Convert.ToByte(mp3Buffer[p][4].Substring(24, 8), 2));
                    }
                    else
                    {
                        string subCode = mp3Buffer[p][4]; //Получаем двоичный субкод
                        byte subCodeByte1 = Convert.ToByte(Convert.ToInt16(subCode.Substring(0, 8), 2));
                        byte subCodeByte2 = Convert.ToByte(Convert.ToInt16(subCode.Substring(8, 8), 2));
                        byte subCodeByte3 = Convert.ToByte(Convert.ToInt16(subCode.Substring(16, 8), 2));
                        byte subCodeByte4 = Convert.ToByte(Convert.ToInt16(subCode.Substring(24, 8), 2));
                        if (subCodeByte1 == 100) //Таймкод
                        {
                            //subcodeSync = false;
                            subcodeTimecode = true;
                            //subcodeTOC = false;
                            string sum = Convert.ToString(subCodeByte2, 2).PadLeft(8, '0') + Convert.ToString(subCodeByte3, 2).PadLeft(8, '0') + Convert.ToString(subCodeByte4, 2).PadLeft(8, '0');
                            string part1 = sum.Substring(0, 12);
                            string part2 = sum.Substring(12, 12);
                            int currentTime = Convert.ToInt32(part1, 2);
                            int duration = Convert.ToInt32(part2, 2);
                            form_main.mpsPlayer_timeSeconds = currentTime;
                            form_main.mpsPlayer_timeDurationSeconds = duration;
                        }
                        if (subCodeByte1 == 200) //TOC
                        {
                            subcodeTOC = true;
                            form_main.mpsPlayer_currentTrackNumber = subCodeByte2;
                            form_main.mpsPlayer_trackCount = subCodeByte3;
                        }
                        if (subCodeByte1 == 123 && subCodeByte2 == 1 && subCodeByte3 == 1 && subCodeByte4 == 1) //Субкод канальной синхронизации правого канала
                        {
                            packetSize = packet.Count;
                            if (packetSize != 2000) LogHandler.WriteStatus("DataHandler/AudioBuffer", "Got packet size of " + packetSize.ToString() + " bytes instead of 2000 bytes");
                            subcodeSync = true;
                            if (packet.Count >= 80) //Триггер размера пакета для обработки фреймов
                            {
                                List<byte> opusFrame = new List<byte>(); //Буфер байт OPUS фрейма
                                byte[] framePCMBytes = new byte[40 * (48000 / 1000) * 2]; //Буфер байт PCM аудиопотока фрейма
                                for (int i = 0; i < packet.Count; i++)
                                {
                                    opusFrame.Add(packet[i]);
                                    if (opusFrame.Count == 80) //Размер фрейма в байтах
                                    {
                                        if (dropout == true) { dropoutFramesCount++; }
                                        else //Триггер конца выпадения
                                        {
                                            //Интерполяция выпавших фреймов
                                            if (dropoutFramesCount > 0 && dropoutFramesCount <= 10)
                                            {
                                                interpolation = true;
                                                framePCMBytes.CopyTo(framePCMBytes2, 0);
                                                double[] framePCMShorts1 = new double[framePCMBytes1.Length / 2];
                                                double[] framePCMShorts2 = new double[framePCMBytes2.Length / 2];
                                                for (int k = 0, l = 0; k < framePCMShorts1.Length; k++, l += 2) framePCMShorts1[k] = (double)BitConverter.ToInt16(new byte[] { framePCMBytes1[l], framePCMBytes1[l + 1] }, 0);
                                                for (int k = 0, l = 0; k < framePCMShorts2.Length; k++, l += 2) framePCMShorts2[k] = (double)BitConverter.ToInt16(new byte[] { framePCMBytes2[l], framePCMBytes2[l + 1] }, 0);
                                                List<short> calculatedPCMShorts = new List<short>();
                                                List<byte> calculatedPCMBytes = new List<byte>();
                                                double[] paddedAudio1 = FftSharp.Pad.ZeroPad(framePCMShorts1);
                                                double[] paddedAudio2 = FftSharp.Pad.ZeroPad(framePCMShorts2);
                                                System.Numerics.Complex[] complex1 = FftSharp.FFT.Forward(paddedAudio1);
                                                System.Numerics.Complex[] complex2 = FftSharp.FFT.Forward(paddedAudio2);
                                                double[] fftMag1 = FftSharp.FFT.Magnitude(complex1, false);
                                                double[] fftMag2 = FftSharp.FFT.Magnitude(complex2, false);
                                                double[] fftPhase1 = FftSharp.FFT.Phase(complex1);
                                                double[] fftPhase2 = FftSharp.FFT.Phase(complex2);
                                                double[] fftMagDropout = new double[2048 * dropoutFramesCount];
                                                double[] fftPhaseDropout = new double[2048 * dropoutFramesCount];
                                                int x1 = 1;
                                                int x2 = dropoutFramesCount + 2;
                                                //Интерполяция магнитуд
                                                for (int z = 0, h = 0; z < fftPhase1.Length; z++)
                                                {
                                                    h = z;
                                                    double y1 = fftMag1[z];
                                                    double y2 = fftMag2[z];
                                                    for (int x = x1 + 1; x < x2; x++, h += 2048)
                                                    {
                                                        int xi = x;
                                                        fftMagDropout[h] = y1 + (((xi - x1) * (y2 - y1)) / (x2 - x1));
                                                    }
                                                }
                                                //Интерполяция фаз
                                                for (int z = 0, h = 0; z < fftPhase1.Length; z++)
                                                {
                                                    h = z;
                                                    double y1 = fftPhase1[z];
                                                    double y2 = fftPhase2[z];
                                                    for (int x = x1 + 1; x < x2; x++, h += 2048)
                                                    {
                                                        int xi = x;
                                                        fftPhaseDropout[h] = y1 + (((xi - x1) * (y2 - y1)) / (x2 - x1));
                                                    }
                                                }
                                                List<System.Numerics.Complex> test = new List<System.Numerics.Complex>();
                                                for (int t = 0; t < fftMagDropout.Length; t++) test.Add(System.Numerics.Complex.FromPolarCoordinates(fftMagDropout[t] * 1.0, fftPhaseDropout[t] * 1.0));
                                                System.Numerics.Complex[] testIFFT = test.ToArray();
                                                List<System.Numerics.Complex[]> testIFFTFramed = new List<System.Numerics.Complex[]>();
                                                for (int k = 0; k < testIFFT.Length; k += 2048)
                                                {
                                                    testIFFTFramed.Add(new System.Numerics.Complex[2048]);
                                                    for (int t = k, l = 0; t < k + 2048; t++, l++) testIFFTFramed[testIFFTFramed.Count - 1][l] = testIFFT[t];
                                                }
                                                for (int t = 0; t < testIFFTFramed.Count; t++) FftSharp.FFT.Inverse(testIFFTFramed[t]);
                                                foreach (System.Numerics.Complex[] c in testIFFTFramed) for (int t = 65; t < 65 + 1920; t++) { double resultShort = c[t].Real * 512; if (resultShort <= 32767 && resultShort >= -32767) calculatedPCMShorts.Add((short)resultShort); else calculatedPCMShorts.Add(0); }
                                                foreach (short s in calculatedPCMShorts) calculatedPCMBytes.AddRange(BitConverter.GetBytes(s));
                                                outputPCMBytes.AddRange(calculatedPCMBytes);
                                                dropout = false;
                                                LogHandler.WriteStatus("DataHandler/AudioBuffer", "Audio interpolation " + dropoutFramesCount.ToString() + " frames (" + (dropoutFramesCount * 1920).ToString() + " samples)");
                                            }
                                            //Добавить тишину, если выпало больше порога фреймов
                                            if (dropoutFramesCount > 10)
                                            {
                                                interpolation = false;
                                                mute = true;
                                                for (int t = 0; t < 1920 * dropoutFramesCount; t++) outputPCMBytes.AddRange(BitConverter.GetBytes(0));
                                                dropout = false;
                                                LogHandler.WriteStatus("DataHandler/AudioBuffer", "Mute " + dropoutFramesCount.ToString() + " frames (" + (dropoutFramesCount * 1920).ToString() + " samples)");
                                            }
                                            dropoutFramesCount = 0;
                                        }
                                        try
                                        {
                                            decoder.Decode(opusFrame.ToArray(), opusFrame.Count, framePCMBytes, framePCMBytes.Length);
                                            if (dropout == false) { outputPCMBytes.AddRange(framePCMBytes); framePCMBytes.CopyTo(framePCMBytes1, 0); }
                                            opusFrame.Clear();
                                            dropout = false;
                                        }
                                        catch (Exception ex)
                                        {
                                            dropout = true;
                                            opusFrame.Clear();
                                        }
                                    }
                                }
                            }
                            packet.Clear();
                        }
                        if (subCodeByte1 == 50)
                        {
                            i = 0;
                            Decoder.unfixedErrorCount = 0;
                            Decoder.fixedErrorCount = 0;
                            Decoder.frameSyncErrorCount = 0;
                        }
                        if (subCodeByte1 == 60)
                        {

                        }
                    }
                }
                i = 0;

                if (outputPCMBytes.Count >= 3840)
                {
                    pos = (int)ms.Position;
                    ms.Position = ms.Length;
                    ms.Write(outputPCMBytes.ToArray(), 0, outputPCMBytes.Count);
                    ms.Position = pos;
                    outputPCMBytes.Clear();
                }
            }
        }

        public static void StartMp3Listening()
        {
            if (thread_bufferMp3 != null) { thread_bufferMp3.Abort(); thread_bufferMp3 = null; }
            if (thread_playMp3 != null) { thread_playMp3.Abort(); thread_playMp3 = null; }
            thread_bufferMp3 = new Thread(BufferMp3);
            thread_bufferMp3.Start();
            thread_playMp3 = new Thread(PlayMp3);
            thread_playMp3.Start();
        }

        public static void StopMp3Listening()
        {
            if (thread_bufferMp3 != null) { thread_bufferMp3.Abort(); thread_bufferMp3 = null; }
            if (thread_playMp3 != null) { thread_playMp3.Abort(); thread_playMp3 = null; }
        }

        //Метод очистки mp3 данных
        public void ClearMp3()
        {
            //AudioIO.naudio_wasapiOut.Dispose();
            //ms = new MemoryStream();
        }

        //Процесс воспроизведения MP3
        public static void PlayMp3()
        {
            List<string> buff_time = new List<string>(); //Буфер текущего времени воспроизведения
            string currentTime = "0"; //Текущее время воспроизведения                     
            while (Decoder.decoderActive)
            {
                try
                {
                    //Буферизация данных
                    while (ms.Length - ms.Position < 96000 && AudioIO.naudio_wasapiOut.PlaybackState != NAudio.Wave.PlaybackState.Playing) { Thread.Sleep(10); form_main.mpsPlayer_showTime = false; }
                    rawSourceWaveStream = new RawSourceWaveStream(ms, new WaveFormat(48000, 16, 1));
                    AudioIO.naudio_wasapiOut = new NAudio.Wave.WasapiOut(AudioClientShareMode.Shared, true, 50);
                    AudioIO.naudio_wasapiOut.Init(rawSourceWaveStream);
                    AudioIO.naudio_wasapiOut.Play();
                    while (AudioIO.naudio_wasapiOut.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        form_main.mpsPlayer_showTime = true;
                        Thread.Sleep(10);
                    }
                    form_main.mpsPlayer_showTime = true;
                }
                catch (Exception ex)
                {
                    LogHandler.WriteError("PlayMp3()", ex.Message);
                    Thread.Sleep(10);
                }
            }
        }
    }
}
