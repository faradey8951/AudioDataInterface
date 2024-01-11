using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AudioDataInterface
{
    public class DataHandler
    {
        public MemoryStream ms;
        public Mp3FileReader reader = null;
        public static int mp3_buffSize = 256;
        public string mp3_message = "";

        long i = 0;
        //Процесс буферизации данных для MP3 плеера
        public void BufferMp3()
        {
            ms = new MemoryStream();
            while (true)
            {
                while (Decoder.buff_decodedData.Count < i + mp3_buffSize * 2) Thread.Sleep(10);
                int pos = (int)ms.Position;
                ms.Position = ms.Length;
                //string[] bytes = { Decoder.buff_decodedData[(int)i][9].Substring(0, 8), Decoder.buff_decodedData[(int)i][9].Substring(8, 8), Decoder.buff_decodedData[(int)i][9].Substring(16, 8), Decoder.buff_decodedData[(int)i][9].Substring(24, 8) };
                byte[] bytes = new byte[mp3_buffSize * 4];
                for (int p = 0; p < mp3_buffSize * 4 && i < Decoder.buff_decodedData.Count; p += 4, i++)
                {
                    //while (Decoder.buff_decodedData.Count < i + mp3_buffSize * 2) Thread.Sleep(10);
                    lock (Decoder.decodedDataLocker)
                    {
                        if (Decoder.buff_decodedData[(int)i][8][38].ToString() == "0")
                        {

                            bytes[p] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(0, 8), 2);
                            bytes[p + 1] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(8, 8), 2);
                            bytes[p + 2] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(16, 8), 2);
                            bytes[p + 3] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(24, 8), 2);
                        }
                        else
                        {
                            form_main.mpsPlayer_disc1Detected = true;
                            string subCode = Decoder.buff_decodedData[(int)i][9]; //Получаем двоичный субкод
                            //Выделяем составляющие байты субкода
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

                            //deltaBytes += 4;
                            p -= 4;
                        }
                    }
                }
                lock (Decoder.decodedDataLocker) Decoder.buff_decodedData.RemoveRange(0, (int)i);
                i = 0;
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = pos;
                //Thread.Sleep(10);
            }
        }

        public static void StartMp3Listening()
        {
            Thread t1 = new Thread(form_main.class_dataHandler.BufferMp3);
            t1.Start();
            Thread t2 = new Thread(form_main.class_dataHandler.PlayMp3);
            t2.Start();
            form_main.window_main.pictureBox_runningIndicator.Image = Properties.Resources.CD_Skip_Transparrent;
        }

        //Метод очистки mp3 данных
        public void ClearMp3()
        {
            //AudioIO.naudio_wasapiOut.Dispose();
            //ms = new MemoryStream();
        }

        //Процесс воспроизведения MP3
        public void PlayMp3()
        {
            List<string> buff_time = new List<string>(); //Буфер текущего времени воспроизведения
            string currentTime = "0"; //Текущее время воспроизведения

            while (true)
            {
                try
                {
                    //Буферизация данных
                    while (ms.Length < mp3_buffSize * 8)
                    {
                        mp3_message = "MP3 Player: buffering...";
                        if (form_main.mpsPlayer_mode != "seek") form_main.MpsPlayerRunningIndicatorSeek();
                        form_main.mpsPlayer_mode = "seek";
                        form_main.mpsPlayer_showTime = false;
                        Thread.Sleep(10);
                    }
                    reader = new Mp3FileReader(ms);
                    //var device = naudio_deviceEnumerator.GetDevice(naudio_outputDeviceId[sound_playDeviceId]);
                    AudioIO.naudio_wasapiOut = new NAudio.Wave.WasapiOut(AudioClientShareMode.Shared, true, 50);
                    AudioIO.naudio_wasapiOut.Init(reader);
                    AudioIO.naudio_wasapiOut.Play();
                    form_main.window_main.pictureBox_runningIndicator.Image = Properties.Resources.CD_Playback_Transparrent;
                    while (AudioIO.naudio_wasapiOut.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        if (form_main.mpsPlayer_mode != "play") form_main.MpsPlayerRunningIndicatorPlay();
                        form_main.mpsPlayer_mode = "play";
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
                        Thread.Sleep(10);
                    }
                    if (form_main.mpsPlayer_mode != "") form_main.MpsPlayerRunningIndicatorStop();
                    form_main.mpsPlayer_mode = "";
                    form_main.mpsPlayer_showTime = true;
                }
                catch (Exception ex)
                {
                    mp3_message = "MP3 Player: " + ex.Message;
                    ms = new MemoryStream();
                    //lock (Decoder.buff_decodedData) Decoder.buff_decodedData.Clear();
                    Thread.Sleep(10);
                }
            }
        }
    }
}
