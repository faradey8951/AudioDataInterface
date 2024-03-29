﻿
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class form_main : Form
    {
        //Экземпляры классов
        //Префикс: "class_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static TextHandler class_TextHandler = new TextHandler();
        public static AudioIO class_audioIO = new AudioIO();
        public static DataHandler class_dataHandler = new DataHandler();
        public static form_debug window_debug = new form_debug();
        public static form_encoder window_encoder = new form_encoder();
        public static form_logMonitor window_logMonitor = new form_logMonitor();
        public static form_settings window_settings = new form_settings();
        //////////////////////////////////////////////////////////////////////////////////////

        public static form_main window_main;

        //Графика
        //Префикс: "graphics_; bitmap_"
        //////////////////////////////////////////////////////////////////////////////////////
        static Graphics graphics_waveGraph = null;
        static Bitmap bitmap_waveGraph = null;
        static Graphics graphics_mpsPlayerInterface = null;
        static Bitmap bitmap_mpsPlayerInterface = null;
        //////////////////////////////////////////////////////////////////////////////////////

        //Настройка осциллографа
        //Префикс: scope_
        //////////////////////////////////////////////////////////////////////////////////////
        public static int scope_horizontalScale = 1;
        public static int scope_additionalHorizontalScale = 1;
        public static int scope_verticalScale = 32767;
        public static int scope_verticalBIAS = 0;
        public static int scope_horizontalBIAS = 0;
        public static bool scope_horizontalBIASInc = false;
        public static bool scope_horizontalBIASDec = false;
        public static bool scope_verticalBIASInc = false;
        public static bool scope_verticalBIASDec = false;
        //////////////////////////////////////////////////////////////////////////////////////

        //MPS Player
        //Префикс: mpsPlayer_
        //////////////////////////////////////////////////////////////////////////////////////
        public static int[] mpsPlayer_instantSpectrum = { 9,9,9,9,9,9,9,9,9,9,9,9, 9 }; //Массив мгновенных уровней спектра [0-9]
        public static int[] mpsPlayer_liveSpectrum = { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 , 9}; //Массив динамических уровней спектра [0-9]
        public static int[] mpsPlayer_spectrumPeakHold = { 5,5,5,5,5,5,5,5,5,5,5,5 , 5}; //Массив пиков спектра [0-9]
        int[] mpsPlayer_spectrumFreq = { 68, 170, 420, 1000, 2400, 5900, 14400}; //Массив опорных частот, для которых строится спектр [Гц]
        int mpsPlayer_peakHoldTimeDelay = 20; //Задержка пиков спектра на дисплее, выраженная в количестве пропущенных кадров отрисовки дисплея из расчета FPS = 40
        int mpsPlayer_peakHoldTimeCount = 0; //Счетчик пропущенных кадров отрисовки дисплея
        public static bool mpsPlayer_showTime = true; //Указывает необходимость показа времени воспроизведения
        public static bool mpsPlayer_disc1Detected = false;
        public static bool mpsPlayer_disc2Detected = false;
        public static int[] mpsPlayer_time = { 0, 0, 0, 0 }; //Массив таймера воспроизведения
        public static int mpsPlayer_timeSeconds = 0; //Текущее время воспроизведения, выраженное в секундах
        public static int mpsPlayer_timeDurationSeconds = 0; //Длительность дорожки в секундах
        int mpsPlayer_timeUpdateDelay = 10; //Задержка обновления времени воспроизведения на дисплее, выраженная в количестве пропущенных кадров отрисовки дисплея из расчета FPS = 50
        int mpsPlayer_timeUpdateCount = 0; //Счетчик пропущенных кадров отрисовки дисплея
        public static int mpsPlayer_currentTrackNumber = -1; //Номер текущего проигрываемой дорожки
        public static int mpsPlayer_lastTrackNumber = -1; //Номер последней проигрываемой дорожки
        public static int mpsPlayer_trackCount = 16; //Количество дорожен
        public static string mpsPlayer_mode = ""; //Статус работы плеера
        public double[] mpsPlayer_RAWspectrum = null; //Массив необработанных мгновенных уровней спектра
        public static bool mpsPlayer_remainingTime = false; //Включает режим отображения оставшегося времени воспроизведения дорожки
        public static bool mpsPlayer_tapeSkin = false;
        public static int mpsPlayer_fftSize = 0;
        public static string mpsPlayer_spectrumMode = "";
        public static int mpsPlayer_spectrumVescosity = 0;
        public static int mpsPlayer_mp3Bitrate = 0;
        //////////////////////////////////////////////////////////////////////////////////////

        byte[] buffer_fftBytes = new byte[512];
        public RawSourceWaveStream rs = null;

        double[] bandIntensity = new double[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        string[] decodedData = null;

        public form_main()
        {
            InitializeComponent();
            window_main = this; //Передача статического доступа классу
            scope_verticalBIAS = pictureBox_waveGraph.Height / 2;
            //Type type = listView.GetType();
            //PropertyInfo propertyInfo = type.GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            //propertyInfo.SetValue(listView, true, null);
            decodedData = new string[10];
        }

        /// <summary>
        /// Отрисовка кадра сигналограммы
        /// </summary>
        public void DrawWaveGraphFrame()
        {
            if (this.WindowState != FormWindowState.Minimized && AudioIO.buff_graphSamples.Count > 0)
            {

                graphics_waveGraph.Clear(Color.FromArgb(34, 31, 31));
                int pointsCount = (pictureBox_waveGraph.Width / scope_horizontalScale) + 8;
                PointF[] points = new PointF[pointsCount]; //Массив точек кадра сигналограммы
                                                           //graphics_waveGraph.DrawLine(new Pen(Color.FromArgb(251,176,64)), 0, (pictureBox_waveGraph.Height / 2) + 1, pictureBox_waveGraph.Width, (pictureBox_waveGraph.Height / 2) + 1);
                for (int i = 0; i < pictureBox_waveGraph.Width; i += 23) graphics_waveGraph.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), i, 0, i, pictureBox_waveGraph.Height);
                for (int i = 0; i < pictureBox_waveGraph.Width; i += 23) graphics_waveGraph.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), i + 1, 0, i + 1, pictureBox_waveGraph.Height);
                for (int i = 0; i < pictureBox_waveGraph.Height; i += 23) graphics_waveGraph.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), 0, i, pictureBox_waveGraph.Width, i);
                for (int i = 0; i < pictureBox_waveGraph.Height; i += 23) graphics_waveGraph.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), 0, i + 1, pictureBox_waveGraph.Width, i + 1);
                graphics_waveGraph.DrawLine(new Pen(Color.FromArgb(251, 176, 64)), 0, (pictureBox_waveGraph.Height / 2) - 1, pictureBox_waveGraph.Width, (pictureBox_waveGraph.Height / 2) - 1);
                graphics_waveGraph.DrawLine(new Pen(Color.FromArgb(251, 176, 64)), 0, pictureBox_waveGraph.Height / 2, pictureBox_waveGraph.Width, pictureBox_waveGraph.Height / 2);
                for (int i = 0, k = scope_horizontalBIAS, x = 0; i < pointsCount; i += 1, k += scope_additionalHorizontalScale, x += scope_horizontalScale) points[i] = new PointF(x, (((pictureBox_waveGraph.Height / 2) * -AudioIO.buff_graphSamples[k]) / scope_verticalScale) + scope_verticalBIAS);
                graphics_waveGraph.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                points = new PointF[pointsCount];
                for (int i = 0, k = scope_horizontalBIAS, x = 0; i < pointsCount; i += 1, k += scope_additionalHorizontalScale, x += scope_horizontalScale) points[i] = new PointF(x, (((pictureBox_waveGraph.Height / 2) * -AudioIO.buff_graphSamples[k]) / scope_verticalScale) + scope_verticalBIAS - 1);
                graphics_waveGraph.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                pictureBox_waveGraph.Image = bitmap_waveGraph;
                AudioIO.buff_graphSamples.RemoveRange(0, AudioIO.buff_graphSamples.Count);
            }
        }

        /// <summary>
        /// Отрисовка интерфейса проигрывателя аудио потока ADI-MPS
        /// </summary>
        public void DrawMPSPlayerInterface()
        {
            double mpsPlayerWidth = 810;
            double mpsPlayerHeight = 335;
            double spectrumBarWidth = 0.65;
            double spectrumBarHeight = 0.35;
            int spectrumBarY0P = pictureBox_spectrumBorder1.Location.Y + pictureBox_spectrumBorder1.Height - 3;
            int spectrumBarX0P = pictureBox_spectrumBorder1.Location.X + pictureBox_spectrumBorder1.Width;
            int spectrumBarWidthP = (int)Math.Ceiling(mpsPlayerWidth * spectrumBarWidth);
            int spectrumBarHeightP = (int)Math.Ceiling((double)mpsPlayerHeight * (double)spectrumBarHeight);
            int spectrumBarSegmentWidthP = (int)Math.Ceiling((double)spectrumBarWidthP / 39.0);
            int spectrumBarSegmentHeightP = (int)Math.Floor((double)spectrumBarHeightP / 39.0);
            int spectrumBarSegmentDeltaP = (int)Math.Ceiling((double)spectrumBarSegmentWidthP / 12.0);
            int spectrumBarHeightGapReducerP = -1;

            graphics_mpsPlayerInterface.Clear(Color.FromArgb(34, 31, 31));

            //Отрисовка спектра
            for (int i = 1; i < mpsPlayer_liveSpectrum[0] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[1] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[2] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[3] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[4] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[5] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[6] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[7] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[8] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[9] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[10] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[11] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[12] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }

            //Отрисовка пиков
            for (int i = mpsPlayer_spectrumPeakHold[0] * 2; i >= ((mpsPlayer_spectrumPeakHold[0] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[0] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[1] * 2; i >= ((mpsPlayer_spectrumPeakHold[1] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[1] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[2] * 2; i >= ((mpsPlayer_spectrumPeakHold[2] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[2] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[3] * 2; i >= ((mpsPlayer_spectrumPeakHold[3] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[3] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[4] * 2; i >= ((mpsPlayer_spectrumPeakHold[4] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[4] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[5] * 2; i >= ((mpsPlayer_spectrumPeakHold[5] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[5] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[6] * 2; i >= ((mpsPlayer_spectrumPeakHold[6] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[6] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[7] * 2; i >= ((mpsPlayer_spectrumPeakHold[7] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[7] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[8] * 2; i >= ((mpsPlayer_spectrumPeakHold[8] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[8] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[9] * 2; i >= ((mpsPlayer_spectrumPeakHold[9] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[9] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[10] * 2; i >= ((mpsPlayer_spectrumPeakHold[10] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[10] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[11] * 2; i >= ((mpsPlayer_spectrumPeakHold[11] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[11] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[12] * 2; i >= ((mpsPlayer_spectrumPeakHold[12] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[12] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(205, 255, 191)), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(Color.FromArgb(255, 72, 30)), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            pictureBox_mpsPlayer.Image = bitmap_mpsPlayerInterface;
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            pictureBox_runningIndicator.Invalidate();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Settings.Load();

            bitmap_waveGraph = new Bitmap(pictureBox_waveGraph.Width, pictureBox_waveGraph.Height);
            graphics_waveGraph = Graphics.FromImage(bitmap_waveGraph); //Инициализация графики
            bitmap_mpsPlayerInterface = new Bitmap(pictureBox_mpsPlayer.Width, pictureBox_mpsPlayer.Height);
            graphics_mpsPlayerInterface = Graphics.FromImage(bitmap_mpsPlayerInterface);
            try
            {
                comboBox_recDevices.Items.AddRange(AudioIO.GetRecDevices());
                comboBox_recDevices.Text = AudioIO.GetRecDevices()[AudioIO.audio_recDeviceId];
                comboBox_playDevices.Items.AddRange(AudioIO.GetPlayDevices());
                comboBox_playDevices.Text = AudioIO.GetPlayDevices()[AudioIO.audio_playDeviceId];
            }
            catch
            {
                Properties.Settings.Default.Reset();
                Settings.Load();
            }
            AudioIO.GraphCaptureInit();
            Decoder.Start();
            MpsPlayerRunningIndicatorStop();         
        }

        private void button_capture_Click(object sender, EventArgs e)
        {
            if (window_debug != null)
            {
                window_debug.Dispose();
                window_debug = new form_debug();
            }
            window_debug.Show();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIO.GraphCaptureClose();
            AudioIO.SignalCaptureClose();
            if (window_debug != null)
                window_debug.Close();
            Settings.Save();
            //Close();
            Environment.Exit(0);
        }

        private void button_encoder_Click(object sender, EventArgs e)
        {
            if (window_encoder != null)
            {
                window_encoder.Dispose();
                window_encoder = new form_encoder();
            }
            window_encoder.ShowDialog();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            //Анимация запуска
            if (this.Opacity < 100) this.Opacity += 0.04;
            label_signalGainL.Text = "Усиление ЛК: " + Math.Round(AudioIO.audio_signalGainL, 2).ToString();
            label_signalGainR.Text = "Усиление ПК: " + Math.Round(AudioIO.audio_signalGainR, 2).ToString();
            label_fixedErrorCount.Text = "Исправлено: " + Decoder.fixedErrorCount.ToString();
            label_unfixedErrorCount.Text = "Неисправимые: " + Decoder.unfixedErrorCount.ToString();
            label_frameSyncErrorCount.Text = "Кадровая синхр.: " + Decoder.frameSyncErrorCount.ToString();
            label_signalQuality.Text = "Качество сигнала: " + Decoder.signalQuality.ToString() + "%";
            if (AudioIO.audio_autoSignalGain == true) checkBox_autoGain.Checked = true;
            else checkBox_autoGain.Checked = false;

            if (form_main.mpsPlayer_currentTrackNumber != form_main.mpsPlayer_lastTrackNumber)
            {
                Decoder.unfixedErrorCount = 0;
                Decoder.fixedErrorCount = 0;
                Decoder.frameSyncErrorCount = 0;
                form_main.mpsPlayer_lastTrackNumber = form_main.mpsPlayer_currentTrackNumber;
            }

            if (scope_horizontalBIASInc == true) scope_horizontalBIAS -= 1;
            if (scope_horizontalBIASDec == true) scope_horizontalBIAS += 1;
            if (scope_verticalBIASInc == true) scope_verticalBIAS += 2;
            if (scope_verticalBIASDec == true) scope_verticalBIAS -= 2;
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /*
        async void ListViewUpdate()
        {
            while (true)
            {
                if (Decoder.buff_decodedData.Count() > 1)
                {
                    Decoder.buff_decodedData[0].CopyTo(decodedData, 0);
                    listView.Invoke((MethodInvoker)(() => listView.Items.Add(listView.Items.Count.ToString())));
                    listView.Invoke((MethodInvoker)(() => listView.Items[listView.Items.Count - 1].SubItems.Add(decodedData[8])));
                    listView.Invoke((MethodInvoker)(() => listView.Items[listView.Items.Count - 1].SubItems.Add("-")));
                    listView.Invoke((MethodInvoker)(() => listView.Items[listView.Items.Count - 1].SubItems.Add("-")));
                    listView.Invoke((MethodInvoker)(() => listView.Items[listView.Items.Count - 1].SubItems.Add(decodedData[9])));
                    listView.Invoke((MethodInvoker)(() => listView.Items[listView.Items.Count - 1].EnsureVisible()));
                    Decoder.buff_decodedData.RemoveAt(0);
                }
                else
                    Thread.Sleep(10);
            }
        }
        async void TaskListViewUpdate()
        {
            await Task.Run(() => ListViewUpdate());
        }
        */

        private void toolStripLabel_Click(object sender, EventArgs e)
        {
            //TaskListViewUpdate();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void form_main_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void timer_drawWaveGraphFrame_Tick(object sender, EventArgs e)
        {
            DrawWaveGraphFrame();
        }

        private void comboBox_recDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioIO.audio_recDeviceId = comboBox_recDevices.SelectedIndex;
            AudioIO.GraphCaptureInit();
            AudioIO.SignalCaptureInit();
        }

        private void form_main_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                bitmap_waveGraph = new Bitmap(pictureBox_waveGraph.Width, pictureBox_waveGraph.Height);
                graphics_waveGraph = Graphics.FromImage(bitmap_waveGraph); //Инициализация графики
                bitmap_mpsPlayerInterface = new Bitmap(pictureBox_mpsPlayer.Width, pictureBox_mpsPlayer.Height);
                graphics_mpsPlayerInterface = Graphics.FromImage(bitmap_mpsPlayerInterface);
            }
        }

        private void pictureBox_waveGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            if (radioButton_horizontalScale.Checked)
            {
                if (e.Delta > 0 && scope_additionalHorizontalScale == 1) scope_horizontalScale += 1;
                else if (e.Delta > 0 && scope_additionalHorizontalScale != 1) scope_additionalHorizontalScale -= 1;
                else if (scope_horizontalScale > 1 && e.Delta != 0) scope_horizontalScale -= 1;
                else if (e.Delta != 0) scope_additionalHorizontalScale += 1;
            }
            else
            {
                if (e.Delta < 0) scope_verticalScale += 512;
                else if (scope_verticalScale > 1 && e.Delta != 0) scope_verticalScale -= 512;
            }
        }

        private void pictureBox_waveGraph_MouseEnter(object sender, EventArgs e)
        {
            pictureBox_waveGraph.MouseWheel += new MouseEventHandler(pictureBox_waveGraph_MouseWheel);
        }

        private void pictureBox_waveGraph_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_waveGraph.MouseWheel -= new MouseEventHandler(pictureBox_waveGraph_MouseWheel);
        }

        private void pictureBox_waveGraph_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && radioButton_horizontalBIAS.Checked) scope_horizontalBIASInc = true;
            if (e.Button == MouseButtons.Left && radioButton_horizontalBIAS.Checked) scope_horizontalBIASDec = true;
            if (e.Button == MouseButtons.Right && radioButton_verticalBIAS.Checked) scope_verticalBIASInc = true;
            if (e.Button == MouseButtons.Left && radioButton_verticalBIAS.Checked) scope_verticalBIASDec = true;
        }

        private void pictureBox_waveGraph_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && radioButton_horizontalBIAS.Checked) scope_horizontalBIASInc = false;
            if (e.Button == MouseButtons.Left && radioButton_horizontalBIAS.Checked) scope_horizontalBIASDec = false;
            if (e.Button == MouseButtons.Right && radioButton_verticalBIAS.Checked) scope_verticalBIASInc = false;
            if (e.Button == MouseButtons.Left && radioButton_verticalBIAS.Checked) scope_verticalBIASDec = false;
        }

        private void pictureBox_waveGraph_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) { scope_horizontalBIAS = 0; scope_horizontalScale = 1; scope_verticalScale = 32767; scope_verticalBIAS = pictureBox_waveGraph.Height / 2; scope_additionalHorizontalScale = 1; }
        }

        private void button_buffMp3_Click(object sender, EventArgs e)
        {
            AudioIO.SignalCaptureInit();

            mpsPlayer_liveSpectrum = new int[] { 6, 5, 3, 1, 2, 1, 3, 4, 3, 2, 3, 5, 6 };
            DataHandler.StartMp3Listening();
            AudioIO.MPSAudioOutputCaptureInit();
            //DrawMPSPlayerInterface();
            MpsPlayerTrackCalendarSetAmount(16);
            MpsPlayerTrackCalendarSetCurrentTrack(1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //window_debug.Show();

        }

        public void MpsPlayerRunningIndicatorPlay()
        {
            if (mpsPlayer_tapeSkin == false)
            {
                window_main.pictureBox_runningIndicator.Image = Properties.Resources.CD_Playback_Transparrent;
                window_main.pictureBox_playPause.Image = Properties.Resources.play;
            }
            else
            {
                if (mpsPlayer_remainingTime == false)
                {
                    window_main.pictureBox_runningIndicator.Image = Properties.Resources.TAPE_FWD_Playback_Transparrent;
                }
                else
                {
                    window_main.pictureBox_runningIndicator.Image = Properties.Resources.TAPE_RVS_Playback_Transparrent;
                }
            }
        }

        public static void MpsPlayerRunningIndicatorSeek()
        {
            if (mpsPlayer_tapeSkin == false)
            {
                window_main.pictureBox_runningIndicator.Image = Properties.Resources.CD_Skip_Transparrent;
                window_main.pictureBox_playPause.Image = Properties.Resources.pause;
            }
            else
            {
                window_main.pictureBox_runningIndicator.Image = Properties.Resources.Running_Indicator;
            }
        }

        public static void MpsPlayerRunningIndicatorStop()
        {
            window_main.pictureBox_runningIndicator.Image = Properties.Resources.Running_Indicator;
            window_main.pictureBox_playPause.Image = null;
        }

        public static void MpsPlayerTrackCalendarSetAmount(int trackCount)
        {
            if (trackCount > 16) trackCount = 16;
            PictureBox[] pictureBox_trackNumber = { window_main.pictureBox_track1, window_main.pictureBox_track2, window_main.pictureBox_track3, window_main.pictureBox_track4, window_main.pictureBox_track5, window_main.pictureBox_track6, window_main.pictureBox_track7, window_main.pictureBox_track8, window_main.pictureBox_track9, window_main.pictureBox_track10, window_main.pictureBox_track11, window_main.pictureBox_track12, window_main.pictureBox_track13, window_main.pictureBox_track14, window_main.pictureBox_track15, window_main.pictureBox_track16 };

            for (int i = 0; i < 16; i++)
            {

                if (i < trackCount) pictureBox_trackNumber[i].Visible = true;
                else pictureBox_trackNumber[i].Visible = false;

            }
        }

        public static void MpsPlayerTrackCalendarSetCurrentTrack(int currentTrackNum)
        {
            try
            {
                PictureBox[] pictureBox_trackNumber = { window_main.pictureBox_track1, window_main.pictureBox_track2, window_main.pictureBox_track3, window_main.pictureBox_track4, window_main.pictureBox_track5, window_main.pictureBox_track6, window_main.pictureBox_track7, window_main.pictureBox_track8, window_main.pictureBox_track9, window_main.pictureBox_track10, window_main.pictureBox_track11, window_main.pictureBox_track12, window_main.pictureBox_track13, window_main.pictureBox_track14, window_main.pictureBox_track15, window_main.pictureBox_track16 };

                for (int i = 0; i < currentTrackNum - 1; i++) pictureBox_trackNumber[i].Visible = false;
            }
            catch
            {

            }
        }

        private void timer_mpsPlayerHandler_Tick(object sender, EventArgs e)
        {
            if (mpsPlayer_spectrumMode != "off") DrawMPSPlayerInterface();           
            mpsPlayer_timeUpdateCount++;
            if (mpsPlayer_timeUpdateCount == mpsPlayer_timeUpdateDelay)
            {               
                window_main.pictureBox_dots.Image = Properties.Resources.DOTS;
                Bitmap[] symbolImages = { Properties.Resources._0symbol, Properties.Resources._1symbol, Properties.Resources._2symbol, Properties.Resources._3symbol, Properties.Resources._4symbol, Properties.Resources._5symbol, Properties.Resources._6symbol, Properties.Resources._7symbol, Properties.Resources._8symbol, Properties.Resources._9symbol };
                PictureBox[] pictureBox_timeSymbols = { window_main.pictureBox_symbol7, window_main.pictureBox_symbol8, window_main.pictureBox_symbol9, window_main.pictureBox_symbol10 };
                PictureBox[] pictureBox_trackNumberSymbols = { window_main.pictureBox_symbol4, window_main.pictureBox_symbol5 };
                mpsPlayer_timeUpdateCount = 0;
                if (mpsPlayer_tapeSkin == false)
                {
                    pictureBox_playPause.Visible = true;
                    pictureBox_symbol1.Image = Properties.Resources.Msymbol;
                    pictureBox_symbol2.Image = Properties.Resources.Psymbol;
                    pictureBox_symbol3.Image = null;
                    pictureBox_symbol4.Image = Properties.Resources.DASHsymbol;
                    pictureBox_symbol5.Image = Properties.Resources.DASHsymbol;
                    pictureBox_symbol7.Image = null;
                    pictureBox_dots.Visible = true;
                    pictureBox_disc1.Visible = true;
                    pictureBox_disc2.Visible = true;
                    pictureBox_disc3.Visible = true;
                    pictureBox_cassette.Image = null;
                    //Отображение текущего времени воспроизведения
                    if (mpsPlayer_remainingTime == false)
                    {
                        pictureBox_symbol6.Image = null;
                        pictureBox_symbol7.Image = null;
                        mpsPlayer_time = Decoder.GetTimeFromSeconds(mpsPlayer_timeSeconds);
                    }
                    else
                    {
                        if (mpsPlayer_timeDurationSeconds - mpsPlayer_timeSeconds >= 600) pictureBox_symbol6.Image = Properties.Resources.DASHsymbol;
                        else
                        {
                            pictureBox_symbol6.Image = null;
                            pictureBox_symbol7.Image = Properties.Resources.DASHsymbol;
                        }
                        mpsPlayer_time = Decoder.GetTimeFromSeconds(mpsPlayer_timeDurationSeconds - mpsPlayer_timeSeconds);
                    }
                    if (mpsPlayer_showTime == true)
                    {
                        for (int i = 0; i < mpsPlayer_time.Length; i++) if (mpsPlayer_time[i] != 0 || i > 0) pictureBox_timeSymbols[i].Image = symbolImages[mpsPlayer_time[i]];
                    }
                    else
                    {
                        pictureBox_symbol7.Image = null;
                        window_main.pictureBox_dots.Image = null;
                        for (int k = 0; k < pictureBox_timeSymbols.Length; k++) pictureBox_timeSymbols[k].Image = null;
                    }
                    MpsPlayerTrackCalendarSetAmount(mpsPlayer_trackCount);
                    MpsPlayerTrackCalendarSetCurrentTrack(mpsPlayer_currentTrackNumber);
                    //Отображение текущей проигрываемой дорожки
                    string currentTrackNumber = "";
                    if (mpsPlayer_currentTrackNumber != -1 && mpsPlayer_currentTrackNumber <= 99)
                    {
                        if (mpsPlayer_currentTrackNumber < 10) currentTrackNumber += "0";
                        currentTrackNumber += mpsPlayer_currentTrackNumber.ToString();
                        for (int i = 0; i < currentTrackNumber.Length; i++) pictureBox_trackNumberSymbols[i].Image = symbolImages[Convert.ToInt16(currentTrackNumber[i].ToString())];
                    }
                    else
                    {
                        pictureBox_trackNumberSymbols[0].Image = Properties.Resources.DASHsymbol;
                        pictureBox_trackNumberSymbols[1].Image = Properties.Resources.DASHsymbol;
                    }
                    if (mpsPlayer_disc1Detected == true) pictureBox_disc1.Image = Properties.Resources.disc1Detected;
                    else pictureBox_disc1.Image = Properties.Resources.disc1Selected;
                }
                else
                {
                    MpsPlayerTrackCalendarSetAmount(0);
                    pictureBox_playPause.Visible = false;
                    pictureBox_symbol1.Image = Properties.Resources.Tsymbol;
                    pictureBox_symbol2.Image = Properties.Resources.Asymbol;
                    pictureBox_symbol3.Image = Properties.Resources.Psymbol;
                    pictureBox_symbol4.Image = Properties.Resources.Esymbol;
                    pictureBox_symbol5.Image = null;
                    pictureBox_symbol7.Image = Properties.Resources._0symbol;
                    pictureBox_dots.Visible = false;
                    pictureBox_disc1.Visible = false;
                    pictureBox_disc2.Visible = false;
                    pictureBox_disc3.Visible = false;
                    if (mpsPlayer_disc1Detected == true) pictureBox_cassette.Image = Properties.Resources.cassette;
                    else pictureBox_cassette.Image = null;
                    mpsPlayer_time = new int[4];
                    int currentTapeTime = 0;
                    if (mpsPlayer_remainingTime == false) currentTapeTime = mpsPlayer_timeSeconds;
                    else currentTapeTime = mpsPlayer_timeDurationSeconds - mpsPlayer_timeSeconds;
                    if (currentTapeTime < 0) currentTapeTime = 0;
                    for (int i = 3, k = currentTapeTime.ToString().Length - 1; i >= 0; i--)
                    {
                        if (k >= 0)
                        {
                            mpsPlayer_time[i] = Convert.ToInt16(currentTapeTime.ToString()[k].ToString());
                            k--;
                        }
                    }
                    for (int i = 0; i < mpsPlayer_time.Length; i++) if (mpsPlayer_time[i] != 0 || i > 0) pictureBox_timeSymbols[i].Image = symbolImages[mpsPlayer_time[i]];
                }
            }
        }

        private void timer_mpsPlayerSpectrumHandler_Tick(object sender, EventArgs e)
        {
            if (mpsPlayer_peakHoldTimeCount == mpsPlayer_peakHoldTimeDelay)
            {
                mpsPlayer_peakHoldTimeCount = 0;
                mpsPlayer_spectrumPeakHold = new int[]{ 0,0,0,0,0,0,0,0,0,0,0,0,0};
            }
            for (int i = 0; i < mpsPlayer_instantSpectrum.Length; i++)
            {
                if (mpsPlayer_instantSpectrum[i] > mpsPlayer_liveSpectrum[i] && mpsPlayer_liveSpectrum[i] < 9) mpsPlayer_liveSpectrum[i]++;
                else if (mpsPlayer_liveSpectrum[i] > 0) mpsPlayer_liveSpectrum[i]--;
            }
            for (int i = 0; i < mpsPlayer_liveSpectrum.Length; i++)
            {
                if (mpsPlayer_spectrumPeakHold[i] < mpsPlayer_liveSpectrum[i]) mpsPlayer_spectrumPeakHold[i] = mpsPlayer_liveSpectrum[i];
            }
            mpsPlayer_peakHoldTimeCount++;
            if (mpsPlayer_spectrumMode == "noPeak") mpsPlayer_spectrumPeakHold = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        private void timer_mpsPlayerSpectrumUpdater_Tick(object sender, EventArgs e)
        {
            if (AudioIO.buff_fftValues != null)
            {
                double[] paddedAudio = FftSharp.Pad.ZeroPad(AudioIO.buff_fftSamples);
                System.Numerics.Complex[] complex = FftSharp.FFT.Forward(paddedAudio);
                double[] fftMag = FftSharp.FFT.Magnitude(complex);
                double[] frequencyValues = FftSharp.FFT.FrequencyScale(fftMag.Length, AudioIO.waveLoop.WaveFormat.SampleRate);
                Array.Copy(fftMag, AudioIO.buff_fftValues, fftMag.Length);
                double[] RAWspectrumSelection = new double[mpsPlayer_instantSpectrum.Length];
                double[] RAWspectrumSelectionKenwood = new double[mpsPlayer_spectrumFreq.Length];

                //Выборка заданных частот
                for (int i = 0, k = 0; i < AudioIO.buff_fftValues.Length && k < mpsPlayer_spectrumFreq.Length; )
                {
                    if (Math.Round(frequencyValues[i]) >= mpsPlayer_spectrumFreq[k])
                    {
                        RAWspectrumSelectionKenwood[k] = AudioIO.buff_fftValues[i];
                        k++;
                        i = 0;
                    }
                    else i++;
                }
                //Интерполяция промежуточных значений
                for (int i = 0, k = 0, j = 1; (i < RAWspectrumSelectionKenwood.Length); i++, k+=2, j+=2)
                {
                    RAWspectrumSelection[k] = RAWspectrumSelectionKenwood[i];
                    if (j < RAWspectrumSelection.Length) RAWspectrumSelection[j] = 0.5*(RAWspectrumSelectionKenwood[i] + RAWspectrumSelectionKenwood[i + 1]);
                }

                //Преобразование уровня спектра к шкале 0-9
                for (int i = 0; i < mpsPlayer_instantSpectrum.Length; i++)
                {   if (i == 0) RAWspectrumSelection[i] = 0.8 * RAWspectrumSelection[i];
                    if (i > 3) RAWspectrumSelection[i] = 4 *  Math.Log10(i) * RAWspectrumSelection[i]; //Фильтр АЧХ
                    if (12 - i > 3) RAWspectrumSelection[12 - i] = 4 * Math.Log10(12 - i) * RAWspectrumSelection[12 - i]; //Фильтр АЧХ
                    if (RAWspectrumSelection[i] > 3000) RAWspectrumSelection[i] = 3000;
                    mpsPlayer_instantSpectrum[i] = (int)Math.Floor((9 * RAWspectrumSelection[i]) / 3000.0);
                }
            }
            else
            {
                for (int i = 0; i < mpsPlayer_instantSpectrum.Length; i++) mpsPlayer_instantSpectrum[i] = 0;
            }
        }

        private void pictureBox_mpsPlayer_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void timer_mpsPlayerTimeUpdate_Tick(object sender, EventArgs e)
        {
            if (mpsPlayer_mode == "play") mpsPlayer_timeSeconds++;
        }

        private void checkBox_invertSignal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_invertSignal.Checked) AudioIO.audio_invertSignal = true;
            else AudioIO.audio_invertSignal = false;
        }

        private void checkBox_remainingTime_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_remainingTime.Checked) mpsPlayer_remainingTime = true;
            else mpsPlayer_remainingTime = false;
            if (form_main.mpsPlayer_mode == "play" && mpsPlayer_tapeSkin == true) MpsPlayerRunningIndicatorPlay();
        }

        private void checkBox_autoGain_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_autoGain.Checked) AudioIO.audio_autoSignalGain = true;
            else AudioIO.audio_autoSignalGain = false;
        }

        private void checkBox_tapeSkin_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_tapeSkin.Checked)
            {
                mpsPlayer_tapeSkin = true;
                if (form_main.mpsPlayer_mode == "play") MpsPlayerRunningIndicatorPlay();
                else form_main.MpsPlayerRunningIndicatorStop();
            }
            else
            {
                mpsPlayer_tapeSkin = false;
                mpsPlayer_liveSpectrum = new int[] { 6, 5, 3, 1, 2, 1, 3, 4, 3, 2, 3, 5, 6 };
                if (form_main.mpsPlayer_mode == "play") MpsPlayerRunningIndicatorPlay();
            }

        }

        private void groupBox_signalCapture_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox_playDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioIO.audio_playDeviceId = comboBox_playDevices.SelectedIndex;
            AudioIO.MPSAudioOutputCaptureInit();
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            if (window_settings != null)
            {
                window_settings.Dispose();
                window_settings = new form_settings();
            }
            window_settings.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string[] test = BinaryHandler.HammingDecode("110000010000111111111000000001101111110");
                string[] test2 = BinaryHandler.HammingEncode("11111111111111111111111111111111");
                string test3 = "";
                foreach (string s in test2) test3 += s;
                groupBox_info.BackgroundImage = Image.FromStream(DataHandler.ms);
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImageAnimator.Animate(pictureBox_runningIndicator.BackgroundImage, OnFrameChanged);
            Decoder.buff_decodedData.Clear();
            Decoder.buff_signalAmplitudesL.Clear();
            Decoder.buff_signalAmplitudesR.Clear();
            DataHandler.ms = null;
            DataHandler.ms = new MemoryStream();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                tabPage1.BackgroundImage = Image.FromStream(DataHandler.ms);
                this.Text = "ok";
            }
            catch (Exception ex)
            {
                this.Text = ex.Message;
            }
        }

        private void timer_signalQualityUpdater_Tick(object sender, EventArgs e)
        {
            double currentOverallErrorCount = Decoder.overallErrorCount;
            double currentOverallBlockCount = Decoder.overallBlockCount;
            if (currentOverallBlockCount > 0) Decoder.signalQuality = (int)(100 - (100.0 * currentOverallErrorCount / currentOverallBlockCount));
            if (Decoder.signalQuality < 0) this.Name = "";
            Decoder.overallErrorCount = 0;
            Decoder.overallBlockCount = 0;
            if (Decoder.signalQuality >= 70)
            {
                if (form_main.mpsPlayer_mode != "play") { MpsPlayerRunningIndicatorPlay(); Decoder.fixedErrorCount = 0; Decoder.frameSyncErrorCount = 0; Decoder.unfixedErrorCount = 0; }
                form_main.mpsPlayer_mode = "play";
                form_main.mpsPlayer_disc1Detected = true;
            }
            else
            {
                if (form_main.mpsPlayer_mode != "seek") form_main.MpsPlayerRunningIndicatorSeek();
                form_main.mpsPlayer_mode = "seek";
                form_main.mpsPlayer_disc1Detected = false;
                Decoder.fixedErrorCount = 0;
                Decoder.frameSyncErrorCount = 0;
                Decoder.unfixedErrorCount = 0;
            }
        }
    }
}
