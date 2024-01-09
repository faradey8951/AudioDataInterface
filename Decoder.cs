using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
        public static short maxAmplitudeL = 0;
        public static short maxAmplitudeR = 0;
        public static int signalContrast = 0;
        public static int errorCount = 0;
        public static Object bytesLocker = new Object();
        public static Object samplesLLocker = new object();
        public static Object samplesRLocker = new object();
        public static Object amplitudesLLocker = new Object();
        public static Object amplitudesRLocker = new Object();
        public static Object decodedDataLocker = new Object();

        static void SamplesDecoder()
        {
            while (true)
            {
                while (AudioIO.buff_signalBytes.Count < 96000) Thread.Sleep(10);
                if (!AudioIO.audio_invertSignal) lock (AudioIO.buff_signalSamples) AudioIO.buff_signalSamples.Add((short)(AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                else lock (AudioIO.buff_signalSamples) AudioIO.buff_signalSamples.Add((short)(-AudioIO.audio_signalGain * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                AudioIO.buff_signalBytes.RemoveRange(0, 2);
            }
        }

        static void SamplesDecoderStereo()
        {
            while (true)
            {
                while (AudioIO.buff_signalBytes.Count < 12000) Thread.Sleep(10);
                if (!AudioIO.audio_invertSignal)
                {
                    lock (samplesLLocker) AudioIO.buff_signalSamplesL.Add((short)(AudioIO.audio_signalGainL * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                    lock (samplesRLocker) AudioIO.buff_signalSamplesR.Add((short)(AudioIO.audio_signalGainR * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[2], AudioIO.buff_signalBytes[3] }, 0) + (short)AudioIO.audio_signalHeight));
                }
                else
                {
                    lock (samplesLLocker) AudioIO.buff_signalSamplesL.Add((short)(-AudioIO.audio_signalGainL * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[0], AudioIO.buff_signalBytes[1] }, 0) + (short)AudioIO.audio_signalHeight));
                    lock (samplesRLocker) AudioIO.buff_signalSamplesR.Add((short)(-AudioIO.audio_signalGainR * BitConverter.ToInt16(new byte[2] { AudioIO.buff_signalBytes[2], AudioIO.buff_signalBytes[3] }, 0) + (short)AudioIO.audio_signalHeight));
                }
                lock (bytesLocker) AudioIO.buff_signalBytes.RemoveRange(0, 4);
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
                    lock (samplesLLocker) AudioIO.buff_signalSamplesL.RemoveAt(0);
                    while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                }
                while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamplesL[0] >= 0)
                {
                    lock (samplesLLocker)
                    {
                        tempList.Add(AudioIO.buff_signalSamplesL[0]);
                        AudioIO.buff_signalSamplesL.RemoveAt(0);
                    }
                    while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                }
                lock (amplitudesLLocker) buff_signalAmplitudesL.Add(tempList.Max());
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
                    lock (samplesRLocker) AudioIO.buff_signalSamplesR.RemoveAt(0);
                    while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                }
                while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamplesR[0] >= 0)
                {
                    lock (samplesRLocker)
                    {
                        tempList.Add(AudioIO.buff_signalSamplesR[0]);
                        AudioIO.buff_signalSamplesR.RemoveAt(0);
                    }
                    while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                }
                lock (amplitudesRLocker) buff_signalAmplitudesR.Add(tempList.Max());
                tempList.Clear();
            }
        }

        static void AmplitudeDecoder()
        {
            var tempList = new List<short>(); //Временный буфер сэмплов
            while (true)
            {

                while (AudioIO.buff_signalSamples.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamples[0] < 0)
                {
                    lock (AudioIO.buff_signalSamples) AudioIO.buff_signalSamples.RemoveAt(0);
                    while (AudioIO.buff_signalSamples.Count == 0) Thread.Sleep(10);
                }
                while (AudioIO.buff_signalSamples.Count == 0) Thread.Sleep(10);
                while (AudioIO.buff_signalSamples[0] >= 0)
                {
                    lock (AudioIO.buff_signalSamples)
                    {
                        tempList.Add(AudioIO.buff_signalSamples[0]);
                        AudioIO.buff_signalSamples.RemoveAt(0);
                    }
                    while (AudioIO.buff_signalSamples.Count == 0) Thread.Sleep(10);
                }
                lock (buff_signalAmplitudes) buff_signalAmplitudes.Add(tempList.Max());
                tempList.Clear();
            }
        }

        static void BinaryDecoderStereo()
        {
            //Установить приоритет процесса на максимум
            //System.Diagnostics.Process thisProc = System.Diagnostics.Process.GetCurrentProcess();
            //thisProc.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

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
                if (channelSwitch == 0) while (buff_signalAmplitudesL.Count < 80) Thread.Sleep(10);
                else while (buff_signalAmplitudesR.Count < 80) Thread.Sleep(10);

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

                if (channelSwitch == 0)
                {
                    if (buff_signalAmplitudesL.Count >= 80)
                    {
                        lock (amplitudesLLocker) { for (int i = 0; i < 80; i++) amplitudeBuff.Add(buff_signalAmplitudesL[i]); }
                        amplitudeBuffCopy.AddRange(amplitudeBuff);
                        List<int> suncPulsePower = new List<int>();
                        for (int i = 0; i <= 39; i++) suncPulsePower.Add((int)(amplitudeBuffCopy[i] + amplitudeBuffCopy[i + 39 + 1]));
                        int syncPulsePowerMax = suncPulsePower.IndexOf(suncPulsePower.Max());
                        maxAmplitudeL = (short)(suncPulsePower.Max() / 2);
                        maxSyncPulseIndex = syncPulsePowerMax;
                        syncPulseIndex = syncPulsePowerMax + 39 + 1;
                    }
                    else
                    {
                        Thread.Sleep(10);
                        sync = false;
                    }
                }
                else
                {
                    if (buff_signalAmplitudesR.Count >= 80)
                    {
                        lock (amplitudesRLocker) { for (int i = 0; i < 80; i++) amplitudeBuff.Add(buff_signalAmplitudesR[i]); }
                        amplitudeBuffCopy.AddRange(amplitudeBuff);
                        List<int> suncPulsePower = new List<int>();
                        for (int i = 0; i <= 39; i++) suncPulsePower.Add((int)(amplitudeBuffCopy[i] + amplitudeBuffCopy[i + 39 + 1]));
                        int syncPulsePowerMax = suncPulsePower.IndexOf(suncPulsePower.Max());
                        maxAmplitudeR = (short)(suncPulsePower.Max() / 2);
                        maxSyncPulseIndex = syncPulsePowerMax;
                        syncPulseIndex = syncPulsePowerMax + 39 + 1;
                    }
                    else
                    {
                        Thread.Sleep(10);
                        sync = false;
                    }
                }


                if (sync == true) //Если синхронизация обнаружена
                {
                    lock (decodedDataLocker)
                    {
                        tempSyncIndexes.Add(syncPulseIndex);
                        tempSyncIndexes.Add(maxSyncPulseIndex);
                        tempSyncIndexes.Sort(); //Сортировка индексов синхроимпульсов по возрастанию
                        for (int i = tempSyncIndexes[0] + 1; i < tempSyncIndexes[1]; i++) //Составляем буфер данных между синхроимпульсами
                        {
                            dataBlockBuff.Add(amplitudeBuff[i]);
                            dataBlockBuffLnr.Add(amplitudeBuff[i]);
                        }
                        buff_decodedData.Add(new string[10]);
                        buff_decodedData.Last()[0] = String.Join(":", amplitudeBuff);
                        buff_decodedData.Last()[1] = String.Join(":", dataBlockBuff);
                        if (dataBlockBuff.Count == 39) //Если блок данных обнаружен
                        {
                            int jumpIndex = 0;
                            List<int> dataBlockBuffCopyIndexes = new List<int>(); //Буфер индексов амлитуд
                            List<short> dataBlockBuffCopySorted = new List<short>(); //Отсортированный по убыванию буфер амплитуд
                            List<int> dataBlockBuffCopySortedDynamics = new List<int>(); //Буфер относительной динамики амплитуд (принимает значения [0-100]%), показывает степень изменения амплитуды в отсортированном по убыванию буфере
                            dataBlockBuffCopy.AddRange(dataBlockBuff);
                            dataBlockBuffCopySorted.AddRange(dataBlockBuff);
                            dataBlockBuffCopySorted.Sort();
                            dataBlockBuffCopySorted.Reverse();
                            //Связываем амплитуды в отсортированном буфере с индексами в исходном dataBlockBuff
                            for (int i = 0; i < dataBlockBuffCopySorted.Count; i++)
                            {
                                dataBlockBuffCopyIndexes.Add(dataBlockBuffCopy.IndexOf(dataBlockBuffCopySorted[i]));
                                dataBlockBuffCopy[dataBlockBuffCopyIndexes[i]] = 0;
                            }
                            dataBlockBuffCopySorted.Clear();
                            dataBlockBuffCopySorted.AddRange(dataBlockBuff);
                            dataBlockBuffCopySorted.Sort();
                            dataBlockBuffCopySorted.Reverse();
                            //Расчитываем степень изменения амплитуды в отсортированном буфере
                            for (int i = 0; i < dataBlockBuffCopySorted.Count - 2; i++) dataBlockBuffCopySortedDynamics.Add((int)Math.Round(100 - (((double)dataBlockBuffCopySorted[i + 1] * 100) / (double)dataBlockBuffCopySorted[i])));
                            jumpIndex = dataBlockBuffCopySortedDynamics.IndexOf(dataBlockBuffCopySortedDynamics.Max());
                            signalContrast = dataBlockBuffCopySortedDynamics.Max();
                            for (int i = 0; i <= jumpIndex; i++)
                            {
                                tempOneIndexes.Add(dataBlockBuffCopyIndexes[i]);
                            }
                            tempOneIndexes.Sort();

                            buff_decodedData.Last()[5] = String.Join(":", dataBlockBuffLnr);
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

                            //Контроль канальной синхронизации
                            bool channelSyncSucc = false;
                            if (channelSwitch == 1 && buff_decodedData.Last()[8][38].ToString() == "1") //Детектируем наличие субкода
                            {
                                string subCode = buff_decodedData.Last()[9]; //Получаем двоичный субкод
                                                                             //Выделяем составляющие байты субкода
                                byte subCodeByte1 = Convert.ToByte(Convert.ToInt16(subCode.Substring(0, 8), 2));
                                byte subCodeByte2 = Convert.ToByte(Convert.ToInt16(subCode.Substring(8, 8), 2));
                                byte subCodeByte3 = Convert.ToByte(Convert.ToInt16(subCode.Substring(16, 8), 2));
                                byte subCodeByte4 = Convert.ToByte(Convert.ToInt16(subCode.Substring(24, 8), 2));
                                if (subCodeByte1 == 255)
                                {
                                    if (subCodeByte2 == 255 || subCodeByte3 == 255 || subCodeByte4 == 255) //Определяем субкод контроля канальной синхронизации правого канала
                                    {
                                        if (buff_decodedData[buff_decodedData.Count - 2][8][38].ToString() == "1") //Проверяем предыдущий блок на наличие субкода
                                        {
                                            subCode = buff_decodedData[buff_decodedData.Count - 2][9]; //Получаем двоичный субкод
                                                                                                       //Выделяем составляющие байты субкода
                                            subCodeByte1 = Convert.ToByte(Convert.ToInt16(subCode.Substring(0, 8), 2));
                                            subCodeByte2 = Convert.ToByte(Convert.ToInt16(subCode.Substring(8, 8), 2));
                                            subCodeByte3 = Convert.ToByte(Convert.ToInt16(subCode.Substring(16, 8), 2));
                                            subCodeByte4 = Convert.ToByte(Convert.ToInt16(subCode.Substring(24, 8), 2));
                                            //Определяем субкод контроля канальной синхронизации левого канала
                                            if (subCodeByte1 == 255 && (subCodeByte2 == 0 || subCodeByte3 == 0 || subCodeByte4 == 0)) channelSyncSucc = true;
                                        }
                                    }
                                }
                                else
                                {

                                    channelSyncSucc = true;
                                }
                            }
                            else channelSyncSucc = true;
                            if (channelSyncSucc == false)
                            {
                                form_main.mpsPlayer_disc1Detected = false;
                                lock (amplitudesLLocker) Decoder.buff_signalAmplitudesL.Clear();
                                lock (amplitudesRLocker) Decoder.buff_signalAmplitudesR.Clear();
                                form_main.class_dataHandler.ms = new System.IO.MemoryStream();
                            }


                        }
                        else
                            Thread.Sleep(10);
                        try
                        {
                        if (buff_signalAmplitudesL != null && buff_signalAmplitudesR != null && buff_signalAmplitudesL.Count > 0 && buff_signalAmplitudesR.Count > 0)
                        {
                            if (channelSwitch == 0)
                            {
                                lock (amplitudesLLocker) buff_signalAmplitudesL.RemoveRange(0, tempSyncIndexes[1] - 1); //Удалить отработанные амплитуды
                            }
                            else
                            {
                                lock (amplitudesRLocker) buff_signalAmplitudesR.RemoveRange(0, tempSyncIndexes[1] - 1); //Удалить отработанные амплитуды
                            }
                        }
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    if (channelSwitch == 0)
                    {
                        if (buff_signalAmplitudesL.Count > 0)
                        {
                            lock (amplitudesLLocker) buff_signalAmplitudesL.RemoveRange(0, buff_signalAmplitudesL.Count - 1);
                        }
                    }
                    else if (buff_signalAmplitudesR.Count > 0)
                    {
                        lock (amplitudesRLocker) buff_signalAmplitudesR.RemoveRange(0, buff_signalAmplitudesR.Count - 1);
                    }

                    //Thread.Sleep(10);
                }
                if (channelSwitch == 0) channelSwitch = 1;
                else channelSwitch = 0;
            }
        }

        public static int[] GetTimeFromSeconds(int sec)
        {
            if (sec < 0) sec = 0;
            double time = Math.Round((double)((double)sec / 60), 3);
            int minutes = (int)Math.Truncate(time);
            //MessageBox.Show((time - Math.Truncate(time)).ToString());
            int seconds = (int)Math.Round(((time - Math.Truncate(time)) * 6000) / 100);
            int a, b, c, d;
            if (minutes < 10)
            {
                a = 0;
                b = minutes;
            }
            else
            {
                a = Convert.ToInt16(minutes.ToString()[0].ToString());
                b = Convert.ToInt16(minutes.ToString()[1].ToString());
            }
            if (seconds < 10)
            {
                c = 0;
                d = seconds;
            }
            else
            {
                c = Convert.ToInt16(seconds.ToString()[0].ToString());
                d = Convert.ToInt16((string)seconds.ToString()[1].ToString());
            }
            if (a < 10 && b < 10 && c < 10 && d < 10) return new[] { a, b, c, d };
            else return new[] { 0, 0, 0, 0 };
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
            buff_decodedData.Last()[8] = bin;
            string[] data = null; //Данные
            bin = bin.Remove(38); //Удаляем бит-маркер
            data = BinaryHandler.HammingDecode(bin); //Декодируем блок данных по Хэммингу
            if (data[1] == "error") errorCount++;
            buff_decodedData.Last()[9] = data[0];
        }

        public static void Start()
        {
            thread_samplesDecoder = new Thread(SamplesDecoderStereo);
            thread_samplesDecoder.Start();
            thread_amplitudeDecoderL = new Thread(AmplitudeDecoderL);
            thread_amplitudeDecoderL.Start();
            thread_amplitudeDecoderR = new Thread(AmplitudeDecoderR);
            thread_amplitudeDecoderR.Start();
            thread_binaryDecoder = new Thread(BinaryDecoderStereo);
            thread_binaryDecoder.Start();
            AudioIO.thread_signalAutoGainControl = new Thread(AudioIO.SignalAutoGainControll);
            AudioIO.thread_signalAutoGainControl.Start();
        }
    }
}
