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
    /// Конвертатор цифровых данных в звуковые аудиофайлы
    /// </summary>
    public class Encoder
    {
        //Другое
        //////////////////////////////////////////////////////////////////////////////////////
        public static FileStream fs_output = null;
        public static FileStream fs_input = null;
        public static List<short> list_outputFileSamples = new List<short>(); //Коллекция сэмплов для записи выходного файла
        public static string[] alphabets = { "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 
                                             "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"}; //Алфавиты символов
        //////////////////////////////////////////////////////////////////////////////////////

        //Энкодер
        //Префикс: "encoder_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static string encoder_outputFilePath = "output.wav";
        public static string encoder_inputFilePath = "";
        public static int encoder_signalGain = 2; //Коэффициент усиления аудиосигнала выходного файла
        public static int encoder_sampleRate = 72000; //Частота дискретизации выходного файла
        public static bool encoder_ADIFShell = false; //Функция ADIFShell
        public static bool encoder_forceStop = false; //Принудительная остановка конвертации
        public static int encoder_samplesPerBit = 8; //Кол-во аудиосэмплов, описывающих один бит информации в аудиопотоке
        public static int encoder_progress = 0; //Прогресс конвертации [%]
        //////////////////////////////////////////////////////////////////////////////////////

        public static Thread thread_encodeFileStream = null;

        /// <summary>
        /// Создает поток записи выходного файла
        /// </summary>
        public static void CreateOutputStream()
        {
            fs_output = new FileStream(encoder_outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        /// <summary>
        /// Создает поток записи входного файла
        /// </summary>
        public static void CreateInputStream()
        {
            fs_input = new FileStream(encoder_inputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
            list_outputFileSamples.Add(Convert.ToInt16(4096 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(4096 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2560 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (4096 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (4096 * encoder_signalGain))));
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
            list_outputFileSamples.Add(Convert.ToInt16(1024 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(1024 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();
        }

        static void GenerateStereo01()
        {
            list_outputFileSamples.Add(Convert.ToInt16(1024 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(2048 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();
        }

        static void GenerateStereo10()
        {
            list_outputFileSamples.Add(Convert.ToInt16(2048 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(1024 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (576 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1024 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Clear();
        }

        static void GenerateStereo11()
        {
            list_outputFileSamples.Add(Convert.ToInt16(2048 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16(2048 * encoder_signalGain));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] - (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (1280 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);

            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2048 * encoder_signalGain))));
            foreach (byte element in BitConverter.GetBytes(list_outputFileSamples[list_outputFileSamples.Count - 1]))
                fs_output.WriteByte(element);
            list_outputFileSamples.Add(Convert.ToInt16((list_outputFileSamples[list_outputFileSamples.Count - 2] + (2048 * encoder_signalGain))));
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
                string[] dataBlock = BinaryHandler.HCMake(bin); //Массив бит блока данных
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
            string[] dataBlock = BinaryHandler.HCMake(bin); //Массив бит блока данных
            //Перебираем финальную кодовую последовательность
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

        public static void EncodeFileStream()
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
                LogHandler.Write("Encoder.cs->EncodeFileStream", ex.Message);
                encoder_forceStop = true;
            }
            if (encoder_forceStop == true)
                Thread.CurrentThread.Abort();
            encoder_forceStop = false;
            CreateInputStream();
            CreateOutputStream();
            fs_output.Seek(44, SeekOrigin.Begin); //Пропуск первых 44 байт потока, предназначенных для записи оглавления
            GenerateVoid(2); //Генерация тишины 2 сек.
            GenerateSync(); //Генерация синхроимпульса       
            GenerateMarkerDataBlock(0, 3); //Генерация маркера начала потока
            string binSymbol = "";
            string tempB = "";
            //Преобразование данных в бинарный код
            for (int i = 0; i < fs_input.Length;)
            {
                if (encoder_forceStop == true)
                    Thread.CurrentThread.Abort();
                tempB = fs_input.ReadByte().ToString();
                tempB = Convert.ToString(Convert.ToInt16(tempB), 2);
                tempB = tempB.PadLeft(8, '0');
                binSymbol += tempB;
                i++;
                tempB = fs_input.ReadByte().ToString();
                tempB = Convert.ToString(Convert.ToInt16(tempB), 2);
                tempB = tempB.PadLeft(8, '0');
                binSymbol += tempB;
                i++;
                tempB = fs_input.ReadByte().ToString();
                tempB = Convert.ToString(Convert.ToInt16(tempB), 2);
                tempB = tempB.PadLeft(8, '0');
                binSymbol += tempB;
                i++;
                tempB = fs_input.ReadByte().ToString();
                tempB = Convert.ToString(Convert.ToInt16(tempB), 2);
                tempB = tempB.PadLeft(8, '0');
                binSymbol += tempB;
                i++;
                encoder_progress = ProgressHandler.GetPercent(fs_input.Length + 1, fs_input.Position);
                GenerateRAWDataBlock(binSymbol);
                binSymbol = "";
            }
            fs_output.Seek(0, SeekOrigin.Begin); //Переход на 0 положение потока записи, для нанесения оглавления wave файла
            WriteHeader(fs_output, encoder_sampleRate, 1);
            fs_output.Close();
            fs_output.Dispose();
            fs_input.Close();
            fs_input.Dispose();
            encoder_progress = ProgressHandler.GetPercent(100, 100);
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
            string binSymbol = ""; //Бинарный код текущего символа
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
                        binSymbol = "0";
                        break;
                    case 0:
                        binSymbol = GeneralForm.locOut.SymbolLibEncode(currentSymbol);
                        GeneralForm.temp_currentSymbolLibrary = GeneralForm.temp_lastSymbolLibrary;
                        break;
                    case 1:
                        binSymbol = Array.IndexOf(GeneralForm.locOut.alphabetEN, currentSymbol).ToString();
                        break;
                    case 2:
                        binSymbol = Array.IndexOf(GeneralForm.locOut.alphabetRU, currentSymbol).ToString();
                        break;
                }
                //Если произошла смена символьной библиотеки, сгенерировать маркер символьной библиотеки
                if (GeneralForm.temp_currentSymbolLibrary != GeneralForm.temp_lastSymbolLibrary && (GeneralForm.locOut.CheckLibForExist(GeneralForm.temp_currentSymbolLibrary) == true))
                    GenerateTextMarker(fs_output, GeneralForm.temp_currentSymbolLibrary.ToString(), 3);
                GeneralForm.temp_lastSymbolLibrary = GeneralForm.temp_currentSymbolLibrary;
                binSymbol = Convert.ToString(Convert.ToInt16(binSymbol), 2); //Перевести символ в двоичную систему
                binSymbol = binSymbol.PadLeft(8, '0'); //Дописать нули слева до 8 знаков
                block = GeneralForm.rawData.Construct(binSymbol); //Дополнить блок данных контрольными битами
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
