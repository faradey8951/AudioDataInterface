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
        public static string[] decodedDataBlock = new string[6]; //Декодированный блок данных
        public static string[] lastDecodedDataBlock = new string[6]; //Последний декодированный блок данных
        public static readonly List<string[]> buff_decodedData = new List<string[]>(); //Буфер декодированных данных
        public static Thread thread_binaryDecoderStereo = null;
        public static Thread thread_amplitudeDecoderL = null;
        public static Thread thread_amplitudeDecoderR = null;
        public static Thread thread_samplesDecoderStereo = null;
        public static short maxAmplitudeL = 0;
        public static short maxAmplitudeR = 0;
        public static int overallBlockCount = 0;
        public static int overallErrorCount = 0;
        public static int signalQuality = 0;
        public static int fixedErrorCount = 0;
        public static int unfixedErrorCount = 0;
        public static int frameSyncErrorCount = 0;
        public static Object bytesLocker = new Object();
        public static Object samplesLLocker = new object();
        public static Object samplesRLocker = new object();
        public static Object amplitudesLLocker = new Object();
        public static Object amplitudesRLocker = new Object();
        public static Object decodedDataLocker = new Object();
        public static bool decoderActive = false;
        public static bool sectorGet = false;
        public static string decoderMode = "";
        public static List<byte> sector = new List<byte>();
        public static string sectorType = "";
        public static bool sectorGot = false;
        public static int decodedBlocksCounter = 0;
        public static int decodedBlocksCountFirst = 0;
        public static int decodedBlocksCountSecond = 0;

        static void SamplesDecoderStereo()
        {
            while (Decoder.decoderActive)
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
            while (Decoder.decoderActive)
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
                        if (tempList.Count >= 512)
                        {
                            tempList.Clear();
                            break;
                        }
                    }
                    while (AudioIO.buff_signalSamplesL.Count == 0) Thread.Sleep(10);
                }
                if (tempList.Count > 0 && tempList.Count > 1) lock (amplitudesLLocker) buff_signalAmplitudesL.Add(tempList.Max());
                tempList.Clear();
            }
        }

        static void AmplitudeDecoderR()
        {
            var tempList = new List<short>(); //Временный буфер сэмплов
            while (Decoder.decoderActive)
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
                        if (tempList.Count >= 512)
                        {
                            tempList.Clear();
                            break;
                        }
                    }
                    while (AudioIO.buff_signalSamplesR.Count == 0) Thread.Sleep(10);
                }
                if (tempList.Count > 0 && tempList.Count > 1) lock (amplitudesRLocker) buff_signalAmplitudesR.Add(tempList.Max());
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
            int syncPulsePowerSpanL = 0;
            int syncPulsePowerSpanR = 0;
            int channelSwitch = 0;

            while (Decoder.decoderActive)
            {
                if (channelSwitch == 0) while (buff_signalAmplitudesL.Count < 80) Thread.Sleep(10);
                else while (buff_signalAmplitudesR.Count < 80) Thread.Sleep(10);

                decodedDataBlock = new string[7];
                lastDecodedDataBlock = new string[7];
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
                        maxAmplitudeL = amplitudeBuff.Max();
                        double bufferGain = maxAmplitudeL / 20000.0;
                        for (int i = 0; i < amplitudeBuff.Count; i++) amplitudeBuff[i] = (short)(amplitudeBuff[i] / bufferGain); //Нормализация сигнала
                        List<int> suncPulsePower = new List<int>();
                        for (int i = 0; i <= 39; i++) suncPulsePower.Add((int)(amplitudeBuffCopy[i] + amplitudeBuffCopy[i + 39 + 1]));
                        int syncPulsePowerMax = suncPulsePower.IndexOf(suncPulsePower.Max());
                        syncPulsePowerSpanL = suncPulsePower.Max() - suncPulsePower.Min();
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
                        maxAmplitudeR = amplitudeBuff.Max();
                        double bufferGain = maxAmplitudeR / 20000.0;
                        for (int i = 0; i < amplitudeBuff.Count; i++) amplitudeBuff[i] = (short)(amplitudeBuff[i] / bufferGain); //Нормализация сигнала
                        List<int> suncPulsePower = new List<int>();
                        for (int i = 0; i <= 39; i++) suncPulsePower.Add((int)(amplitudeBuffCopy[i] + amplitudeBuffCopy[i + 39 + 1]));
                        int syncPulsePowerMax = suncPulsePower.IndexOf(suncPulsePower.Max());
                        syncPulsePowerSpanR = suncPulsePower.Max() - suncPulsePower.Min();
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
                    lock (decodedDataLocker) if (buff_decodedData.Count > 0) lastDecodedDataBlock = buff_decodedData.Last();
                    tempSyncIndexes.Add(syncPulseIndex);
                    tempSyncIndexes.Add(maxSyncPulseIndex);
                    tempSyncIndexes.Sort(); //Сортировка индексов синхроимпульсов по возрастанию
                    syncPulse = Convert.ToInt16(amplitudeBuff[maxSyncPulseIndex]);
                    maxSyncPulse = Convert.ToInt16(amplitudeBuff[syncPulseIndex]);
                    for (int i = tempSyncIndexes[0] + 1; i < tempSyncIndexes[1]; i++) //Составляем буфер данных между синхроимпульсами
                    {
                        dataBlockBuff.Add(amplitudeBuff[i]);
                        dataBlockBuffLnr.Add(amplitudeBuff[i]);
                    }
                    decodedDataBlock[0] = String.Join(":", amplitudeBuff);
                    decodedDataBlock[1] = String.Join(":", dataBlockBuff);
                    if (dataBlockBuff.Count == 39) //Если блок данных обнаружен
                    {
                        //dataBlockBuff[0] = (short)(dataBlockBuff[0] * 0.97);
                        //Дифференциальный метод декодирования
                        double[] derivative = new double[dataBlockBuff.Count - 1]; //Первая производная сигнала
                        List<double> frwLnrDerivative = new List<double>(); //Прямая линеаризованная производная сигнала
                        List<double> rvsLnrDerivative = new List<double>(); //Обратная линеаризованная производная сигнала
                        List<double> sortedLnrDerivative = new List<double>(); //Упорядоченная производная сигнала
                        List<double> sortedLnrSecondDerivative = new List<double>();
                        double sortedLnrSecondDerivativeMaxValue = 0;
                        double sortedLnrDerivativeJumpValue = 0;
                        List<string> derivativeDirection = new List<string>(); //Знак первой производной
                        List<string> derivativeBinaryValue = new List<string>(); //Бинарное значение производной
                        List<string> derivativeDecryptor = new List<string>();
                        //dataBlockBuff = new List<short> { 6001, 3157, 3109, 6960, 3268, 3247, 7220, 7516, 3231, 3279, 7003, 6785, 3252, 3586, 3422, 3565, 3618, 3390, 3448, 3167, 6828, 2998, 3003, 3226, 2855, 6642, 3045, 3008, 6860, 2924, 6785, 3130, 3247, 3220, 3453, 3395, 3475, 3024, 6584 };
                        for (int i = 0; i < derivative.Length; i++)
                        {
                            double derivativeValue = dataBlockBuff[i + 1] - dataBlockBuff[i];
                            derivative[i] = Math.Abs(derivativeValue);
                            if (derivativeValue < 0) derivativeDirection.Add("-");
                            else derivativeDirection.Add("+");
                        }
                        //Линеаризация данных с помощью линейного полинома (A = k*i + b) методом наименьших квадратов
                        double[] I = new double[derivative.Length];
                        double[] iSquare = new double[derivative.Length];
                        double[] Ai = new double[derivative.Length];
                        double[] trend = new double[derivative.Length];
                        double[] median = new double[derivative.Length];
                        double[] mid = new double[derivative.Length];
                        double iSumm = 0;
                        double iSquareSumm = 0;
                        double AiSumm = 0;
                        double ASumm = 0;
                        double n = derivative.Length;
                        for (int p = 0; p < derivative.Length; p++)
                        {
                            I[p] = p + 1;
                            iSquare[p] = (p + 1) * (p + 1);
                            Ai[p] = derivative[p] * (p + 1);
                        }
                        for (int p = 0; p < derivative.Length; p++)
                        {
                            iSumm += I[p];
                            iSquareSumm += iSquare[p];
                            AiSumm += Ai[p];
                            ASumm += derivative[p];
                        }
                        double k1 = ((n * AiSumm) - (ASumm * iSumm)) / ((n * ASumm) - (iSumm * iSumm));
                        double b1 = ((ASumm * ASumm) - (iSumm * AiSumm)) / ((n * ASumm) - (iSumm * iSumm));
                        double k2 = ((n * AiSumm) - (ASumm * iSumm)) / ((n * iSquareSumm) - (iSumm * iSumm));
                        double b2 = ((ASumm * iSquareSumm) - (iSumm * AiSumm)) / ((n * iSquareSumm) - (iSumm * iSumm));
                        for (int p = 0; p < median.Length; p++) median[p] = Math.Round((k1 * (p + 1)) + b1);
                        for (int p = 0; p < trend.Length; p++) trend[p] = Math.Round((k2 * (p + 1)) + b2);
                        for (int p = 0; p < mid.Length; p++) mid[p] = Math.Round((median[p] + trend[p]) / 2.0);

                        for (int p = 1; p <= derivative.Length; p++)
                        {
                            double linearizedAmplitude = (double)derivative[p - 1] - (mid[p - 1] - mid[0]);
                            frwLnrDerivative.Add(Convert.ToInt32(linearizedAmplitude));
                        }
                        for (int p = 1; p <= derivative.Length; p++)
                        {
                            double linearizedAmplitude = (double)frwLnrDerivative[p - 1] - (mid[mid.Length - 1] - mid[p - 1]);
                            rvsLnrDerivative.Add(Convert.ToInt32(linearizedAmplitude));
                        }
                        sortedLnrDerivative.AddRange(rvsLnrDerivative);
                        sortedLnrDerivative.Sort();
                        sortedLnrDerivative.Reverse();
                        for (int i = 0; i < derivative.Length - 1; i++) sortedLnrSecondDerivative.Add(sortedLnrDerivative[i] - sortedLnrDerivative[i + 1]);
                        sortedLnrSecondDerivativeMaxValue = sortedLnrSecondDerivative.Max();
                        sortedLnrDerivativeJumpValue = sortedLnrDerivative[sortedLnrSecondDerivative.IndexOf(sortedLnrSecondDerivativeMaxValue)];
                        for (int i = 0; i < rvsLnrDerivative.Count; i++)
                        {
                            if (rvsLnrDerivative[i] >= sortedLnrDerivativeJumpValue) derivativeBinaryValue.Add("1");
                            else derivativeBinaryValue.Add("0");
                        }
                        int derivativeChangeCount = 0;
                        for (int i = 0; i < derivative.Length; i++)
                        {
                            if (derivativeBinaryValue[i] == "0") derivativeDecryptor.Add("-");
                            if (derivativeBinaryValue[i] == "1" && derivativeDirection[i] == "+")
                            {
                                derivativeDecryptor.Add("01");
                                derivativeChangeCount++;
                            }
                            if (derivativeBinaryValue[i] == "1" && derivativeDirection[i] == "-")
                            {
                                derivativeDecryptor.Add("10");
                                derivativeChangeCount++;
                            }
                        }
                        if (derivativeChangeCount < 2) { tempBin = "000000000000000000000000000000000000001"; }
                        else
                        {
                            derivativeDecryptor.Add("-");
                            while (derivativeDecryptor.IndexOf("-") != -1)
                            {
                                for (int i = 1; i < derivative.Length; i++)
                                {
                                    if (derivativeDecryptor[i] != "-")
                                    {
                                        if (derivativeDecryptor[i - 1] == "-") derivativeDecryptor[i - 1] = derivativeDecryptor[i][0].ToString() + derivativeDecryptor[i][0].ToString();
                                        if (derivativeDecryptor[i + 1] == "-") derivativeDecryptor[i + 1] = derivativeDecryptor[i][1].ToString() + derivativeDecryptor[i][1].ToString();
                                    }
                                }
                            }
                            for (int i = 0; i < derivativeDecryptor.Count; i++) tempBin += derivativeDecryptor[i][0].ToString();
                        }
                        decodedDataBlock[2] = String.Join(":", dataBlockBuff);
                        BinaryDecode(tempBin);
                        decodedBlocksCounter++;
                        if (decodedDataBlock[3] != null && decodedDataBlock[4] != null)
                        {
                            //Контроль канальной синхронизации
                            bool channelSyncSucc = true;
                            if (decodedDataBlock[3][38] == '0') //Детектируем наличие субкода
                            {
                                string subCode = decodedDataBlock[4]; //Получаем двоичный субкод
                                byte subCodeByte1 = Convert.ToByte(Convert.ToInt16(subCode.Substring(0, 8), 2));
                                byte subCodeByte2 = Convert.ToByte(Convert.ToInt16(subCode.Substring(8, 8), 2));
                                byte subCodeByte3 = Convert.ToByte(Convert.ToInt16(subCode.Substring(16, 8), 2));
                                byte subCodeByte4 = Convert.ToByte(Convert.ToInt16(subCode.Substring(24, 8), 2));
                                if (subCodeByte1 == 123) //Субкод канальной синхронизации
                                {
                                    if (subCodeByte2 == 1 && subCodeByte3 == 1 && subCodeByte4 == 1) //Определяем субкод контроля канальной синхронизации правого канала
                                    {
                                        channelSyncSucc = false;
                                        if (lastDecodedDataBlock[3][38] == '0') //Проверяем предыдущий блок на наличие субкода
                                        {
                                            subCode = lastDecodedDataBlock[4]; //Получаем двоичный субкод
                                            subCodeByte1 = Convert.ToByte(Convert.ToInt16(subCode.Substring(0, 8), 2));
                                            subCodeByte2 = Convert.ToByte(Convert.ToInt16(subCode.Substring(8, 8), 2));
                                            subCodeByte3 = Convert.ToByte(Convert.ToInt16(subCode.Substring(16, 8), 2));
                                            subCodeByte4 = Convert.ToByte(Convert.ToInt16(subCode.Substring(24, 8), 2));
                                            //Определяем субкод контроля канальной синхронизации левого канала
                                            if (subCodeByte1 == 123 && (subCodeByte2 == 0 && subCodeByte3 == 0 && subCodeByte4 == 0)) channelSyncSucc = true;
                                        }
                                    }
                                    else channelSyncSucc = true;
                                }
                                if (decoderMode == "sector")
                                {
                                    if (subCodeByte1 == 24 && subCodeByte2 == 0 && subCodeByte3 == 0 && subCodeByte4 == 0) { sectorGet = true; sector.Clear(); sectorType = "header"; }
                                    if (subCodeByte1 == 24 && subCodeByte2 == 1 && subCodeByte3 == 1 && subCodeByte4 == 1 && sector.Count != 0) { sectorGet = false; sectorType = "header"; }
                                    if (subCodeByte1 == 25 && subCodeByte2 == 0 && subCodeByte3 == 0 && subCodeByte4 == 0) { sectorGet = true; sector.Clear(); sectorType = "hashes"; }
                                    if (subCodeByte1 == 25 && subCodeByte2 == 1 && subCodeByte3 == 1 && subCodeByte4 == 1 && sector.Count != 0) { sectorGet = false; sectorType = "hashes"; }
                                    if (subCodeByte1 == 26 && subCodeByte2 == 0 && subCodeByte3 == 0) { sectorGet = true; sector.Clear(); sectorType = ((int)subCodeByte4).ToString(); }
                                    if (subCodeByte1 == 26 && subCodeByte2 == 1 && subCodeByte3 == 1) { sectorGet = false; sectorType = ((int)subCodeByte4).ToString(); }
                                }
                            }
                            else
                            {
                                if (sectorGet == true) { sector.Add(Convert.ToByte(Convert.ToInt16(decodedDataBlock[4].Substring(0, 8), 2))); sector.Add(Convert.ToByte(Convert.ToInt16(decodedDataBlock[4].Substring(8, 8), 2))); sector.Add(Convert.ToByte(Convert.ToInt16(decodedDataBlock[4].Substring(16, 8), 2))); sector.Add(Convert.ToByte(Convert.ToInt16(decodedDataBlock[4].Substring(24, 8), 2))); }
                            }
                            if (syncPulsePowerSpanL < 8000 && syncPulsePowerSpanR < 8000) channelSyncSucc = false;
                            if (channelSyncSucc == false)
                            {
                                DataHandler.subcodeSyncError = true;
                                LogHandler.WriteStatus("Decoder/BinaryDecoderStereo", "Channel frame sync error");
                                frameSyncErrorCount++;
                                decodedDataBlock = new string[7];
                                lastDecodedDataBlock = new string[7];
                                form_main.mpsPlayer_disc1Detected = false;
                                lock (amplitudesLLocker) Decoder.buff_signalAmplitudesL.Clear();
                                lock (amplitudesRLocker) Decoder.buff_signalAmplitudesR.Clear();
                                if (Decoder.decoderMode == "sector" && Decoder.sectorGet == true) { form_tapeRecordingWizard.errorOccurred = true; form_tapeRecordingWizard.errorReason = "channel sync error"; }
                            }
                            if (channelSyncSucc == true) lock (decodedDataLocker) buff_decodedData.Add(decodedDataBlock);
                        }
                    }
                    {
                        if (buff_signalAmplitudesL != null && buff_signalAmplitudesR != null && buff_signalAmplitudesL.Count > 0 && buff_signalAmplitudesR.Count > 0)
                        {
                            if (channelSwitch == 0) lock (amplitudesLLocker) buff_signalAmplitudesL.RemoveRange(0, tempSyncIndexes[1] - 1); //Удалить отработанные амплитуды
                            else lock (amplitudesRLocker) buff_signalAmplitudesR.RemoveRange(0, tempSyncIndexes[1] - 1); //Удалить отработанные амплитуды
                        }
                    }
                }
                else
                {
                    if (channelSwitch == 0) if (buff_signalAmplitudesL.Count > 0) lock (amplitudesLLocker) buff_signalAmplitudesL.RemoveRange(0, buff_signalAmplitudesL.Count - 1);
                    else if (buff_signalAmplitudesR.Count > 0) lock (amplitudesRLocker) buff_signalAmplitudesR.RemoveRange(0, buff_signalAmplitudesR.Count - 1);
                    Thread.Sleep(10);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        public static void BinaryDecode(string bin)
        {
            decodedDataBlock[3] = bin;
            string[] data = null; //Данные
            if (bin != null)
            {
                overallBlockCount++;
                bin = bin.Remove(bin.Length - 1);
                data = BinaryHandler.HammingDecode(bin); //Декодируем блок данных по Хэммингу
                if (data[1] == "fixed") { fixedErrorCount++; overallErrorCount++; }
                if (data[1] == "error") { unfixedErrorCount++; overallErrorCount++; data[0] = ""; if (Decoder.decoderMode == "sector" && Decoder.sectorGet == true) form_tapeRecordingWizard.errorOccurred = true; form_tapeRecordingWizard.errorReason = "uncorrectable error"; if (form_main.mpsPlayer_disc1Detected) LogHandler.WriteStatus("Decoder/BinaryDecode", "Uncorrectable error at " + bin); }
                if (data[0] != "") decodedDataBlock[4] = data[0];
                signalQuality = (int)(100 - (100.0 * overallErrorCount / overallBlockCount));
                decodedDataBlock[6] = signalQuality.ToString();
                if (overallBlockCount == 125) {overallBlockCount = 0; overallErrorCount = 0; }               
            }
        }

        /// <summary>
        /// Очищает буферы, связанные с декодером
        /// </summary>
        public static void ClearBuffers()
        {
            AudioIO.buff_signalBytes.Clear();
            Decoder.buff_signalAmplitudesL.Clear();
            Decoder.buff_signalAmplitudesR.Clear();
            if (DataHandler.ms != null) DataHandler.ms.Close();
        }

        public static void Start()
        {
            thread_samplesDecoderStereo = new Thread(SamplesDecoderStereo);
            thread_samplesDecoderStereo.Start();
            thread_amplitudeDecoderL = new Thread(AmplitudeDecoderL);
            thread_amplitudeDecoderL.Start();
            thread_amplitudeDecoderR = new Thread(AmplitudeDecoderR);
            thread_amplitudeDecoderR.Start();
            thread_binaryDecoderStereo = new Thread(BinaryDecoderStereo);
            thread_binaryDecoderStereo.Start();     
        }
        public static void Stop()
        {
            if (thread_amplitudeDecoderL != null) thread_amplitudeDecoderL.Abort();
            if (thread_amplitudeDecoderR != null) thread_amplitudeDecoderR.Abort();
            if (thread_samplesDecoderStereo != null) thread_samplesDecoderStereo.Abort();
            if (thread_binaryDecoderStereo != null) thread_binaryDecoderStereo.Abort();
        }
    }
}
