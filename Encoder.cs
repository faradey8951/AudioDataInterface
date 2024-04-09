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
        public static string[] alphabets = { "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 
                                             "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ",
                                             "1234567890",
                                             @"~!@#$%^&*()_+`-=<>,.?/|\"}; //Алфавиты символов
        //////////////////////////////////////////////////////////////////////////////////////

        //Энкодер
        //Префикс: "encoder_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static string encoder_outputFilePath = "output.wav"; //Путь к выходному файлу энкодера
        public static string encoder_inputFilePath = ""; //Путь ко входному файлу энкодера
        public static int encoder_signalGain = 2; //Коэффициент усиления аудиосигнала выходного файла
        public static int encoder_sampleRate = 82000; //Частота дискретизации выходного файла
        public static bool encoder_ADIFShell = false; //Функция ADIFShell
        public static bool encoder_forceStop = false; //Принудительная остановка конвертации
        public static int encoder_samplesPerBit = 8; //Кол-во аудиосэмплов, описывающих один бит информации в аудиопотоке
        public static int encoder_progress = 0; //Прогресс конвертации [%]
        public static double minSampleDeltaCoefficient = 0.4;
        public static double maxSampleDeltaCoefficient = 0.9;
        public static int encoder_silenceSeconds = 0;
        public static int encoder_leadInOutSubcodesAmount = 0;
        public static int encoder_leadInSubcodesAmount = 0;
        public static int encoder_mpsPlayerSubCodeInterval = 0;
        public static bool encoder_longLeadIn = false;
        public static string encoder_mode = "";
        public static string encoder_ffmpeg1Cmd = "-vn -ar 11025 -ac 1 -b:a 16k -map 0:a -map_metadata -1";
        public static string encoder_ffmpeg2Cmd = "-vn -ar 11025 -ac 1 -b:a 16k";
        public static string encoder_ffmpeg2EffectCmd = "equalizer=f=5000:width_type=h:width=1000:g=20";
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
                fs_output = new FileStream(encoder_outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
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

        public static int GetMp3FileDuration()
        {
            Mp3FileReader reader = new Mp3FileReader(encoder_inputFilePath);
            int minutes = reader.TotalTime.Minutes;
            return reader.TotalTime.Seconds + (minutes * 60);
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

        /// <summary>
        /// Метод генерации синхроимпульса. fs_output - поток записи WAV
        /// </summary>
        /// <param name="fs_output"></param>
        static void GenerateSync()
        {
            list_outputFileSamples.Add(Convert.ToInt16(4096 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Clear();
        }

        static void GenerateStereoSync()
        {
            double maxSampleLevelL = 4096;
            double minSampleDeltaL = maxSampleLevelL * minSampleDeltaCoefficient;
            double maxSampleDeltaL = maxSampleLevelL * maxSampleDeltaCoefficient;
            double maxSampleLevelR = 4096;
            double minSampleDeltaR = maxSampleLevelR * minSampleDeltaCoefficient;
            double maxSampleDeltaR = maxSampleLevelR * maxSampleDeltaCoefficient;
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaL * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaR * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();
        }

        /// <summary>
        /// Метод генерации прямоугольного сигнала. picth - тон, fs_output - поток записи WAV, duration - длительность
        /// </summary>
        /// <param name="fs_output"></param>
        /// <param name="duration"></param>
        /// <param name="pitch"></param>
        static void GenerateSquare(int duration, int pitch)
        {
            int encoder_samplePerBit = pitch + 4;
            for (int i = 0; i < (int)((encoder_sampleRate * duration) / encoder_samplePerBit); i++)
            {
                list_outputFileSamples.Add(Convert.ToInt16(4096 * encoder_signalGain));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                for (int b = 0; b < pitch; b++)
                {
                    list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (0 * encoder_signalGain))));
                    foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                        fs_output.WriteByte(element);
                }
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (4096 * encoder_signalGain))));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (4096 * encoder_signalGain))));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                for (int b = 0; b < pitch; b++)
                {
                    list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (0 * encoder_signalGain))));
                    foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                        fs_output.WriteByte(element);
                }
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (4096 * encoder_signalGain))));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Clear();
            }
        }

        /// <summary>
        /// Метод генерации тишины. fs_output - поток записи файла WAV, duration - длительность тишины
        /// </summary>
        /// <param name="fs_output"></param>
        /// <param name="duration"></param>
        static void GenerateVoid(int duration)
        {
            int encoder_samplePerBit = 8; //Кол-во сэмплов на 1 бит

            for (int i = 0; i < (int)((encoder_sampleRate * duration) / encoder_samplePerBit); i++)
            {
                list_outputFileSamples.Add(Convert.ToInt16(0));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1])));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1])));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1])));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1])));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1])));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1])));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1])));
                foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                    fs_output.WriteByte(element);
                list_outputFileSamples.Clear();
            }
        }

        /// <summary>
        /// Метод генерации полупериода нуля. fs_output - поток записи WAV
        /// </summary>
        /// <param name="fs_output"></param>
        static void GenerateZero()
        {
            list_outputFileSamples.Add(Convert.ToInt16(1024 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Clear();
        }

        static void GenerateStereo00()
        {
            double maxSampleLevelL = 1024;
            double minSampleDeltaL = maxSampleLevelL * minSampleDeltaCoefficient;
            double maxSampleDeltaL = maxSampleLevelL * maxSampleDeltaCoefficient;
            double maxSampleLevelR = 1024;
            double minSampleDeltaR = maxSampleLevelR * minSampleDeltaCoefficient;
            double maxSampleDeltaR = maxSampleLevelR * maxSampleDeltaCoefficient;
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaL * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaR * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();

        }

        static void GenerateStereo01()
        {
            double maxSampleLevelL = 1024;
            double minSampleDeltaL = maxSampleLevelL * minSampleDeltaCoefficient;
            double maxSampleDeltaL = maxSampleLevelL * maxSampleDeltaCoefficient;
            double maxSampleLevelR = 2048;
            double minSampleDeltaR = maxSampleLevelR * minSampleDeltaCoefficient;
            double maxSampleDeltaR = maxSampleLevelR * maxSampleDeltaCoefficient;
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaL * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaR * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();

        }

        static void GenerateStereo10()
        {
            double maxSampleLevelL = 2048;
            double minSampleDeltaL = maxSampleLevelL * minSampleDeltaCoefficient;
            double maxSampleDeltaL = maxSampleLevelL * maxSampleDeltaCoefficient;
            double maxSampleLevelR = 1024;
            double minSampleDeltaR = maxSampleLevelR * minSampleDeltaCoefficient;
            double maxSampleDeltaR = maxSampleLevelR * maxSampleDeltaCoefficient;
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaL * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaR * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();

        }

        static void GenerateStereo11()
        {
            double maxSampleLevelL = 2048;
            double minSampleDeltaL = maxSampleLevelL * minSampleDeltaCoefficient;
            double maxSampleDeltaL = maxSampleLevelL * maxSampleDeltaCoefficient;
            double maxSampleLevelR = 2048;
            double minSampleDeltaR = maxSampleLevelR * minSampleDeltaCoefficient;
            double maxSampleDeltaR = maxSampleLevelR * maxSampleDeltaCoefficient;
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaL * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(maxSampleDeltaR * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (minSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaL * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (maxSampleDeltaR * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();

        }

        /// <summary>
        /// Метод генерации полупериода единицы. fs_output - поток записи WAV
        /// </summary>
        /// <param name="fs_output"></param>
        static void GenerateOne()
        {
            list_outputFileSamples.Add(Convert.ToInt16(2048 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 1] + (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Clear();
        }

        /// <summary>
        /// Генерирует блок маркер с кодом markerCode [0-255], в количестве count
        /// </summary>
        /// <param name="markerCode"></param>
        /// <param name="count"></param>
        static void GenerateMarkerDataBlock(int markerCode, int count)
        {
            for (int k = 0; k < count; k++)
            {
                string bin = ""; //Бинарный код маркера
                bin = Convert.ToString(Convert.ToInt16(markerCode), 2);
                bin = bin.PadLeft(8, '0');
                //Дописать биты до 32
                for (int i = bin.Length; i < 32; i++)
                    bin += "0";
                string[] dataBlock = BinaryHandler.HammingEncode(bin); //Массив бит блока данных
                                                                //Перебираем финальную кодовую последовательность
                for (int i = 0; i < dataBlock.Length; i++)
                {
                    if (dataBlock[i] == "0")
                        GenerateZero();
                    else
                        GenerateOne();
                }
                GenerateOne(); //Генерируем указатель маркер-блока
                GenerateSync(); //Генерируем синхроимпульс
            }
        }

        /// <summary>
        /// Генерирует 39-битный блок данных bin
        /// </summary>
        /// <param name="bin"></param>
        static void GenerateRAWDataBlock(string bin)
        {
            //Дописать биты до 32
            for (int i = bin.Length; i < 32; i++)
                bin += "0";
            string[] dataBlock = BinaryHandler.HammingEncode(bin); //Массив бит блока данных
            //Перебираем финальную последовательность
            for (int i = 0; i < dataBlock.Length; i++)
            {
                if (dataBlock[i] == "0")
                    GenerateZero();
                else
                    GenerateOne();
            }
            GenerateZero(); //Генерируем указатель RAW-блока
            GenerateSync(); //Генерируем синхроимпульс
        }

        static void GenerateRAWDataBlockStereo(string binL, string binR)
        {
            //Дописать биты до 32
            for (int i = binL.Length; i < 32; i++) binL += "0";
            for (int i = binR.Length; i < 32; i++) binR += "0";     
            string[] dataBlockL = BinaryHandler.HammingEncode(binL); //Массив бит блока данных левого канала
            string[] dataBlockR = BinaryHandler.HammingEncode(binR); //Массив бит блока данных правого канала
            //Перебираем финальную последовательность
            for (int i = 0; i < dataBlockL.Length; i++)
            {
                if (dataBlockL[i] == "0" && dataBlockR[i] == "0") GenerateStereo00();
                if (dataBlockL[i] == "1" && dataBlockR[i] == "1") GenerateStereo11();
                if (dataBlockL[i] == "1" && dataBlockR[i] == "0") GenerateStereo10();
                if (dataBlockL[i] == "0" && dataBlockR[i] == "1") GenerateStereo01();
            }
            GenerateStereo11();
            GenerateStereoSync(); //Генерируем синхроимпульс
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
            //Перебираем финальную последовательность
            for (int i = 0; i < dataBlockL.Length; i++)
            {
                if (dataBlockL[i] == "0" && dataBlockR[i] == "0") GenerateStereo00();
                if (dataBlockL[i] == "1" && dataBlockR[i] == "1") GenerateStereo11();
                if (dataBlockL[i] == "1" && dataBlockR[i] == "0") GenerateStereo10();
                if (dataBlockL[i] == "0" && dataBlockR[i] == "1") GenerateStereo01();
            }
            GenerateStereo00();
            GenerateStereoSync(); //Генерируем синхроимпульс
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
            int mp3Duration = 0;
            if (encoder_mode == "mp3") mp3Duration = GetMp3FileDuration();
            else mp3Duration = GetEncodingAudioDuration();
            int deltaByte = (int)Math.Round((double)((encoder_mpsPlayerSubCodeInterval * fs_input.Length) / mp3Duration));
            List<int> targetBytePositions = new List<int>();          
            for (int i = 0; i < fs_input.Length; i += deltaByte) targetBytePositions.Add(i);
            List<int> targetDurations = new List<int>();
            for (int i = 0; i < targetBytePositions.Count; i++) targetDurations.Add((int)Math.Round((double)((targetBytePositions[i] * (double)mp3Duration) / fs_input.Length)));
            if (encoder_forceStop == true)
            {
                LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding aborted");
                if (fs_output != null)
                    fs_output.Close();
                if (fs_input != null)
                    fs_input.Close();
                Thread.CurrentThread.Abort();
            }
            fs_output.Seek(44, SeekOrigin.Begin); //Пропуск первых 44 байт потока, предназначенных для записи оглавления
            GenerateVoid(encoder_silenceSeconds); //Генерация тишины 2 сек.
            GenerateStereoSync(); //Генерация синхроимпульса       
            for (int i = 0; i < encoder_leadInSubcodesAmount; i++) GenerateSubCodeBlockStereo(50, 50, 50, 50, 50, 50, 50, 50);
            //Преобразование данных в бинарный код
            for (int i = 0, k = 0; i < fs_input.Length;)
            {
                if (encoder_forceStop == true)
                {
                    LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding aborted");
                    if (fs_output != null)
                        fs_output.Close();
                    if (fs_input != null)
                        fs_input.Close();
                    Thread.CurrentThread.Abort();
                }
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
                GenerateRAWDataBlockStereo(binaryL, binaryR);
                binaryL = "";
                binaryR = "";

                if (targetBytePositions.Count > 0)
                {
                    if (fs_input.Position >= targetBytePositions[0])
                    {
                        if (encoder_mode == "mp3")
                        {
                            string part1 = Convert.ToString(targetDurations[0], 2);
                            string part2 = Convert.ToString(mp3Duration, 2);
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
                        }
                        GenerateSubCodeBlockStereo(123, 0, 0, 0, 123, 1, 1, 1);
                        targetDurations.RemoveAt(0);
                        targetBytePositions.RemoveAt(0);
                    }
                }
            }
            for (int i = 0; i < encoder_leadInOutSubcodesAmount; i++) GenerateSubCodeBlockStereo(60, 60, 60, 60, 60, 60, 60, 60);
            fs_output.Seek(0, SeekOrigin.Begin); //Переход на 0 положение потока записи, для записи оглавления wave файла
            WriteHeader(fs_output, encoder_sampleRate, 2);
            fs_output.Close();
            fs_output.Dispose();
            fs_input.Close();
            fs_input.Dispose();
            encoder_progress = ProgressHandler.GetPercent(100, 100);
            LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding finished");
            form_encoder.trackNumber++;
        }

        public static void EncodeFileStream()
        {
            string temp = "";
            string binary = "";
            LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding started");
            encoder_forceStop = false;
            CreateInputStream();
            CreateOutputStream();
            if (encoder_forceStop == true)
            {
                LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding aborted");
                if (fs_output != null)
                    fs_output.Close();
                if (fs_input != null)
                    fs_input.Close();
                Thread.CurrentThread.Abort();
            }    
            fs_output.Seek(44, SeekOrigin.Begin); //Пропуск первых 44 байт потока, предназначенных для записи оглавления
            GenerateVoid(2); //Генерация тишины 2 сек.
            GenerateSync(); //Генерация синхроимпульса       
            GenerateMarkerDataBlock(0, 3); //Генерация маркера начала потока
            //Преобразование данных в бинарный код
            for (int i = 0; i < fs_input.Length;)
            {
                if (encoder_forceStop == true)
                {
                    LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding aborted");
                    if (fs_output != null)
                        fs_output.Close();
                    if (fs_input != null)
                        fs_input.Close();
                    Thread.CurrentThread.Abort();
                }
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binary += temp;
                i++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binary += temp;
                i++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binary += temp;
                i++;
                temp = fs_input.ReadByte().ToString();
                temp = Convert.ToString(Convert.ToInt16(temp), 2);
                temp = temp.PadLeft(8, '0');
                binary += temp;
                i++;
                encoder_progress = ProgressHandler.GetPercent(fs_input.Length + 1, fs_input.Position);
                GenerateRAWDataBlock(binary);
                binary = "";
            }
            fs_output.Seek(0, SeekOrigin.Begin); //Переход на 0 положение потока записи, для записи оглавления wave файла
            WriteHeader(fs_output, encoder_sampleRate, 1);
            fs_output.Close();
            fs_output.Dispose();
            fs_input.Close();
            fs_input.Dispose();
            encoder_progress = ProgressHandler.GetPercent(100, 100);
            LogHandler.WriteStatus("Encoder.cs->EncoderFileStream()", "Encoding finished");
        }

        /*
        /// <summary>
        /// Конвертирует файл в аудиопоток с оболочкой ADI-FShell
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="filePath"></param>
        public static void EncodeFileADIFShell()
        {
            encoder_forceStop = false;
            try
            {
                if (File.Exists("output.wav"))
                    File.Delete("output.wav");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DebugHandler.Write("Encoder.cs->EncodeFileStream", ex.Message);
                encoder_forceStop = true;
            }
            if (encoder_forceStop == true)
                Thread.CurrentThread.Abort();
            encoder_forceStop = false;
            CreateInputStream();
            CreateOutputStream();
            fs_output.Seek(44, SeekOrigin.Begin);
            GenerateVoid(fs_output, 2) //Генерация тишины 2 сек.           
            GenerateTextMarker(fs_output, "254", 1024); //Генерация маркера начала потока текстовых данных
            GenerateSync(fs_output); //Генерация синхроимпульса
            GenerateTextMarker(fs_output, "253", 3); //Генерация маркера начала оглавления файла
            List<byte> encBytes = new List<byte>(); //Список байт кодируемого файла
            //Формирование списка байт кодируемого файла
            for (int i = 0; i < fs_input.Length; i++)
                encBytes.Add((byte)fs_input.ReadByte());
            string header = ""; //Заголовок файла
            string fileName = Path.GetFileName(encoder_inputFilePath); //Имя кодируемого файла
            int fileSize = (int)fs_input.Length; //Размер кодируемого файла в байтах
            int clusterSize = (int)fileSize / 32; //Размер кластера
            int blocksInCluster = (int)clusterSize / 4; //Кол-во блоков данных в кластере
            clusterSize = blocksInCluster * 4; //Расчитанный размер кластера
            int byteOffset = 0; //Кол-во дополнительных байт, необходимых для кодирования файла с текущей разметкой
            int partialSize = clusterSize * 32; //Кол-во байт файла, которые можно записать с текущей разметкой
            //Вычисление смещения
            while (((double)(fileSize + byteOffset - partialSize) / 4) - Math.Truncate(((double)(fileSize + byteOffset - partialSize) / 4)) != 0)
                byteOffset++;
            //Дописываем нулевые байты в конец файла
            for (int i = 0; i < byteOffset; i++)
                encBytes.Add(0);
            //Формирование заголовка файла
            header = fileName + ";" + fileSize + ";" + clusterSize + ";" + blocksInCluster + ";" + byteOffset + ";" + (fileSize - partialSize + byteOffset).ToString() + ";";
            string temp = "";
            //Вычисление хеш-сумм кластеров
            for (int i = 0; i < encBytes.Count;)
            {
                string cluster = ""; //Текущий кластер
                for (int p = 0, k = 0; p < 32; p++, k += clusterSize)
                {
                    //КОД ОБНОВЛЕНИЯ ПРОГРЕСС БАРА ДЛЯ ХЕШ-СУММ

                    cluster = "";
                    if (p == 31) //Коррекция условий для последнего кластера
                        clusterSize += fileSize - partialSize + byteOffset;
                    while (i < k + clusterSize)
                    {
                        temp = encBytes[i].ToString();
                        temp = Convert.ToString(Convert.ToInt16(temp), 2);
                        temp = temp.PadLeft(8, '0');
                        cluster += temp;
                        i++;
                    }
                    header += BinaryHandler.GetHash(cluster) + ";"; //Записываем хеш-сумму кластера в оглавление
                }
            }
            clusterSize -= fileSize - partialSize + byteOffset; //Возвращаем оригинальное значение
            header += RawData.GetHash(header) + ";";//Добавляет хеш-сумму текущего оглавления в конец оглавления
            //Запись оглавления в WAV
            int length = header.Length; //Кол-во символов в оглавлении
            string binary = ""; //Бинарный код текущего символа
            string currentSymbol = ""; //Текущий символ
            temp = "";
            string[] block = null;
            //Перебор символов текста
            for (int i = 0; i < length; i++)
            {
                //Остановить кодирование, при ручной отмене
                if (GeneralForm.switch_disposeEncoding == true)
                    Thread.CurrentThread.Abort();
                currentSymbol = header[i].ToString();
                GeneralForm.temp_currentSymbolLibrary = GeneralForm.locOut.GetLibrary(currentSymbol); //Получить индекс символьной библиотеки для текущего символа текста
                //Закодировать символ согласно его символьной библиотеке
                switch (GeneralForm.temp_currentSymbolLibrary)
                {
                    case -1:
                        binary = "0";
                        break;
                    case 0:
                        binary = GeneralForm.locOut.SymbolLibEncode(currentSymbol);
                        GeneralForm.temp_currentSymbolLibrary = GeneralForm.temp_lastSymbolLibrary;
                        break;
                    case 1:
                        binary = Array.IndexOf(GeneralForm.locOut.alphabetEN, currentSymbol).ToString();
                        break;
                    case 2:
                        binary = Array.IndexOf(GeneralForm.locOut.alphabetRU, currentSymbol).ToString();
                        break;
                }
                //Если произошла смена символьной библиотеки, сгенерировать маркер символьной библиотеки
                if (GeneralForm.temp_currentSymbolLibrary != GeneralForm.temp_lastSymbolLibrary && (GeneralForm.locOut.CheckLibForExist(GeneralForm.temp_currentSymbolLibrary) == true))
                    GenerateTextMarker(fs_output, GeneralForm.temp_currentSymbolLibrary.ToString(), 3);
                GeneralForm.temp_lastSymbolLibrary = GeneralForm.temp_currentSymbolLibrary;
                binary = Convert.ToString(Convert.ToInt16(binary), 2); //Перевести символ в двоичную систему
                binary = binary.PadLeft(8, '0'); //Дописать нули слева до 8 знаков
                block = GeneralForm.rawData.Construct(binary); //Дополнить блок данных контрольными битами
                //Перебираем финальную бинарную последовательность
                for (int u = 0; u < block.Length; u++)
                {
                    if (block[u] == "0")
                        GenerateZero(fs_output);
                    else
                        GenerateOne(fs_output);
                }            
                GenerateSync(fs_output); //Генерация синхроимпульса
            }            
            GenerateTextMarker(fs_output, "252", 1); //Генерация маркера конца оглавления файла         
            GenerateTextMarker(fs_output, "251", 3); //Генерация маркера команды на переход в режим получения данных файла          
            GenerateDataMarker(fs_output, "00000000000000000000000000000000", 512); //Генерация маркера начала потока данных
            //Кодирование кластеров
            for (int i = 0; i < encBytes.Count;)
            {
                var cluster = new List<string>(); //Список блоков данных в кластере
                string currentBlock = ""; //Буфер для одного 4 байт под блок данных
                for (int p = 0, k = 0; p < 32; p++, k += clusterSize)
                {
                    //КОД ОБНОВЛЕНИЯ ПРОГРЕСС БАРА ДЛЯ КОДИРОВАНИЯ КЛАСТЕРОВ

                    cluster.Clear();
                    if (p == 31) //Коррекция условий для последнего кластера
                        clusterSize += fileSize - partialSize + byteOffset;
                    while (i < k + clusterSize)
                    {
                        temp = encBytes[i].ToString();
                        temp = Convert.ToString(Convert.ToInt16(temp), 2);
                        temp = temp.PadLeft(8, '0');
                        currentBlock += temp;
                        //Когда буфер наполнился, записываем блок в список
                        if (currentBlock.Length == 32)
                        {
                            cluster.Add(currentBlock);
                            currentBlock = "";
                        }
                        i++;
                    }                 
                    GenerateDataMarker(fs_output, "01010101010101010101010101010101", 1); //Генерация маркера начала кластера
                    //Кодируем текущий кластер
                    for (int n = 0; n < cluster.Count; n++)
                    {
                        string[] tempBlock = BinaryHandler.HCMake(cluster[n]); //Дополнить блок данных контрольными битами
                        //Перебираем финальную кодовую последовательность
                        for (int u = 0; u < tempBlock.Length; u++)
                        {
                            if (tempBlock[u] == "0")
                                GenerateZero(fs_output);
                            else
                                GenerateOne(fs_output);
                        }                        
                        GenerateSync(fs_output); //Генерация синхроимпульса
                    }                    
                    GenerateDataMarker(fs_output, "10101010101010101010101010101010", 1); //Генерация маркера конца кластера
                }
            }
            clusterSize -= fileSize - partialSize + byteOffset; //Возвращаем оригинальное значение       
            GenerateDataMarker(fs_output, "00100100100100100100100100100100", 3); //Генерация маркера конца файла           
            GenerateDataMarker(fs_output, "00000000000000000000000000000000", 512); //Генерация маркера начала потока данных
            fs_output.Seek(0, SeekOrigin.Begin);
            GeneralForm.writeHeader(fs_output, sampleRate, 1);
            fs_output.Close();
            fs_input.Close();
            GeneralForm.temp_progressValue = GeneralForm.progress.GetPercent(100, 100);
        }
        */
    }
}
