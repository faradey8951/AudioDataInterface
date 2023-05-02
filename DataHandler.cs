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
        int mp3_buffSize = 512;
        public string mp3_message = "";
        long i = 0;
        //Процесс буферизации данных для MP3 плеера
        public void BufferMp3()
        {
            ms = new MemoryStream();          
            while (true)
            {
                while (Decoder.buff_decodedData.Count < i + mp3_buffSize) Thread.Sleep(10);
                int pos = (int)ms.Position;
                ms.Position = ms.Length;
                //string[] bytes = { Decoder.buff_decodedData[(int)i][9].Substring(0, 8), Decoder.buff_decodedData[(int)i][9].Substring(8, 8), Decoder.buff_decodedData[(int)i][9].Substring(16, 8), Decoder.buff_decodedData[(int)i][9].Substring(24, 8) };
                byte[] bytes = new byte[mp3_buffSize * 4];
                for (int p = 0; p < mp3_buffSize * 4; p += 4, i++)
                {
                    lock (Decoder.buff_decodedData)
                    {
                        bytes[p] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(0, 8), 2);
                        bytes[p + 1] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(8, 8), 2);
                        bytes[p + 2] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(16, 8), 2);
                        bytes[p + 3] = Convert.ToByte(Decoder.buff_decodedData[(int)i][9].Substring(24, 8), 2);
                    }
                }
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = pos;
                Thread.Sleep(10);
            }
        }

        public static void StartMp3Listening()
        {
            Thread t1 = new Thread(form_main.class_dataHandler.BufferMp3);
            t1.Start();
            Thread t2 = new Thread(form_main.class_dataHandler.PlayMp3);
            t2.Start();
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
            Mp3FileReader reader = null;
            List<string> buff_time = new List<string>(); //Буфер текущего времени воспроизведения
            string currentTime = "0"; //Текущее время воспроизведения

            while (true)
            {
                try
                {
                    //Буферизация данных
                    while (ms.Length < mp3_buffSize * 4)
                    {
                        mp3_message = "MP3 Player: buffering...";
                        Thread.Sleep(10);
                    }
                    reader = new Mp3FileReader(ms);
                    //var device = naudio_deviceEnumerator.GetDevice(naudio_outputDeviceId[sound_playDeviceId]);
                    AudioIO.naudio_wasapiOut = new WasapiOut(AudioClientShareMode.Shared, true, 50);
                    AudioIO.naudio_wasapiOut.Init(reader);
                    AudioIO.naudio_wasapiOut.Play();
                    while (AudioIO.naudio_wasapiOut.PlaybackState == PlaybackState.Playing)
                    {
                        //mp3_status = "PLAY";
                        mp3_message = "MP3 Player: " + AudioIO.naudio_wasapiOut.PlaybackState.ToString();
                        currentTime = reader.CurrentTime.Minutes.ToString() + ":" + reader.CurrentTime.Seconds.ToString() + ":" + reader.CurrentTime.Milliseconds.ToString();
                        //mp3_currentTime = currentTime;
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
                    //mp3_status = "STOP";
                }
                catch (Exception ex)
                {
                    mp3_message = "MP3 Player: " + ex.Message;
                    //mp3_currentTime = "--:--:--";
                    ms = new MemoryStream();
                    Thread.Sleep(10);
                }
            }
        }
    }
}
