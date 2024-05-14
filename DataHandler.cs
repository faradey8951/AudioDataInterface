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
            OpusDecoder decoder = new OpusDecoder(48000, 1);
            while (Decoder.decoderActive)
            {
                mp3Buffer.Clear();
                while (Decoder.buff_decodedData.Count < i + 128 || ms.Length - ms.Position > 192000) Thread.Sleep(10); //Ожидание наполнения данных + задержка буферизации для синхронизации таймкода               
                lock (Decoder.decodedDataLocker)
                {
                    for (; i < 128; i++) mp3Buffer.Add(Decoder.buff_decodedData[(int)i]);
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
                        if (form_main.mpsPlayer_disc1Detected == true)
                        {
                            string subCode = mp3Buffer[p][4]; //Получаем двоичный субкод
                            byte subCodeByte1 = Convert.ToByte(Convert.ToInt16(subCode.Substring(0, 8), 2));
                            byte subCodeByte2 = Convert.ToByte(Convert.ToInt16(subCode.Substring(8, 8), 2));
                            byte subCodeByte3 = Convert.ToByte(Convert.ToInt16(subCode.Substring(16, 8), 2));
                            byte subCodeByte4 = Convert.ToByte(Convert.ToInt16(subCode.Substring(24, 8), 2));
                            if (subCodeByte1 == 100) //Таймкод
                            {
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
                                form_main.mpsPlayer_currentTrackNumber = subCodeByte2;
                                form_main.mpsPlayer_trackCount = subCodeByte3;
                            }
                            if (subCodeByte1 == 123 && subCodeByte2 == 1 && subCodeByte3 == 1 && subCodeByte4 == 1) //Субкод канальной синхронизации правого канала
                            {
                                packetSize = packet.Count;
                                if (packet.Count >= 80)
                                {
                                    List<byte> opusFrame = new List<byte>(); //Буфер байт OPUS фрейма
                                    byte[] framePCMBytes = new byte[40 * (48000 / 1000) * 2]; //Буфер байт PCM аудиопотока фрейма
                                    for (int i = 0; i < packet.Count; i++)
                                    {
                                        opusFrame.Add(packet[i]);
                                        if (opusFrame.Count == 80)
                                        {
                                            if (dropout == true) { dropoutFramesCount++; }
                                            else
                                            {
                                                if (dropoutFramesCount > 0) //Регистрация буфера байт PCM аудиопотока фрейма после выпадения. Интерполяция сэмплов выпавших фреймов
                                                {
                                                    framePCMBytes.CopyTo(framePCMBytes2, 0);
                                                    double[] framePCMShorts1 = new double[framePCMBytes1.Length / 2];
                                                    double[] framePCMShorts2 = new double[framePCMBytes2.Length / 2];
                                                    for (int k = 0, l = 0; k < framePCMShorts1.Length; k++, l += 2) framePCMShorts1[k] = (double)BitConverter.ToInt16(new byte[] { framePCMBytes1[l], framePCMBytes1[l + 1] }, 0);
                                                    for (int k = 0, l = 0; k < framePCMShorts2.Length; k++, l += 2) framePCMShorts2[k] = (double)BitConverter.ToInt16(new byte[] { framePCMBytes2[l], framePCMBytes2[l + 1] }, 0);
                                                    short[] calculatedPCMShorts = new short[1920 * dropoutFramesCount]; //Буфер интерполированных сэмплов
                                                    List<byte> calculatedPCMBytes = new List<byte>();
                                                    double[] paddedAudio1 = FftSharp.Pad.ZeroPad(framePCMShorts1);
                                                    double[] paddedAudio2 = FftSharp.Pad.ZeroPad(framePCMShorts2);
                                                    System.Numerics.Complex[] complex1 = FftSharp.FFT.Forward(paddedAudio1);
                                                    System.Numerics.Complex[] complex2 = FftSharp.FFT.Forward(paddedAudio2);
                                                    double[] fftMag1 = FftSharp.FFT.Magnitude(complex1, false);
                                                    double[] fftMag2 = FftSharp.FFT.Magnitude(complex2, false);
                                                    double[] fftPhase1 = FftSharp.FFT.Phase(complex1);
                                                    double[] fftPhase2 = FftSharp.FFT.Phase(complex2);
                                                    double[] fftMagDropout = new double[1920 * dropoutFramesCount];
                                                    double[][] fftPhaseDropout = new double[dropoutFramesCount][];
                                                    for (int k = 65; k < 1985; k++)
                                                    {
                                                        int x1 = 64;
                                                        int x2 = 64;
                                                        double y1 = fftMag1[64];
                                                        double y2 = fftMag2[64];
                                                        int xi = k;
                                                    }
                                                    List<System.Numerics.Complex> test = new List<System.Numerics.Complex>();                                              
                                                    for (int t = 0; t < fftMag1.Length; t++) test.Add(System.Numerics.Complex.FromPolarCoordinates(fftMag1[t] * 1024.0, fftPhase1[t]));
                                                    System.Numerics.Complex[] testIFFT = test.ToArray();
                                                    FftSharp.FFT.Inverse(testIFFT);
                                                                                              
                                                    //foreach (short s in calculatedPCMShorts) calculatedPCMBytes.AddRange(BitConverter.GetBytes(s));
                                                    //outputPCMBytes.AddRange(calculatedPCMBytes);
                                                    dropout = false;
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
                    while (ms.Length - ms.Position < 192000 && AudioIO.naudio_wasapiOut.PlaybackState != NAudio.Wave.PlaybackState.Playing) { Thread.Sleep(10); form_main.mpsPlayer_showTime = false; }
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
