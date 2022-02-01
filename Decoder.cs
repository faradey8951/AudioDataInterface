using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AudioDataInterface
{
    public class Decoder
    {
        public static List<short> buff_signalAmplitudes = new List<short>();

        static void GetAmplitudesFromSamples()
        {
            var tempList = new List<short>(); //Временный буфер сэмплов
            while (true)
            {
                {
                    while (AudioIO.buff_signalSamples.Count < 128000)
                        Thread.Sleep(10);
                    if (tempList.Count != 0)
                        tempList.Clear();
                    //Удалить сэмплы отрицательного полюса
                    while (AudioIO.buff_signalSamples[0] < 0)
                    {
                        AudioIO.buff_signalSamples.RemoveAt(0);
                        while (AudioIO.buff_signalSamples.Count == 0)
                            Thread.Sleep(10);
                    }
                    while (AudioIO.buff_signalSamples.Count < 128000)
                        Thread.Sleep(10);
                    //Переместить сэмплы положительного полюса во временный буфер
                    while (AudioIO.buff_signalSamples[0] >= 0 && tempList.Count < 128000)
                    {
                        tempList.Add(AudioIO.buff_signalSamples[0]);
                        AudioIO.buff_signalSamples.RemoveAt(0);
                        while (AudioIO.buff_signalSamples.Count == 0)
                            Thread.Sleep(10);
                    }
                    //Записать амплитуду в буфер
                    if (tempList.Count != 0)
                        buff_signalAmplitudes.Add(tempList.Max());
                    tempList.Clear();
                }
            }
        }

        public static void Start()
        {
            Thread t = new Thread(GetAmplitudesFromSamples);
            t.Start();
        }
    }
}
