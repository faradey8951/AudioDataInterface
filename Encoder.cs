using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public class Encoder
    {
        //Другое
        //////////////////////////////////////////////////////////////////////////////////////
        public static FileStream fs_output = null;
        public static FileStream fs_input = null;
        public static List<short> list_outputFileSamples = new List<short>(); //Коллекция сэмплов для записи выходного файла
        //////////////////////////////////////////////////////////////////////////////////////

        //Энкодер
        //Префикс: "encoder_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static string encoder_outputFilePath = "output.wav";
        public static string encoder_inputFilePath = "output.wav";
        public static int encoder_signalGain = 2; //Коэффициент усиления аудиосигнала выходного файла
        public static int encoder_sampleRate = 48000; //Частота дискретизации выходного файла
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
                DebugHandler.Write("Encoder.cs->EncodeFileStream", ex.Message);
                encoder_forceStop = true;
            }
            if (encoder_forceStop == true)
                Thread.CurrentThread.Abort();
            encoder_forceStop = false;
            CreateInputStream();
            CreateOutputStream();
            fs_output.Seek(44, SeekOrigin.Begin); //Пропуск первых 44 байт потока, предназначенных для записи оглавления
            GenerateVoid(2); //Генерация тишины 2 сек.
            GenerateSync(); //Генерируем синхроимпульс         

            //ВСТАВИТЬ МАРКЕР НАЧАЛА ПОТОКА

            string binSymbol = "";
            string tempB = "";
            string[] block = null;
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
                block = HCECHandler.Compute(binSymbol); //Дополнить блок данных контрольными битами
                binSymbol = "";
                //Перебираем финальную кодовую последовательность
                for (int u = 0; u < block.Length; u++)
                {
                    if (block[u] == "0")
                        GenerateZero();
                    else
                        GenerateOne();
                }
                GenerateSync(); //Генерируем синхроимпульс
            }          
            fs_output.Seek(0, SeekOrigin.Begin); //Переход на 0 положение потока записи, для нанесения оглавления wave файла
            WriteHeader(fs_output, encoder_sampleRate, 1);
            fs_output.Close();
            fs_output.Dispose();
            fs_input.Close();
            fs_input.Dispose();
            encoder_progress = ProgressHandler.GetPercent(100, 100);
        }
    }
}
