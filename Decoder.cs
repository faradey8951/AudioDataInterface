using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AudioDataInterface
{
    public class Decoder
    {
        public static readonly List<short> buff_signalAmplitudes = new List<short>();
        public static readonly List<short> buff_signalAmplitudesL = new List<short>();
        public static readonly List<short> buff_signalAmplitudesR = new List<short>();
        public static readonly List<string[]> buff_decodedData = new List<string[]>(); //Буфер декодированных данных
        public static Thread thread_binaryDecoder = null;
        public static Thread thread_amplitudeDecoder = null;
        public static Thread thread_amplitudeDecoderL = null;
        public static Thread thread_amplitudeDecoderR = null;
        public static Thread thread_samplesDecoder = null;
        public static Thread thread_samplesDecoderStereo = null;
        public static short maxAmplitude = 0;

        static void SamplesDecoder()
        {
            while (true)
            {
                while (AudioIO.buff_signalBytes.Count < 16) Thread.Sleep(10);
                if (!AudioIO.audio_invertSignal) lock (AudioIO.buff_signalSamples) AudioIO.buff_signalSamples.Add((short)(AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                else lock (AudioIO.buff_signalSamples) AudioIO.buff_signalSamples.Add((short)(-AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                AudioIO.buff_signalBytes.RemoveRange(0, 2);
            }
        }

        static void SamplesDecoderStereo()
        {
            while (true)
            {
                while (AudioIO.buff_signalBytes.Count < 128) Thread.Sleep(10);
                if (!AudioIO.audio_invertSignal)
                {
                    lock (AudioIO.buff_signalSamples)
                    {
                        AudioIO.buff_signalSamples.Add((short)(AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                        AudioIO.buff_signalSamples.Add((short)(AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[2], AudioIO.buff_signalBytes[3] }, 0) + (short)AudioIO.audio_signalHeight));
                    }
                }
                else
                {
                    lock (AudioIO.buff_signalSamples)
                    {
                        AudioIO.buff_signalSamples.Add((short)(-AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                        AudioIO.buff_signalSamples.Add((short)(-AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[2], AudioIO.buff_signalBytes[3] }, 0) + (short)AudioIO.audio_signalHeight));
                    }
                }
                lock (AudioIO.buff_signalBytes) AudioIO.buff_signalBytes.RemoveRange(0, 4);
            }
        }

        static void AmplitudeDecoderL()
        {
            var tempList = new List<short>(); //Временный буфер сэмплов
            while (true)
            {

                while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamplesL[0] < 0)
                {
                    lock (AudioIO.buff_signalSamplesL) AudioIO.buff_signalSamplesL.RemoveAt(0);
                    while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                }
                while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamplesL[0] >= 0)
                {
                    lock (AudioIO.buff_signalSamplesL)
                    {
                        tempList.Add(AudioIO.buff_signalSamplesL[0]);
                        AudioIO.buff_signalSamplesL.RemoveAt(0);
                    }
                    //if (tempList.Count >= 512)
                    //{
                    //tempList.Clear();
                    //break;
                    //}
                    while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                }
                lock (buff_signalAmplitudesL) buff_signalAmplitudesL.Add(tempList.Max());
                tempList.Clear();
            }
        }

        static void AmplitudeDecoderR()
        {
            var tempList = new List<short>(); //Временный буфер сэмплов
            while (true)
            {

                while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamplesR[0] < 0)
                {
                    lock (AudioIO.buff_signalSamplesR) AudioIO.buff_signalSamplesR.RemoveAt(0);
                    while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                }
                while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamplesR[0] >= 0)
                {
                    lock (AudioIO.buff_signalSamplesR)
                    {
                        tempList.Add(AudioIO.buff_signalSamplesR[0]);
                        AudioIO.buff_signalSamplesR.RemoveAt(0);
                    }
                    //if (tempList.Count >= 512)
                    //{
                    //tempList.Clear();
                    //break;
                    //}
                    while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                }
                lock (buff_signalAmplitudesR) buff_signalAmplitudesR.Add(tempList.Max());
                tempList.Clear();
            }
        }

        static void AmplitudeDecoder()
        {
            var tempList = new List<short>(); //Временный буфер сэмплов
            var tempListL = new List<short>(); //Временный буфер сэмплов
            var tempListR = new List<short>(); //Временный буфер сэмплов
            while (true)
            {
                var l = new List<short>();
                var r = new List<short>();
                while (AudioIO.buff_signalSamples.Count < 3) Thread.Sleep(10);
                while (AudioIO.buff_signalSamples[0] < 0)
                {
                    lock (AudioIO.buff_signalSamples)
                    {
                        AudioIO.buff_signalSamples.RemoveRange(0, 2);
                    }
                    while (AudioIO.buff_signalSamples.Count < 3) Thread.Sleep(10);
                }
                while (AudioIO.buff_signalSamples.Count < 3) Thread.Sleep(10);
                while (AudioIO.buff_signalSamples[0] >= 0)
                {
                    lock (AudioIO.buff_signalSamples)
                    {
                        tempListL.Add(AudioIO.buff_signalSamples[0]);
                        tempListR.Add(AudioIO.buff_signalSamples[1]);
                        AudioIO.buff_signalSamples.RemoveRange(0, 2);
                    }
                    if (tempList.Count >= 512)
                    {
                        tempListL.Clear();
                        tempListR.Clear();
                        break;
                    }
                    while (AudioIO.buff_signalSamples.Count < 3) Thread.Sleep(10);
                }
                //Сепарация стерео потока
                if (tempListL.Count >= 2 && tempListL.Count < 512)
                {
                    lock (buff_signalAmplitudes)
                    {
                        //Слияние стерео сигнала
                        buff_signalAmplitudes.Add(tempListL.Max());
                        buff_signalAmplitudes.Add(tempListR.Max());
                    }
                }
                //lock (buff_signalAmplitudes) buff_signalAmplitudes.Add(tempList.Max());
                tempListL.Clear();
                tempListR.Clear();
            }
        }

        static void BinaryDecoderStereo()
        {
            //Установить приоритет процесса на максимум
            System.Diagnostics.Process thisProc = System.Diagnostics.Process.GetCurrentProcess();
            thisProc.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

            string tempBin = ""; //Временное бинарное слово
            var amplitudeBuff = new List<short>(); //Буфер амплитуд
            var amplitudeBuffCopy = new List<short>(); //Копия буфера амплитуд
            var amplitudeBuffL = new List<short>(); //Буфер амплитуд
            var amplitudeBuffR = new List<short>(); //Буфер амплитуд
            var tempSyncIndexes = new List<int>(); //Список индексов синхроимпульсов
            var tempOneIndexes = new List<int>(); //Список индексов единиц
            bool sync = true; //Показатель обнаружения синхронизации
            var dataBlockBuff = new List<short>(); //Буфер амплитуд блока данных
            var dataBlockBuffLnr = new List<short>(); //Сглаженный буфер амплитуд блока данных
            var dataBlockBuffCopy = new List<short>(); //Копия буфера амплитуд блока данных
            int syncPulseIndex = 0;
            int maxSyncPulseIndex = 0;
            int maxSyncPulse = 0;
            int syncPulse = 0;
            int syncPulseL = 0; //Второй синхроимпульс блока данных
            int maxSyncPulseL = 0; //Первый (максимальный) синхроимпульс блока данных
            int syncPulseIndexL = 0; //Индекс второго синхроимпульса
            int maxSyncPulseIndexL = 0; //Индекс максимального синхроимпульса
            int syncPulseR = 0; //Второй синхроимпульс блока данных
            int maxSyncPulseR = 0; //Первый (максимальный) синхроимпульс блока данных
            int syncPulseIndexR = 0; //Индекс второго синхроимпульса
            int maxSyncPulseIndexR = 0; //Индекс максимального синхроимпульса
            int difference = 0; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса

            int channelSwitch = 0;

            while (true)
            {
                if (channelSwitch == 0) while (buff_signalAmplitudes.Count < 160) Thread.Sleep(10);
                else while (buff_signalAmplitudes.Count < 160) Thread.Sleep(10);

                tempBin = "";
                amplitudeBuff.Clear();
                amplitudeBuffCopy.Clear();
                amplitudeBuffL.Clear();
                amplitudeBuffR.Clear();
                tempSyncIndexes.Clear();
                tempOneIndexes.Clear();
                sync = true;
                dataBlockBuff.Clear();
                dataBlockBuffLnr.Clear();
                dataBlockBuffCopy.Clear();
                syncPulseL = 0;
                maxSyncPulseL = 0;
                syncPulseIndexL = 0;
                maxSyncPulseIndexL = 0;
                syncPulseR = 0;
                maxSyncPulseR = 0;
                syncPulseIndexR = 0;
                maxSyncPulseIndexR = 0;
                difference = 0;
                channelSwitch = 0;

                if (channelSwitch == 0)
                {
                    if (buff_signalAmplitudes.Count >= 160)
                    {
                        for (int i = 0; i < 160; i++) lock (buff_signalAmplitudes) amplitudeBuff.Add(buff_signalAmplitudes[i]);
                        //Сепарация стерео сигнала
                        for (int i = 0; i < amplitudeBuff.Count - 1; i += 2) amplitudeBuffL.Add(amplitudeBuff[i]);
                        for (int i = 1; i <= amplitudeBuff.Count; i += 2) amplitudeBuffR.Add(amplitudeBuff[i]);
                        amplitudeBuffCopy.AddRange(amplitudeBuffL);
                        maxSyncPulseL = amplitudeBuffCopy.Max(); //Максимальная амплитуда буфера
                        maxSyncPulseIndexL = amplitudeBuffCopy.IndexOf((short)maxSyncPulseL); //Индекс максимальной амплитуды
                        amplitudeBuffCopy[maxSyncPulseIndexL] = 0; //Занулить максимальную амплитуду
                        maxAmplitude = (short)maxSyncPulseL;
                        for (int i = 0; i < amplitudeBuffCopy.Count;) //Парсинг синхроимпульсов
                        {
                            syncPulseL = amplitudeBuffCopy.Max(); //Теоретическая амплитуда второго синхроимпульса
                            syncPulseIndexL = amplitudeBuffCopy.IndexOf((short)syncPulseL); //Индекс теоретической амплитуды второго сиенхроимпульса
                            if (maxSyncPulseIndexL > syncPulseIndexL)
                                difference = maxSyncPulseIndexL - syncPulseIndexL - 1; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса
                            else
                                difference = syncPulseIndexL - maxSyncPulseIndexL - 1; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса
                            amplitudeBuffCopy[syncPulseIndexL] = 0; //Занулить максимальную амплитуду буфера
                            if (difference == 39)
                            {
                                channelSwitch = 1;
                                break;
                            }
                            //else
                            //{
                                //sync = false;
                                //break;
                            //}
                                i++;
                            //Если второй синхроимпульс не обнаружен
                            if (i == amplitudeBuffCopy.Count) sync = false;
                            else channelSwitch = 0;
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                        sync = false;
                    }
                }
                if (channelSwitch == 1)
                {
                    amplitudeBuffCopy.Clear();
                    amplitudeBuffCopy.AddRange(amplitudeBuffR);
                    maxSyncPulseR = amplitudeBuffCopy.Max(); //Максимальная амплитуда буфера
                    maxSyncPulseIndexR = amplitudeBuffCopy.IndexOf((short)maxSyncPulseR); //Индекс максимальной амплитуды
                    amplitudeBuffCopy[maxSyncPulseIndexR] = 0; //Занулить максимальную амплитуду
                    maxAmplitude = (short)maxSyncPulseR;
                    for (int i = 0; i < amplitudeBuffCopy.Count;) //Парсинг синхроимпульсов
                    {
                        syncPulseR = amplitudeBuffCopy.Max(); //Теоретическая амплитуда второго синхроимпульса
                        syncPulseIndexR = amplitudeBuffCopy.IndexOf((short)syncPulseR); //Индекс теоретической амплитуды второго сиенхроимпульса
                        if (maxSyncPulseIndexR > syncPulseIndexR)
                            difference = maxSyncPulseIndexR - syncPulseIndexR - 1; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса
                        else
                            difference = syncPulseIndexR - maxSyncPulseIndexR - 1; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса
                        amplitudeBuffCopy[syncPulseIndexR] = 0; //Занулить максимальную амплитуду буфера
                        if (difference == 39)
                        {
                            sync = true;
                            break;
                        }
                        //else
                        //{
                            //sync = false;
                            //break;
                        //}
                        i++;
                        //Если второй синхроимпульс не обнаружен
                        if (i == amplitudeBuffCopy.Count) sync = false;
                    }
                }

                if (sync == true && channelSwitch == 1) //Если синхронизация обнаружена
                {
                    for (int c = 0; c < 2; c++)
                    {
                        dataBlockBuff.Clear();
                        tempSyncIndexes.Clear();
                        tempOneIndexes.Clear();
                        dataBlockBuffLnr.Clear();
                        tempBin = "";
                        if (c == 0)
                        {
                            syncPulseIndex = syncPulseIndexL;
                            amplitudeBuff.Clear();
                            amplitudeBuff.AddRange(amplitudeBuffL);
                            maxSyncPulse = maxSyncPulseL;
                            maxSyncPulseIndex = maxSyncPulseIndexL;
                            syncPulse = syncPulseL;
                        }
                        else
                        {
                            syncPulseIndex = syncPulseIndexR;
                            amplitudeBuff.Clear();
                            amplitudeBuff.AddRange(amplitudeBuffR);
                            maxSyncPulse = maxSyncPulseR;
                            maxSyncPulseIndex = maxSyncPulseIndexR;
                            syncPulse = syncPulseR;
                        }
                        tempSyncIndexes.Add(syncPulseIndex);
                        tempSyncIndexes.Add(maxSyncPulseIndex);
                        tempSyncIndexes.Sort(); //Сортировка индексов синхроимпульсов по возрастанию
                        for (int i = tempSyncIndexes[0] + 1; i < tempSyncIndexes[1]; i++) //Составляем буфер данных между синхроимпульсами
                        {
                            dataBlockBuff.Add(amplitudeBuff[i]);
                            dataBlockBuffLnr.Add(amplitudeBuff[i]);
                        }
                        lock (buff_decodedData)
                        {
                            buff_decodedData.Add(new string[10]);
                            buff_decodedData.Last()[0] = String.Join(":", amplitudeBuff);
                            buff_decodedData.Last()[1] = String.Join(":", dataBlockBuff);
                        }
                        double lnrGain = 0.25; //Коэффициент усиления линеаризации
                        double lnrCorrLength = 0.75; //Часть амплитуд, подвергаемая линеаризации
                        int lnrTrendLevelLimit = 3000; //Порог уровня тренда 
                        if (dataBlockBuff.Count == 39)
                        {
                            int lnrTrendLevel = maxSyncPulse - syncPulse; //Уровень тренда                    
                            int lnrCorrIncr = (int)(lnrTrendLevel * lnrGain) / (int)(dataBlockBuff.Count * lnrCorrLength); //Приращение амплитуды
                            if (lnrTrendLevel >= lnrTrendLevelLimit)
                            {
                                if (amplitudeBuff[tempSyncIndexes[1]] > amplitudeBuff[tempSyncIndexes[0]]) //Если амплитуда второго синхроимпульса больше амплитуды первого - тренд растущий
                                {
                                    for (int i = (int)(lnrTrendLevel * lnrGain), k = dataBlockBuff.Count - 1; i > 0 && k >= 0; i -= lnrCorrIncr, k--)
                                        dataBlockBuffLnr[k] = (short)(dataBlockBuffLnr[k] - i);
                                }
                                else //Тренд убывающий
                                {
                                    for (int i = (int)(lnrTrendLevel * lnrGain), k = 0; i > 0 && k < dataBlockBuff.Count; i -= lnrCorrIncr, k++)
                                        dataBlockBuffLnr[k] = (short)(dataBlockBuffLnr[k] - i);
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                        lock (buff_decodedData) buff_decodedData.Last()[2] = String.Join(":", dataBlockBuffLnr);

                        if (dataBlockBuffLnr.Min() >= 0) //Проверка выхода линеаризатора за пределы границ
                        {
                            dataBlockBuff.Clear();
                            dataBlockBuff.AddRange(dataBlockBuffLnr);
                            lock (buff_decodedData) buff_decodedData.Last()[3] = "True";
                        }
                        else //Если линеаризатор вышел за границы
                        {
                            dataBlockBuffLnr.Clear();
                            dataBlockBuffLnr.AddRange(dataBlockBuff);
                            lock (buff_decodedData) buff_decodedData.Last()[3] = "False";
                        }
                        if (dataBlockBuff.Count == 39) //Если блок данных обнаружен
                        {
                            dataBlockBuffCopy.AddRange(dataBlockBuff); //Копируем буфер амплитуд
                            int oneLevel = dataBlockBuff.Max(); //Определяем первую максимальную амплитуду в буфере
                            dataBlockBuffCopy[dataBlockBuff.IndexOf((short)oneLevel)] = 0; //Зануляем первую максимальную амплитуду в копии буфера
                            int trapPercent = 20 + (int)(((double)oneLevel / (double)syncPulse) * 90); //Степень понижения амплитуды [%]
                            if (trapPercent < 30 && trapPercent > 90)
                                trapPercent = 70;
                            var aplitudeDynamics = new List<string>(); //Динамика амплитуд
                            aplitudeDynamics.Add("");
                            bool approxSuccess = true;
                            lock (buff_decodedData) buff_decodedData.Last()[4] = trapPercent.ToString();
                            for (int k = 0; k < 1; k++) //Аппроксимация двоичного сигнала (работа над буфером dataBlockBuffLnr)
                            {
                                for (int i = 0; i < dataBlockBuff.Count - 1; i++)
                                {
                                    if (dataBlockBuff[i] != 0 && dataBlockBuff[i + 1] != 0)
                                    {
                                        if (dataBlockBuff[i + 1] >= dataBlockBuff[i]) //Если пара амплитуд растущая
                                        {
                                            int perc = (int)((dataBlockBuff[i] * 100) / dataBlockBuff[i + 1]);
                                            if (perc > trapPercent) //Если обе амплитуды совпадают
                                                dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                            else //Если амплитуды отличаются
                                            {
                                                if (aplitudeDynamics.Last() == "+") //Если, с учётом динамики, предыдущая пара амплитуд также была растущей => аппроксимация далее невозможна!
                                                {
                                                    dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                                    approxSuccess = false;
                                                }
                                                else //Если аппроксимация адекватна
                                                {
                                                    dataBlockBuffLnr[i + 1] = (short)(dataBlockBuffLnr[i] * 2);
                                                    aplitudeDynamics.Add("+");
                                                }
                                            }
                                        }
                                        else //Если пара амплитуд убывающая
                                        {
                                            int perc = (int)((dataBlockBuff[i + 1] * 100) / dataBlockBuff[i]);
                                            if (perc > trapPercent) //Если обе амплитуды совпадают
                                                dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                            else //Если амплитуды отличаются
                                            {
                                                if (aplitudeDynamics.Last() == "-") //Если, с учётом динамики, предыдущая пара амплитуд также была убывающей => аппроксимация далее невозможна!
                                                {
                                                    dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                                    approxSuccess = false;
                                                }
                                                else //Если аппроксимация адекватна
                                                {
                                                    dataBlockBuffLnr[i + 1] = (short)(dataBlockBuffLnr[i] / 2);
                                                    aplitudeDynamics.Add("-");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            lock (buff_decodedData)
                            {
                                buff_decodedData.Last()[5] = String.Join(":", dataBlockBuffLnr);
                                buff_decodedData.Last()[6] = approxSuccess.ToString();
                            }
                            if (approxSuccess == true) //Если аппроксимация успешна
                            {
                                dataBlockBuff.Clear();
                                dataBlockBuff.AddRange(dataBlockBuffLnr);
                                int one = dataBlockBuff.Max(); //Амплитуда единицы                           
                                for (int i = 0; i < dataBlockBuff.Count; i++) //Парсинг единиц. Запись индексов единиц в список
                                {
                                    if (dataBlockBuff[i] == one)
                                        tempOneIndexes.Add(i);
                                    else
                                    {
                                        int approxEqualPercentOneLevel = (int)(((double)dataBlockBuff[i] / (double)one) * 100); //Сравнение текущей амплитуды с единичной                                    
                                        if (approxEqualPercentOneLevel >= 90) //Учёт несовпадения значений объективно одинаковых амплитуд
                                            tempOneIndexes.Add(i);
                                    }
                                }
                                tempOneIndexes.Sort(); //Сортировка индексов единиц по возрастанию
                            }
                            else //Если аппроксимация не удалась - использовать альтернативный алгоритм кластеризации
                            {
                                int aClustMax = dataBlockBuff.Max();
                                int aClustMin = dataBlockBuff.Min();
                                int aClustMidLine = (int)(aClustMax - ((aClustMax - aClustMin) / 2));
                                lock (buff_decodedData) buff_decodedData.Last()[7] = aClustMidLine.ToString();
                                for (int i = 0; i < dataBlockBuff.Count; i++)
                                {
                                    if (dataBlockBuff[i] >= aClustMidLine)
                                        tempOneIndexes.Add(i);
                                }
                            }
                            //Составляем финальную бинарную последовательность, учитывая существующие индексные списки
                            if (tempOneIndexes.Count == dataBlockBuff.Count) //Учитываем случай односоставного блока данных
                            {
                                for (int i = 0; i < dataBlockBuff.Count; i++)
                                    tempBin += "0";
                            }
                            else
                            {
                                for (int i = 0; i < dataBlockBuff.Count; i++)
                                {
                                    if (tempOneIndexes.Contains(i))
                                        tempBin += "1";
                                    else
                                        tempBin += "0";
                                }
                            }
                            BinaryDecode(tempBin);
                        }
                        //else
                            //Thread.Sleep(10);                    
                    }
                    lock (buff_signalAmplitudes) buff_signalAmplitudes.RemoveRange(0, (tempSyncIndexes[1] * 2)); //Удалить отработанные амплитуды
                }
                else
                {
                    if (buff_signalAmplitudes.Count > 0) lock (buff_signalAmplitudes) buff_signalAmplitudes.RemoveRange(0, buff_signalAmplitudes.Count);
                    //Thread.Sleep(10);
                }
            }
        }

        //Метод получения первичного бинарного кода из амплитуд входного сигнала
        static void BinaryDecoder()
        {
            //Установить приоритет процесса на максимум
            System.Diagnostics.Process thisProc = System.Diagnostics.Process.GetCurrentProcess();
            thisProc.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

            string tempBin = ""; //Временное бинарное слово
            var amplitudeBuff = new List<short>(); //Буфер амплитуд
            var amplitudeBuffCopy = new List<short>(); //Копия буфера амплитуд
            var tempSyncIndexes = new List<int>(); //Список индексов синхроимпульсов
            var tempOneIndexes = new List<int>(); //Список индексов единиц
            bool sync = true; //Показатель обнаружения синхронизации
            var dataBlockBuff = new List<short>(); //Буфер амплитуд блока данных
            var dataBlockBuffLnr = new List<short>(); //Сглаженный буфер амплитуд блока данных
            var dataBlockBuffCopy = new List<short>(); //Копия буфера амплитуд блока данных
            int syncPulse = 0; //Второй синхроимпульс блока данных
            int maxSyncPulse = 0; //Первый (максимальный) синхроимпульс блока данных
            int syncPulseIndex = 0; //Индекс второго синхроимпульса
            int maxSyncPulseIndex = 0; //Индекс максимального синхроимпульса
            int difference = 0; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса

            int channelSwitch = 0;

            while (true)
            {
                while (buff_signalAmplitudes.Count < 80) Thread.Sleep(10);

                tempBin = "";
                amplitudeBuff.Clear();
                amplitudeBuffCopy.Clear();
                tempSyncIndexes.Clear();
                tempOneIndexes.Clear();
                sync = true;
                dataBlockBuff.Clear();
                dataBlockBuffLnr.Clear();
                dataBlockBuffCopy.Clear();
                syncPulse = 0;
                maxSyncPulse = 0;
                syncPulseIndex = 0;
                maxSyncPulseIndex = 0;
                difference = 0;
                if (buff_signalAmplitudes.Count >= 80)
                {
                    for (int i = 0; i < 80; i++) lock (buff_signalAmplitudes) amplitudeBuff.Add(buff_signalAmplitudes[i]);
                    amplitudeBuffCopy.AddRange(amplitudeBuff);
                    maxSyncPulse = amplitudeBuffCopy.Max(); //Максимальная амплитуда буфера
                    maxSyncPulseIndex = amplitudeBuffCopy.IndexOf((short)maxSyncPulse); //Индекс максимальной амплитуды
                    amplitudeBuffCopy[maxSyncPulseIndex] = 0; //Занулить максимальную амплитуду
                    maxAmplitude = (short)maxSyncPulse;
                    for (int i = 0; i < amplitudeBuffCopy.Count;) //Парсинг синхроимпульсов
                    {
                        syncPulse = amplitudeBuffCopy.Max(); //Теоретическая амплитуда второго синхроимпульса
                        syncPulseIndex = amplitudeBuffCopy.IndexOf((short)syncPulse); //Индекс теоретической амплитуды второго сиенхроимпульса
                        if (maxSyncPulseIndex > syncPulseIndex)
                            difference = maxSyncPulseIndex - syncPulseIndex - 1; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса
                        else
                            difference = syncPulseIndex - maxSyncPulseIndex - 1; //Кол-во амплитуд между максимальным синхроимпульсом и теоретической амплитудой второго синхроимпульса
                        amplitudeBuffCopy[syncPulseIndex] = 0; //Занулить максимальную амплитуду буфера
                        if (difference == 39) break;
                        else 
                            //Thread.Sleep(10);
                        i++;
                        //Если второй синхроимпульс не обнаружен
                        if (i == amplitudeBuffCopy.Count)
                            sync = false;
                    }
                }
                else
                {
                    Thread.Sleep(10);
                    sync = false;
                }

                if (sync == true) //Если синхронизация обнаружена
                {
                    tempSyncIndexes.Add(syncPulseIndex);
                    tempSyncIndexes.Add(maxSyncPulseIndex);
                    tempSyncIndexes.Sort(); //Сортировка индексов синхроимпульсов по возрастанию
                    for (int i = tempSyncIndexes[0] + 1; i < tempSyncIndexes[1]; i++) //Составляем буфер данных между синхроимпульсами
                    {
                        dataBlockBuff.Add(amplitudeBuff[i]);
                        dataBlockBuffLnr.Add(amplitudeBuff[i]);
                    }
                    lock (buff_decodedData)
                    {
                        buff_decodedData.Add(new string[10]);
                        buff_decodedData.Last()[0] = String.Join(":", amplitudeBuff);
                        buff_decodedData.Last()[1] = String.Join(":", dataBlockBuff);
                    }
                    double lnrGain = 0.25; //Коэффициент усиления линеаризации
                    double lnrCorrLength = 0.75; //Часть амплитуд, подвергаемая линеаризации
                    int lnrTrendLevelLimit = 3000; //Порог уровня тренда 
                    if (dataBlockBuff.Count == 39)
                    {
                        int lnrTrendLevel = maxSyncPulse - syncPulse; //Уровень тренда                    
                        int lnrCorrIncr = (int)(lnrTrendLevel * lnrGain) / (int)(dataBlockBuff.Count * lnrCorrLength); //Приращение амплитуды
                        if (lnrTrendLevel >= lnrTrendLevelLimit)
                        {
                            if (amplitudeBuff[tempSyncIndexes[1]] > amplitudeBuff[tempSyncIndexes[0]]) //Если амплитуда второго синхроимпульса больше амплитуды первого - тренд растущий
                            {
                                for (int i = (int)(lnrTrendLevel * lnrGain), k = dataBlockBuff.Count - 1; i > 0 && k >= 0; i -= lnrCorrIncr, k--)
                                    dataBlockBuffLnr[k] = (short)(dataBlockBuffLnr[k] - i);
                            }
                            else //Тренд убывающий
                            {
                                for (int i = (int)(lnrTrendLevel * lnrGain), k = 0; i > 0 && k < dataBlockBuff.Count; i -= lnrCorrIncr, k++)
                                    dataBlockBuffLnr[k] = (short)(dataBlockBuffLnr[k] - i);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    lock (buff_decodedData) buff_decodedData.Last()[2] = String.Join(":", dataBlockBuffLnr);

                    if (dataBlockBuffLnr.Min() >= 0) //Проверка выхода линеаризатора за пределы границ
                    {
                        dataBlockBuff.Clear();
                        dataBlockBuff.AddRange(dataBlockBuffLnr);
                        lock (buff_decodedData) buff_decodedData.Last()[3] = "True";
                    }
                    else //Если линеаризатор вышел за границы
                    {
                        dataBlockBuffLnr.Clear();
                        dataBlockBuffLnr.AddRange(dataBlockBuff);
                        lock (buff_decodedData) buff_decodedData.Last()[3] = "False";
                    }
                    if (dataBlockBuff.Count == 39) //Если блок данных обнаружен
                    {
                        dataBlockBuffCopy.AddRange(dataBlockBuff); //Копируем буфер амплитуд
                        int oneLevel = dataBlockBuff.Max(); //Определяем первую максимальную амплитуду в буфере
                        dataBlockBuffCopy[dataBlockBuff.IndexOf((short)oneLevel)] = 0; //Зануляем первую максимальную амплитуду в копии буфера
                        int trapPercent = 20 + (int)(((double)oneLevel / (double)syncPulse) * 90); //Степень понижения амплитуды [%]
                        if (trapPercent < 30 && trapPercent > 90)
                            trapPercent = 70;
                        var aplitudeDynamics = new List<string>(); //Динамика амплитуд
                        aplitudeDynamics.Add("");
                        bool approxSuccess = true;
                        lock (buff_decodedData) buff_decodedData.Last()[4] = trapPercent.ToString();
                        for (int k = 0; k < 1; k++) //Аппроксимация двоичного сигнала (работа над буфером dataBlockBuffLnr)
                        {
                            for (int i = 0; i < dataBlockBuff.Count - 1; i++)
                            {
                                if (dataBlockBuff[i] != 0 && dataBlockBuff[i + 1] != 0)
                                {
                                    if (dataBlockBuff[i + 1] >= dataBlockBuff[i]) //Если пара амплитуд растущая
                                    {
                                        int perc = (int)((dataBlockBuff[i] * 100) / dataBlockBuff[i + 1]);
                                        if (perc > trapPercent) //Если обе амплитуды совпадают
                                            dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                        else //Если амплитуды отличаются
                                        {
                                            if (aplitudeDynamics.Last() == "+") //Если, с учётом динамики, предыдущая пара амплитуд также была растущей => аппроксимация далее невозможна!
                                            {
                                                dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                                approxSuccess = false;
                                            }
                                            else //Если аппроксимация адекватна
                                            {
                                                dataBlockBuffLnr[i + 1] = (short)(dataBlockBuffLnr[i] * 2);
                                                aplitudeDynamics.Add("+");
                                            }
                                        }
                                    }
                                    else //Если пара амплитуд убывающая
                                    {
                                        int perc = (int)((dataBlockBuff[i + 1] * 100) / dataBlockBuff[i]);
                                        if (perc > trapPercent) //Если обе амплитуды совпадают
                                            dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                        else //Если амплитуды отличаются
                                        {
                                            if (aplitudeDynamics.Last() == "-") //Если, с учётом динамики, предыдущая пара амплитуд также была убывающей => аппроксимация далее невозможна!
                                            {
                                                dataBlockBuffLnr[i + 1] = dataBlockBuffLnr[i];
                                                approxSuccess = false;
                                            }
                                            else //Если аппроксимация адекватна
                                            {
                                                dataBlockBuffLnr[i + 1] = (short)(dataBlockBuffLnr[i] / 2);
                                                aplitudeDynamics.Add("-");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        lock (buff_decodedData)
                        {
                            buff_decodedData.Last()[5] = String.Join(":", dataBlockBuffLnr);
                            buff_decodedData.Last()[6] = approxSuccess.ToString();
                        }
                        if (approxSuccess == true) //Если аппроксимация успешна
                        {
                            dataBlockBuff.Clear();
                            dataBlockBuff.AddRange(dataBlockBuffLnr);
                            int one = dataBlockBuff.Max(); //Амплитуда единицы                           
                            for (int i = 0; i < dataBlockBuff.Count; i++) //Парсинг единиц. Запись индексов единиц в список
                            {
                                if (dataBlockBuff[i] == one)
                                    tempOneIndexes.Add(i);
                                else
                                {
                                    int approxEqualPercentOneLevel = (int)(((double)dataBlockBuff[i] / (double)one) * 100); //Сравнение текущей амплитуды с единичной                                    
                                    if (approxEqualPercentOneLevel >= 90) //Учёт несовпадения значений объективно одинаковых амплитуд
                                        tempOneIndexes.Add(i);
                                }
                            }
                            tempOneIndexes.Sort(); //Сортировка индексов единиц по возрастанию
                        }
                        else //Если аппроксимация не удалась - использовать альтернативный алгоритм кластеризации
                        {
                            int aClustMax = dataBlockBuff.Max();
                            int aClustMin = dataBlockBuff.Min();
                            int aClustMidLine = (int)(aClustMax - ((aClustMax - aClustMin) / 2));
                            lock (buff_decodedData) buff_decodedData.Last()[7] = aClustMidLine.ToString();
                            for (int i = 0; i < dataBlockBuff.Count; i++)
                            {
                                if (dataBlockBuff[i] >= aClustMidLine)
                                    tempOneIndexes.Add(i);
                            }
                        }
                        //Составляем финальную бинарную последовательность, учитывая существующие индексные списки
                        if (tempOneIndexes.Count == dataBlockBuff.Count) //Учитываем случай односоставного блока данных
                        {
                            for (int i = 0; i < dataBlockBuff.Count; i++)
                                tempBin += "0";
                        }
                        else
                        {
                            for (int i = 0; i < dataBlockBuff.Count; i++)
                            {
                                if (tempOneIndexes.Contains(i))
                                    tempBin += "1";
                                else
                                    tempBin += "0";
                            }
                        }
                        BinaryDecode(tempBin);
                    }
                    else
                        Thread.Sleep(10);
                    lock (buff_signalAmplitudes) buff_signalAmplitudes.RemoveRange(0, tempSyncIndexes[1] - 1); //Удалить отработанные амплитуды
                }
                else
                {
                    if (buff_signalAmplitudes.Count > 0) lock (buff_signalAmplitudes) buff_signalAmplitudes.RemoveRange(0, buff_signalAmplitudes.Count - 1);
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        public static void BinaryDecode(string bin)
        {
            lock (buff_decodedData) buff_decodedData.Last()[8] = bin;
            string data = ""; //Данные
            if (Convert.ToString(bin.Last()) == "0") //Блок данных - RawData
            {              
                bin = bin.Remove(38); //Удаляем бит-маркер
                data = BinaryHandler.HammingDecode(bin); //Декодируем блок данных по Хэммингу
                lock (buff_decodedData) buff_decodedData.Last()[9] = data;
            }
            else //Блок данных - Marker
            {
                bin = bin.Remove(38); //Удаляем бит-маркер
                data = BinaryHandler.HammingDecode(bin); //Декодируем блок данных по Хэммингу
                lock (buff_decodedData) buff_decodedData.Last()[9] = data;
            }
        }

        public static void Start()
        {
            thread_samplesDecoder = new Thread(SamplesDecoderStereo);
            thread_samplesDecoder.Start();
            thread_amplitudeDecoderL = new Thread(AmplitudeDecoder);
            thread_amplitudeDecoderL.Start();
            //thread_amplitudeDecoderR = new Thread(AmplitudeDecoderR);
            //thread_amplitudeDecoderR.Start();
            thread_binaryDecoder = new Thread(BinaryDecoderStereo);
            thread_binaryDecoder.Start();
            AudioIO.thread_signalAutoGainControl = new Thread(AudioIO.SignalAutoGainControll);
            AudioIO.thread_signalAutoGainControl.Start();
        }
    }
}
