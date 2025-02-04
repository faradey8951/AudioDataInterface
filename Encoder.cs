using FftSharp;
using ImageMagick;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AudioDataInterface
{
    /// <summary>
    /// Конвертер цифровых данных в аудио
    /// </summary>
    public class Encoder
    {
        //Другое
        //////////////////////////////////////////////////////////////////////////////////////
        public static FileStream fs_output = null;
        public static FileStream fs_input = null;
        public static List<short> list_outputFileSamples = new List<short>(); //Коллекция сэмплов для записи выходного файла
        public static List<short> list_outputFileLSamples = new List<short>(); //Коллекция сэмплов для записи выходного файла
        public static List<short> list_outputFileRSamples = new List<short>(); //Коллекция сэмплов для записи выходного файла
        public static char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '~', '_', '+', '/', '|', '<', '>', '.', ',', '`', ';', ':', '?', '[', ']', '{', '}', ' ' };
        //////////////////////////////////////////////////////////////////////////////////////

        //Энкодер
        //Префикс: "encoder_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static string encoder_outputFilePath = "output.wav"; //Путь к выходному файлу энкодера
        public static string encoder_inputFilePath = ""; //Путь ко входному файлу энкодера
        public static int encoder_signalGain = 2; //Коэффициент усиления аудиосигнала выходного файла
        public static int encoder_sampleRate = 96000; //Частота дискретизации выходного файла
        public static int encoder_blockRate = 270; //Количество блоков в секунду на канал (250 бл/сек для 1 пакета OPUS + 10 бл/сек для субкодов + 10 бл/сек для запаса буферизации)
        public static int encoder_samplesPerBit = 4;
        public static bool encoder_ADIFShell = false; //Функция ADIFShell
        public static bool encoder_forceStop = false; //Принудительная остановка конвертации
        public static int encoder_progress = 0; //Прогресс конвертации [%]
        public static double minSampleDeltaCoefficient = 0.4;
        public static double maxSampleDeltaCoefficient = 0.9;
        public static int encoder_silenceSeconds = 0;
        public static int encoder_leadInSubcodesAmount = 0;
        public static int encoder_mpsPlayerSubCodeInterval = 0;
        public static bool encoder_longLeadIn = false;
        public static string encoder_mode = "";
        public static int encoder_sectorFileIndex = 0;
        //////////////////////////////////////////////////////////////////////////////////////

        //Потоки
        //Префикс: thread_
        //////////////////////////////////////////////////////////////////////////////////////
        public static Thread thread_encodeFileStereoStream = null;
        //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Создает поток записи выходного файла
        /// </summary>
        public static void CreateOutputStream()
        {
            try
            {
                //fs_output = new FileStream(encoder_outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            }
            catch (Exception ex)
            {
                LogHandler.WriteError("Encoder.cs->CreateOutputStream()", ex.Message);
                encoder_forceStop = true;
            }
        }

        /// <summary>
        /// Создает поток записи входного файла
        /// </summary>
        public static void CreateInputStream()
        {
            if (FileHandler.CheckStatus(encoder_inputFilePath, false) == "available")
                fs_input = new FileStream(encoder_inputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            else
            {
                LogHandler.WriteError("Encoder.cs->CreateInputStream()", encoder_inputFilePath + " is " + FileHandler.CheckStatus(encoder_inputFilePath, false));
                encoder_forceStop = true;
            }
        }

        public static int GetAudioFileDuration(string path)
        {
            AudioFileReader audioFileReader = new AudioFileReader(path);
            int minutes = audioFileReader.TotalTime.Minutes;
            return audioFileReader.TotalTime.Seconds + (minutes * 60);
        }

        public static int GetEncodingAudioDuration()
        {
            FileInfo fileInfo = new FileInfo(encoder_inputFilePath);
            long fileLength = fileInfo.Length;
            double bitsPerSec = (encoder_sampleRate * 2.0) / encoder_samplesPerBit;
            double realBitsPerSec = (32.0 / 41.0) * bitsPerSec;
            int bytesPerSec = (int)Math.Round(realBitsPerSec / 8.0);
            return (int)Math.Round((double)fileLength / (double)bytesPerSec);
        }

        /// <summary>
        /// Записывает заголовок WAV файла. stream - поток записи WAV, sampleRate - частота дискретизации WAV, channels - кол-во каналов
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sampleRate"></param>
        /// <param name="channels"></param>
        public static void WriteHeader(Stream stream, int sampleRate, int channels)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            short frameSize = (short)(16 / 8); // Количество байт в блоке (16 бит делим на 8).
            writer.Write(0x46464952); // Заголовок "RIFF".
            writer.Write(36 + (int)stream.Length * frameSize); // Размер файла от данной точки.
            writer.Write(0x45564157); // Заголовок "WAVE".
            writer.Write(0x20746D66); // Заголовок "frm ".
            writer.Write(16); // Размер блока формата.
            writer.Write((short)1); // Формат 1 значит PCM.
            writer.Write((short)channels); // Количество дорожек.
            writer.Write(sampleRate); // Частота дискретизации.
            writer.Write(sampleRate * frameSize); // Байтрейт (Как битрейт только в байтах).
            writer.Write(frameSize); // Количество байт в блоке.
            writer.Write((short)16); // разрядность.
            writer.Write(0x61746164); // Заголовок "DATA".
            writer.Write((int)stream.Length * frameSize); // Размер данных в байтах.
        }

        static void GenerateRAWDataBlockStereo(string binL, string binR)
        {
            //Дописать биты до 32
            for (int i = binL.Length; i < 32; i++) binL += "0";
            for (int i = binR.Length; i < 32; i++) binR += "0";
            string[] dataBlockL = BinaryHandler.HammingEncode(binL); //Массив бит блока данных левого канала
            string[] dataBlockR = BinaryHandler.HammingEncode(binR); //Массив бит блока данных правого канала
            string bitSequenceL = "";
            string bitSequenceR = "";
            //Перебираем финальную последовательность
            bitSequenceL += "s";
            for (int i = 0; i < dataBlockL.Length; i++) bitSequenceL += dataBlockL[i];
            bitSequenceL += "1";
            bitSequenceR += "s";
            for (int i = 0; i < dataBlockR.Length; i++) bitSequenceR += dataBlockR[i];
            bitSequenceR += "1";
            list_outputFileLSamples.AddRange(GenerateAMSignal(bitSequenceL, encoder_blockRate, encoder_samplesPerBit));
            list_outputFileRSamples.AddRange(GenerateAMSignal(bitSequenceR, encoder_blockRate, encoder_samplesPerBit));
        }

        static short[] GenerateAMSignal(string bitSequence, int blockRate, int samplesPerBit)
        {
            int amplitudeSync = short.MaxValue; // Амплитуда для синхроимпульса
            int amplitudeOne = amplitudeSync / 2; // Амплитуда для единицы
            int amplitudeZero = amplitudeOne / 2; // Амплитуда для нуля
            int sampleRate = blockRate * samplesPerBit * 40;
            encoder_sampleRate = sampleRate;
            int frequency = sampleRate / samplesPerBit;
            //while ((double)(samplesPerBit / 2.0) - Math.Truncate((double)(samplesPerBit / 2.0)) != 0) samplesPerBit++;
            short[] signal = new short[samplesPerBit * bitSequence.Length];

            for (int i = 0; i < bitSequence.Length; i++)
            {
                int amplitude;

                // Определяем амплитуду в зависимости от символа
                if (bitSequence[i] == 's')
                {
                    amplitude = amplitudeSync; // Синхроимпульс
                }
                else if (bitSequence[i] == '1')
                {
                    amplitude = amplitudeOne; // Амплитуда для единицы
                }
                else if (bitSequence[i] == '0')
                {
                    amplitude = amplitudeZero; // Амплитуда для нуля
                }
                else
                {
                    throw new ArgumentException("Неподдерживаемый символ в последовательности: " + bitSequence[i]);
                }

                // Генерация звукового сегмента
                for (int j = 0; j < samplesPerBit; j++)
                {

                    double t = j / (double)sampleRate;
                    signal[i * samplesPerBit + j] = (short)(amplitude * Math.Sin(2 * Math.PI * frequency * t));
                }
            }

            return signal;
        }

        static short[] LowPassFilter(short[] signal, int sampleRate, int cutoffFrequency)
        {
            double alpha = 1.0 / (1.0 + (sampleRate / (2 * Math.PI * cutoffFrequency)));
            alpha *= 0.5;

            short[] filteredSignal = new short[signal.Length];
            filteredSignal[0] = signal[0]; // Инициализация первого значения

            for (int i = 1; i < signal.Length; i++)
            {
                filteredSignal[i] = (short)(alpha * signal[i] + (1 - alpha) * filteredSignal[i - 1]);
            }

            return filteredSignal;
        }

        static short[] HighPassFilter(short[] signal, int sampleRate, int cutoffFrequency)
        {
            double RC = 1.0 / (2 * Math.PI * cutoffFrequency);
            double alpha = RC / (RC + (1.0 / sampleRate));

            short[] filteredSignal = new short[signal.Length];
            filteredSignal[0] = signal[0]; // Инициализация первого значения

            for (int i = 1; i < signal.Length; i++)
            {
                filteredSignal[i] = (short)(alpha * (filteredSignal[i - 1] + signal[i] - signal[i - 1]));
            }

            return filteredSignal;
        }

        static void SaveWav(string filename, short[] signal, int sampleRate)
        {
            using (var fs = new FileStream(filename, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                // Запись заголовка WAV
                bw.Write("RIFF".ToCharArray());
                bw.Write(36 + signal.Length * sizeof(short)); // Размер файла - 8 байт
                bw.Write("WAVE".ToCharArray());
                bw.Write("fmt ".ToCharArray());
                bw.Write(16); // Размер структуры fmt
                bw.Write((short)1); // Тип формата (1 = PCM)
                bw.Write((short)2); // Количество каналов
                bw.Write(sampleRate); // Частота дискретизации
                bw.Write(sampleRate * sizeof(short)); // Битрейт
                bw.Write((short)sizeof(short)); // Размер блока
                bw.Write((short)(8 * sizeof(short))); // Количество бит на сэмпл
                bw.Write("data".ToCharArray());
                bw.Write(signal.Length * sizeof(short)); // Размер данных
                
                foreach (short sample in signal)
                {
                    bw.Write(sample);
                }
            }
        }

        static void GenerateSubCodeBlockStereo(byte byte1L, byte byte2L, byte byte3L, byte byte4L, byte byte1R, byte byte2R, byte byte3R, byte byte4R)
        {
            string binL = "";
            string binR = "";
            string temp = "";
            temp = Convert.ToString(Convert.ToInt16(byte1L), 2);
            temp = temp.PadLeft(8, '0');
            binL += temp;
            temp = Convert.ToString(Convert.ToInt16(byte2L), 2);
            temp = temp.PadLeft(8, '0');
            binL += temp;
            temp = Convert.ToString(Convert.ToInt16(byte3L), 2);
            temp = temp.PadLeft(8, '0');
            binL += temp;
            temp = Convert.ToString(Convert.ToInt16(byte4L), 2);
            temp = temp.PadLeft(8, '0');
            binL += temp;
            temp = Convert.ToString(Convert.ToInt16(byte1R), 2);
            temp = temp.PadLeft(8, '0');
            binR += temp;
            temp = Convert.ToString(Convert.ToInt16(byte2R), 2);
            temp = temp.PadLeft(8, '0');
            binR += temp;
            temp = Convert.ToString(Convert.ToInt16(byte3R), 2);
            temp = temp.PadLeft(8, '0');
            binR += temp;
            temp = Convert.ToString(Convert.ToInt16(byte4R), 2);
            temp = temp.PadLeft(8, '0');
            binR += temp;

            //Дописать биты до 32
            for (int i = binL.Length; i < 32; i++) binL += "0";
            for (int i = binR.Length; i < 32; i++) binR += "0";
            string[] dataBlockL = BinaryHandler.HammingEncode(binL); //Массив бит блока данных левого канала
            string[] dataBlockR = BinaryHandler.HammingEncode(binR); //Массив бит блока данных правого канала
            string bitSequenceL = "";
            string bitSequenceR = "";
            //Перебираем финальную последовательность
            bitSequenceL += "s";
            for (int i = 0; i < dataBlockL.Length; i++) bitSequenceL += dataBlockL[i];
            bitSequenceL += "0";
            bitSequenceR += "s";
            for (int i = 0; i < dataBlockR.Length; i++) bitSequenceR += dataBlockR[i];
            bitSequenceR += "0";
            list_outputFileLSamples.AddRange(GenerateAMSignal(bitSequenceL, encoder_blockRate, encoder_samplesPerBit));
            list_outputFileRSamples.AddRange(GenerateAMSignal(bitSequenceR, encoder_blockRate, encoder_samplesPerBit));
        }

        public static void EncodeFileStereoStream()
        {
            string temp = "";
            string binaryL = "";
            string binaryR = "";
            LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding started");
            encoder_forceStop = false;
            CreateInputStream();
            CreateOutputStream();
            //Расставляем время воспроизведения по файлу
            int audioDuration = 0;
            int deltaByte = 0;
            if (encoder_mode == "mp3")
            {
                audioDuration = GetAudioFileDuration("");
                deltaByte = (int)Math.Round((double)((encoder_mpsPlayerSubCodeInterval * fs_input.Length) / audioDuration));
            }
            if (encoder_mode == "opus") { deltaByte = 2000; audioDuration = GetAudioFileDuration("input.wav"); }
            if (encoder_mode.Contains("sector")) deltaByte = (int)fs_input.Length / 4;
            List<int> targetBytePositions = new List<int>();          
            for (int i = deltaByte; i < fs_input.Length; i += deltaByte) targetBytePositions.Add(i);
            List<int> targetDurations = new List<int>();
            for (int i = 0; i < targetBytePositions.Count; i++) targetDurations.Add((int)Math.Round((double)((targetBytePositions[i] * (double)audioDuration) / fs_input.Length)));
            /*
            if (encoder_forceStop == true)
            {
                LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding aborted");
                if (fs_output != null)
                    fs_output.Close();
                if (fs_input != null)
                    fs_input.Close();
                Thread.CurrentThread.Abort();
            }
            */
            //fs_output.Seek(44, SeekOrigin.Begin); //Пропуск первых 44 байт потока, предназначенных для записи оглавления
            //GenerateVoid(encoder_silenceSeconds); //Генерация тишины 2 сек.
            //GenerateStereoSync(); //Генерация синхроимпульса
            for (int i = 0; i < 8; i++) GenerateSubCodeBlockStereo(123, 0, 0, 0, 123, 1, 1, 1); //Субкод канальной синхронизации (принудительная синхронизация каналов на начале потока)
            for (int i = 0; i < 32; i++) GenerateRAWDataBlockStereo("01010101010101010101010101010101", "01010101010101010101010101010101"); //Сигнал-заглушка для компенсации ошибки синхронизации
            GenerateSubCodeBlockStereo(123, 0, 0, 0, 123, 1, 1, 1);
            if (encoder_mode == "sector_header") GenerateSubCodeBlockStereo(24, 0, 0, 0, 24, 0, 0, 0); //Субкод lead-in для сектора заголовка 
            if (encoder_mode == "sector_hashes") GenerateSubCodeBlockStereo(25, 0, 0, 0, 25, 0, 0, 0); //Субкод lead-in для сектора контрольных сумм
            if (encoder_mode == "sector_file") GenerateSubCodeBlockStereo(26, 0, 0, (byte)encoder_sectorFileIndex, 26, 0, 0, (byte)encoder_sectorFileIndex); //Субкод lead-in для сектора файла
            //Преобразование данных в бинарный код
            for (int i = 0, k = 0; i < fs_input.Length;)
            {
                /*
                if (encoder_forceStop == true)
                {
                    LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding aborted");
                    if (fs_output != null)
                        fs_output.Close();
                    if (fs_input != null)
                        fs_input.Close();
                    Thread.CurrentThread.Abort();
                }
                */
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryL += temp;
                i++;k++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryL += temp;
                i++;k++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryL += temp;
                i++;k++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryL += temp;
                i++;k++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryR += temp;
                i++;k++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryR += temp;
                i++;k++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryR += temp;
                i++;k++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binaryR += temp;
                i++;k++;
                encoder_progress = ProgressHandler.GetPercent(fs_input.Length + 1, fs_input.Position);
                if (encoder_mode != "mp3" && encoder_mode != "opus") form_tapeRecordingWizard.sectorTable[encoder_sectorFileIndex - 1][3] = encoder_progress.ToString() + "%";
                GenerateRAWDataBlockStereo(binaryL, binaryR);
                binaryL = "";
                binaryR = "";
                //Запись секции субкодов
                if (targetBytePositions.Count > 0)
                {
                    if (fs_input.Position >= targetBytePositions[0])
                    {
                        if (encoder_mode == "mp3" || encoder_mode == "opus")
                        {
                            string part1 = Convert.ToString(targetDurations[0], 2);
                            string part2 = Convert.ToString(audioDuration, 2);
                            part1 = part1.PadLeft(12, '0');
                            part2 = part2.PadLeft(12, '0');
                            string sum = part1 + part2;
                            byte byteL1 = 100;
                            byte byteL2 = Convert.ToByte(sum.Substring(0, 8), 2);
                            byte byteL3 = Convert.ToByte(sum.Substring(8, 8), 2);
                            byte byteL4 = Convert.ToByte(sum.Substring(16, 8), 2);
                            byte byteR1 = 200;
                            byte byteR2 = Convert.ToByte(form_encoder.trackNumber);
                            byte byteR3 = Convert.ToByte(form_encoder.trackCount);
                            byte byteR4 = Convert.ToByte(0);
                            GenerateSubCodeBlockStereo(byteL1, byteL2, byteL3, byteL4, byteR1, byteR2, byteR3, byteR4);

                            int b1 = 10;
                            for (int m = 0; m < 24; m+=6, b1+=2)
                            {
                                byteL1 = (byte)b1;
                                byteL2 = ConvertSymbolToByte(form_encoder.artist[m]);
                                byteL3 = ConvertSymbolToByte(form_encoder.artist[m + 1]);
                                byteL4 = ConvertSymbolToByte(form_encoder.artist[m + 2]);
                                byteR1 = (byte)(b1+1);
                                byteR2 = ConvertSymbolToByte(form_encoder.artist[m + 3]);
                                byteR3 = ConvertSymbolToByte(form_encoder.artist[m + 4]);
                                byteR4 = ConvertSymbolToByte(form_encoder.artist[m + 5]);
                                GenerateSubCodeBlockStereo(byteL1, byteL2, byteL3, byteL4, byteR1, byteR2, byteR3, byteR4);
                            }
                            for (int m = 0; m < 24; m += 6, b1 += 2)
                            {
                                byteL1 = (byte)b1;
                                byteL2 = ConvertSymbolToByte(form_encoder.track[m]);
                                byteL3 = ConvertSymbolToByte(form_encoder.track[m + 1]);
                                byteL4 = ConvertSymbolToByte(form_encoder.track[m + 2]);
                                byteR1 = (byte)(b1 + 1);
                                byteR2 = ConvertSymbolToByte(form_encoder.track[m + 3]);
                                byteR3 = ConvertSymbolToByte(form_encoder.track[m + 4]);
                                byteR4 = ConvertSymbolToByte(form_encoder.track[m + 5]);
                                GenerateSubCodeBlockStereo(byteL1, byteL2, byteL3, byteL4, byteR1, byteR2, byteR3, byteR4);
                            }
                        }
                        GenerateSubCodeBlockStereo(123, 0, 0, 0, 123, 1, 1, 1); //Субкод канальной синхронизации
                        targetDurations.RemoveAt(0);
                        targetBytePositions.RemoveAt(0);
                    }
                }
            }
            if (encoder_mode == "sector_header") GenerateSubCodeBlockStereo(24, 1, 1, 1, 24, 1, 1, 1); //Субкод lead-out для сектора заголовка
            if (encoder_mode == "sector_hashes") GenerateSubCodeBlockStereo(25, 1, 1, 1, 25, 1, 1, 1); //Субкод lead-out для сектора контрольных сумм
            if (encoder_mode == "sector_file") GenerateSubCodeBlockStereo(26, 1, 1, (byte)encoder_sectorFileIndex, 26, 1, 1, (byte)encoder_sectorFileIndex); //Субкод lead-out для сектора файла
            for (int i = 0; i < 32; i++) GenerateRAWDataBlockStereo("01010101010101010101010101010101", "01010101010101010101010101010101");
            list_outputFileLSamples.AddRange(GenerateAMSignal("s", encoder_blockRate, encoder_samplesPerBit));
            list_outputFileRSamples.AddRange(GenerateAMSignal("s", encoder_blockRate, encoder_samplesPerBit));

            //short[] filteredSignalL = LowPassFilter(list_outputFileLSamples.ToArray(), encoder_sampleRate, (int)(encoder_frequency * 1.5));
            //short[] filteredSignalR = LowPassFilter(list_outputFileRSamples.ToArray(), encoder_sampleRate, (int)(encoder_frequency * 1.5));
            //filteredSignalL = LowPassFilter(filteredSignalL, encoder_sampleRate, encoder_frequency);
            //filteredSignalR = LowPassFilter(filteredSignalR, encoder_sampleRate, encoder_frequency);
            short[] srereoSignal = CreateStereoSignal(list_outputFileLSamples.ToArray(), list_outputFileRSamples.ToArray());

            SaveWav(encoder_outputFilePath, srereoSignal, encoder_sampleRate);

            list_outputFileLSamples.Clear();
            list_outputFileRSamples.Clear();
            list_outputFileSamples.Clear();

            fs_input.Close();
            fs_input.Dispose();

            encoder_progress = ProgressHandler.GetPercent(100, 100);
            LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding finished");
            form_encoder.trackNumber++;
        }

        static short[] CreateStereoSignal(short[] leftSignal, short[] rightSignal)
        {
            int stereoLength = leftSignal.Length + rightSignal.Length;
            short[] stereoSignal = new short[stereoLength];
            for (int i = 0; i < leftSignal.Length; i++)
            {
                stereoSignal[i * 2] = leftSignal[i];      // Левый канал
                stereoSignal[i * 2 + 1] = rightSignal[i]; // Правый канал
            }
            return stereoSignal;
        }

        public static byte ConvertSymbolToByte(char symbol)
        {            
            if (alphabet.Contains(symbol)) return (byte)Array.IndexOf(alphabet, symbol);
            else return (byte)Array.IndexOf(alphabet, '?');
        }
        public static char ConvertByteToChar(byte b)
        {
            if ((int)b < alphabet.Length) return alphabet[b];
            else return '?';
        }
    }
}
