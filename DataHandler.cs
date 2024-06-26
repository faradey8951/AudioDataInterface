﻿using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public class DataHandler
    {
        public static MemoryStream ms;
        public static MemoryStream msFile;
        public static Mp3FileReader reader = null;
        public static int mp3_buffSize = 0;
        public static List<string[]> mp3Buffer = new List<string[]>();
        public static long currentErrorPos = 0;
        bool writeFile = false;
        public static Thread thread_bufferMp3;
        public static Thread thread_playMp3;
        public static bool testEx = false;

        FileStream fs = null;

        static long i = 0;
        //Процесс буферизации данных для MP3 плеера
        public static void BufferMp3()
        {
            ms = new MemoryStream();
            msFile = new MemoryStream();
            while (Decoder.decoderActive)
            {
                mp3Buffer.Clear();
                while (Decoder.buff_decodedData.Count < i + 128 || ms.Length - ms.Position >= mp3_buffSize) Thread.Sleep(10);
                int pos = (int)ms.Position;
                ms.Position = ms.Length;
                List<byte> bytes = new List<byte>();
                List<byte> bytesFile = new List<byte>();
                int byteCount = mp3_buffSize * 4;
                lock (Decoder.decodedDataLocker)
                {
                    for (; i < 128; i++) mp3Buffer.Add(Decoder.buff_decodedData[(int)i]);
                    Decoder.buff_decodedData.RemoveRange(0, (int)i);
                }
                for (int p = 0; p < mp3Buffer.Count; p++)
                {
                    if (mp3Buffer[p][3][38] == '1')
                    {                        
                        bytes.Add(Convert.ToByte(mp3Buffer[p][4].Substring(0, 8), 2));
                        bytes.Add(Convert.ToByte(mp3Buffer[p][4].Substring(8, 8), 2));
                        bytes.Add(Convert.ToByte(mp3Buffer[p][4].Substring(16, 8), 2));
                        bytes.Add(Convert.ToByte(mp3Buffer[p][4].Substring(24, 8), 2));

                        //if (writeFile == true) msFile.Write(new byte[] { Convert.ToByte(mp3Buffer[p][4].Substring(0, 8), 2), Convert.ToByte(mp3Buffer[p][4].Substring(8, 8), 2), Convert.ToByte(mp3Buffer[p][4].Substring(16, 8), 2), Convert.ToByte(mp3Buffer[p][4].Substring(24, 8), 2) }, 0, 4);
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
                            if (subCodeByte1 == 100)
                            {
                                string sum = Convert.ToString(subCodeByte2, 2).PadLeft(8, '0') + Convert.ToString(subCodeByte3, 2).PadLeft(8, '0') + Convert.ToString(subCodeByte4, 2).PadLeft(8, '0');
                                string part1 = sum.Substring(0, 12);
                                string part2 = sum.Substring(12, 12);
                                int currentTime = Convert.ToInt32(part1, 2);
                                int duration = Convert.ToInt32(part2, 2);
                                form_main.mpsPlayer_timeSeconds = currentTime;
                                form_main.mpsPlayer_timeDurationSeconds = duration;
                            }
                            if (subCodeByte1 == 200) //Субкод
                            {
                                form_main.mpsPlayer_currentTrackNumber = subCodeByte2;
                                form_main.mpsPlayer_trackCount = subCodeByte3;
                            }
                            if (subCodeByte1 == 50)
                            {
                                i = 0;
                                Decoder.unfixedErrorCount = 0;
                                Decoder.fixedErrorCount = 0;
                                Decoder.frameSyncErrorCount = 0;
                                //writeFile = true;
                                //msFile = new MemoryStream();
                            }
                            if (subCodeByte1 == 60)
                            {
                                //writeFile = false;
                                //fs = new FileStream("test.mp3", FileMode.Create);
                                //msFile.Position = 0;
                                //msFile.CopyTo(fs);
                                //fs.Close();
                            }
                        }
                    }
                }

                i = 0;
                ms.Write(bytes.ToArray(), 0, bytes.Count);
                ms.Position = pos;
                Thread.Sleep(10);
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
                    while (ms.Length - ms.Position < mp3_buffSize) { Thread.Sleep(10); form_main.mpsPlayer_showTime = false; }
                    reader = new Mp3FileReader(ms);
                    AudioIO.naudio_wasapiOut = new NAudio.Wave.WasapiOut(AudioClientShareMode.Shared, true, 50);
                    AudioIO.naudio_wasapiOut.Init(reader);
                    AudioIO.naudio_wasapiOut.Play();
                    while (AudioIO.naudio_wasapiOut.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        form_main.mpsPlayer_showTime = true;
                        currentTime = reader.CurrentTime.Minutes.ToString() + ":" + reader.CurrentTime.Seconds.ToString() + ":" + reader.CurrentTime.Milliseconds.ToString();
                        //Если воспроизведение прервалось
                        if (buff_time.Count > 5 && buff_time[4] == buff_time[3] && buff_time[2] == buff_time[0])
                        {
                            buff_time.Clear();
                            throw new Exception("WASAPI clear triggered");
                        }
                        if (buff_time.Count > 5) buff_time.Clear();
                        buff_time.Add(currentTime);
                        if (testEx) { reader.Close(); testEx = false; throw new Exception("TEST exception"); }
                        Thread.Sleep(10);
                    }
                    form_main.mpsPlayer_showTime = true;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(10);
                }
            }
        }
    }
}
