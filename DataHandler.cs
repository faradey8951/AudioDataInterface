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
        public static Thread thread_bufferMp3 = null;
        public static Thread thread_playMp3 = null;
        public MemoryStream ms;
        int mp3_buffSize = 512;
        public string mp3_message = "";
        public static string mp3_currentTime = "";
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
                lock (Decoder.buff_decodedData) Decoder.buff_decodedData.RemoveRange(0, mp3_buffSize);
                i = 0;
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = pos;
                Thread.Sleep(100);
            }
        }

        public static void StartMp3Listening()
        {
            thread_bufferMp3 = new Thread(form_main.class_dataHandler.BufferMp3);
            thread_bufferMp3.Start();
            thread_playMp3 = new Thread(form_main.class_dataHandler.PlayMp3);
            thread_playMp3.Start();
        }

        //Метод очистки mp3 данных
        public void ClearMp3()
        {
            ms = new MemoryStream();
            AudioIO.naudio_wasapiOut.Dispose();
            lock (AudioIO.buff_signalBytes) AudioIO.buff_signalBytes.RemoveRange(0, 2);
            lock (AudioIO.buff_graphSamples) AudioIO.buff_graphSamples.Clear();
            lock (Decoder.buff_signalAmplitudesL) Decoder.buff_signalAmplitudesL.Clear();
            lock (Decoder.buff_signalAmplitudesR) Decoder.buff_signalAmplitudesR.Clear();
            lock (Decoder.buff_decodedData) Decoder.buff_decodedData.Clear();
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
                        mp3_currentTime = currentTime;
                        //Если воспроизведение прервалось
                        
                        //mp3_currentTime = reader.HasData(2).ToString();                     
                        //mp3_currentTime = frame.ToString();
                        //mp3_currentTime = reader.Mp3WaveFormat.AverageBytesPerSecond.ToString();
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
                    ClearMp3();
                    Thread.Sleep(10);
                }
            }
        }
    }
}
