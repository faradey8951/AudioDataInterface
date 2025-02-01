
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;
using OpusDotNet;
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
        public static mpsPlayerSkinHandler class_mpsPlayerSkinHandler = new mpsPlayerSkinHandler();
        public static form_encoder window_encoder = new form_encoder();
        public static form_tapeRecordingWizard window_tapeRecordingWizard = new form_tapeRecordingWizard();
        public static form_tapeRecoverWizard window_tapeRecoverWizard = new form_tapeRecoverWizard();
        public static form_settings window_settings = new form_settings();
        public static form_debug window_debug = new form_debug();
        //////////////////////////////////////////////////////////////////////////////////////

        public static form_main window_main;

        //Графика
        //Префикс: "graphics_; bitmap_"
        //////////////////////////////////////////////////////////////////////////////////////
        static Graphics graphics_waveGraphL = null; //Графика осциллографа
        static Graphics graphics_waveGraphR = null;
        static Bitmap bitmap_waveGraphL = null; //Графика осциллографа
        static Bitmap bitmap_waveGraphR = null;
        static Graphics graphics_mpsPlayerInterface = null; //Графика mps плеера
        static Bitmap bitmap_mpsPlayerInterface = null; //Графика mps плеера
        //Индикаторы состояния аудиопроцессора
        static Graphics graphics_subcodeSyncIndicator = null;
        static Graphics graphics_subcodeTimecodeIndicator = null;
        static Graphics graphics_subcodeTOCIndicator = null;
        static Graphics graphics_audioInterpolationIndicator = null;
        static Graphics graphics_audioMutingIndicator = null;
        static Graphics graphics_packetLossIndicator = null;
        static Bitmap bitmap_subcodeSyncIndicator = null;
        static Bitmap bitmap_subcodeTimecodeIndicator = null;
        static Bitmap bitmap_subcodeTOCIndicator = null;
        static Bitmap bitmap_audioInterpolationIndicator = null;
        static Bitmap bitmap_audioMutingIndicator = null;
        static Bitmap bitmap_packetLossIndicator = null;
        //Счетчики задержки отображения индикаторов состояния аудиопроцессора
        static int subcodeSyncIndicatorCount = 0;
        static int subcodeSyncErrorIndicatorCount = 0;
        static int subcodeTimecodeIndicatorCount = 0;
        static int subcodeTOCIndicatorCount = 0;
        static int audioInterpolationIndicatorCount = 0;
        static int audioMutingIndicatorCount = 0;
        static int packetLossIndicatorCount  = 0;

        Image[] symbolImages; //Изображения отображаемых символов mps плеера
        PictureBox[] pictureBox_timeSymbols; 
        PictureBox[] pictureBox_trackNumberSymbols;
        PictureBox[] pictureBox_textSymbols;
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
        public static int mpsPlayer_peakHoldTimeDelay = 0; //Задержка итераций отрисовки пиков спектра
        int mpsPlayer_peakHoldTimeCount = 0; //Счетчик пропущенных итераций отрисовки пиков
        public static bool mpsPlayer_showTime = true; //Указывает необходимость показа времени воспроизведения
        public static bool mpsPlayer_disc1Detected = false; //Показывает индикацию обнаружения сигнала
        public static int[] mpsPlayer_time = { 0, 0, 0, 0 }; //Массив таймера воспроизведения
        public static int mpsPlayer_timeSeconds = 0; //Текущее время воспроизведения, выраженное в секундах
        public static int mpsPlayer_timeDurationSeconds = 0; //Длительность дорожки в секундах
        public static int mpsPlayer_currentTrackNumber = -1; //Номер текущего проигрываемой дорожки
        public static int mpsPlayer_lastTrackNumber = -1; //Номер последней проигрываемой дорожки
        public static int mpsPlayer_trackCount = 16; //Количество дорожен
        public static string mpsPlayer_mode = ""; //Режим работы плеера
        public double[] mpsPlayer_RAWspectrum = null; //Массив необработанных мгновенных уровней спектра
        public static bool mpsPlayer_remainingTime = false; //Включает режим отображения оставшегося времени воспроизведения дорожки
        public static bool mpsPlayer_tapeSkin = false; //Переключает суб-скин на режим проигрывания ленты
        public static int mpsPlayer_fftSize = 0; //Размер буфера сэмплов FFT для отрисовки спектра
        public static string mpsPlayer_spectrumMode = ""; //Режим отображения спектра
        public static int mpsPlayer_runningIndicatorAnimationFrameIndex = 0; //Текущий индекс кадра анимации бегущего индикатора
        public static double mpsPlayerWidth = 0; //Ширина mps плеера
        public static double mpsPlayerHeight = 0; //Высота mps плеера
        public static bool mpsPlayer_skinEdit = false; //Управляет вкл/выкл режима редактирования скина
        public static bool mpsPlayer_controlFree = false; //Управляет режимом свободного перемещения элементов
        public static bool mpsPlayer_allControl = false; //Управляет режимом выбора всех элементов сразу
        public static object mpsPlayer_control = null; //Ссылается на перемещаемый мышкой элемент
        public static object mpsPlayer_selectedControl = null; //Ссылается на последний выбранный элемент по ЛКМ/ПКМ
        public static object mpsPlayer_alignmentControl = null; //Ссылается на элемент, выбранный в качестве ориентира для выравнивания
        public static char[] mpsPlayer_MPSTextMatrix = { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
        public static string mpsPlayer_MPSTextLastArtist = "                        ";
        public static string mpsPlayer_MPSTextLastTrack = "                        ";
        public static int mpsPlayer_MPSTextHoldTimeDelay = 0;
        public static bool mpsPlayer_MPSTextChange = false;
        public static int mpsPlayer_MPSTextChangeCount = 0;

        public static double spectrumBarWidth = 0; //Ширина области спектра относительно ширины mps плеера
        public static double spectrumBarHeight = 0; //Высота области спектра относительно высоты mps плеера
        public static int spectrumBarY0P;
        public static int spectrumBarX0P;
        public static int spectrumBarWidthP;
        public static int spectrumBarHeightP;
        public static int spectrumBarSegmentWidthP;
        public static int spectrumBarSegmentHeightP;
        public static int spectrumBarSegmentDeltaP;
        public static int spectrumBarHeightGapReducerP;
        public static double spectrumBarSegmentWidthCount;
        public static double spectrumBarSegmentHeightCount;
        public static double spectrumBarSegmentDeltaCount;
        //////////////////////////////////////////////////////////////////////////////////////

        public form_main()
        {
            InitializeComponent();
            window_main = this; //Передача статического доступа классу
            scope_verticalBIAS = pictureBox_waveGraphL.Height / 2;
        }

        /// <summary>
        /// Отрисовка кадра сигналограммы
        /// </summary>
        public void DrawWaveGraphFrame()
        {
            if (this.WindowState != FormWindowState.Minimized && AudioIO.buff_graphSamples.Count > 0)
            {

                graphics_waveGraphL.Clear(Color.FromArgb(34, 31, 31));
                graphics_waveGraphR.Clear(Color.FromArgb(34, 31, 31));
                int pointsCount = (pictureBox_waveGraphL.Width / scope_horizontalScale) + 8;
                PointF[] points = new PointF[pointsCount]; //Массив точек кадра сигналограммы
                //Отрисовка координатной сетки левого канала
                for (int i = 0; i < pictureBox_waveGraphL.Width; i += 12) graphics_waveGraphL.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), i, 0, i, pictureBox_waveGraphL.Height);
                for (int i = 0; i < pictureBox_waveGraphL.Width; i += 12) graphics_waveGraphL.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), i + 1, 0, i + 1, pictureBox_waveGraphL.Height);
                for (int i = 0; i < pictureBox_waveGraphL.Height; i += 12) graphics_waveGraphL.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), 0, i, pictureBox_waveGraphL.Width, i);
                for (int i = 0; i < pictureBox_waveGraphL.Height; i += 12) graphics_waveGraphL.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), 0, i + 1, pictureBox_waveGraphL.Width, i + 1);
                graphics_waveGraphL.DrawLine(new Pen(Color.FromArgb(251, 176, 64)), 0, (pictureBox_waveGraphL.Height / 2) - 1, pictureBox_waveGraphL.Width, (pictureBox_waveGraphL.Height / 2) - 1);
                graphics_waveGraphL.DrawLine(new Pen(Color.FromArgb(251, 176, 64)), 0, pictureBox_waveGraphL.Height / 2, pictureBox_waveGraphL.Width, pictureBox_waveGraphL.Height / 2);
                //Отрисовка координатной сетки правого канала
                for (int i = 0; i < pictureBox_waveGraphR.Width; i += 12) graphics_waveGraphR.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), i, 0, i, pictureBox_waveGraphR.Height);
                for (int i = 0; i < pictureBox_waveGraphR.Width; i += 12) graphics_waveGraphR.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), i + 1, 0, i + 1, pictureBox_waveGraphR.Height);
                for (int i = 0; i < pictureBox_waveGraphR.Height; i += 12) graphics_waveGraphR.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), 0, i, pictureBox_waveGraphR.Width, i);
                for (int i = 0; i < pictureBox_waveGraphR.Height; i += 12) graphics_waveGraphR.DrawLine(new Pen(Color.FromArgb(132, 96, 46)), 0, i + 1, pictureBox_waveGraphR.Width, i + 1);
                graphics_waveGraphR.DrawLine(new Pen(Color.FromArgb(251, 176, 64)), 0, (pictureBox_waveGraphR.Height / 2) - 1, pictureBox_waveGraphR.Width, (pictureBox_waveGraphR.Height / 2) - 1);
                graphics_waveGraphR.DrawLine(new Pen(Color.FromArgb(251, 176, 64)), 0, pictureBox_waveGraphR.Height / 2, pictureBox_waveGraphR.Width, pictureBox_waveGraphR.Height / 2);
                //Отрисовка осциллограммы
                for (int i = 0, k = scope_horizontalBIAS, x = 0; i < pointsCount; i += 1, k += 2, x += scope_horizontalScale) points[i] = new PointF(x, (((pictureBox_waveGraphL.Height / 2) * -AudioIO.buff_graphSamples[k]) / scope_verticalScale) + scope_verticalBIAS);
                graphics_waveGraphL.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                points = new PointF[pointsCount];
                for (int i = 0, k = scope_horizontalBIAS + 1, x = 0; i < pointsCount; i += 1, k += 2, x += scope_horizontalScale) points[i] = new PointF(x, (((pictureBox_waveGraphR.Height / 2) * -AudioIO.buff_graphSamples[k]) / scope_verticalScale) + scope_verticalBIAS);
                graphics_waveGraphR.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                points = new PointF[pointsCount];
                for (int i = 0, k = scope_horizontalBIAS, x = 0; i < pointsCount; i += 1, k += 2, x += scope_horizontalScale) points[i] = new PointF(x, (((pictureBox_waveGraphL.Height / 2) * -AudioIO.buff_graphSamples[k]) / scope_verticalScale) + scope_verticalBIAS - 1);
                graphics_waveGraphL.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                points = new PointF[pointsCount];
                for (int i = 0, k = scope_horizontalBIAS + 1, x = 0; i < pointsCount; i += 1, k += 2, x += scope_horizontalScale) points[i] = new PointF(x, (((pictureBox_waveGraphR.Height / 2) * -AudioIO.buff_graphSamples[k]) / scope_verticalScale) + scope_verticalBIAS - 1);
                graphics_waveGraphR.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                pictureBox_waveGraphL.Image = bitmap_waveGraphL;
                pictureBox_waveGraphR.Image = bitmap_waveGraphR;
                AudioIO.buff_graphSamples.RemoveRange(0, AudioIO.buff_graphSamples.Count);
            }
        }

        /// <summary>
        /// Отрисовка интерфейса проигрывателя аудио потока ADI-MPS
        /// </summary>
        public void DrawMPSPlayerInterface()
        {
            graphics_mpsPlayerInterface.Clear(class_mpsPlayerSkinHandler.color_playerBackColor);

            //Отрисовка спектра
            for (int i = 1; i < mpsPlayer_liveSpectrum[0] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[1] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[2] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[3] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[4] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[5] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[6] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[7] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[8] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[9] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[10] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[11] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = 1; i < mpsPlayer_liveSpectrum[12] * 2; i++)
            {
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumBaseColor), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumHighColor), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }

            //Отрисовка пиков
            for (int i = mpsPlayer_spectrumPeakHold[0] * 2; i >= ((mpsPlayer_spectrumPeakHold[0] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[0] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + spectrumBarSegmentWidthP + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[1] * 2; i >= ((mpsPlayer_spectrumPeakHold[1] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[1] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (3 * spectrumBarSegmentWidthP) + spectrumBarSegmentDeltaP, spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (4 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[2] * 2; i >= ((mpsPlayer_spectrumPeakHold[2] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[2] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (6 * spectrumBarSegmentWidthP) + (2 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (7 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[3] * 2; i >= ((mpsPlayer_spectrumPeakHold[3] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[3] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (9 * spectrumBarSegmentWidthP) + (3 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (10 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[4] * 2; i >= ((mpsPlayer_spectrumPeakHold[4] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[4] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (12 * spectrumBarSegmentWidthP) + (4 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (13 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[5] * 2; i >= ((mpsPlayer_spectrumPeakHold[5] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[5] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (15 * spectrumBarSegmentWidthP) + (5 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (16 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[6] * 2; i >= ((mpsPlayer_spectrumPeakHold[6] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[6] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (18 * spectrumBarSegmentWidthP) + (6 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (19 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[7] * 2; i >= ((mpsPlayer_spectrumPeakHold[7] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[7] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (21 * spectrumBarSegmentWidthP) + (7 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (22 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[8] * 2; i >= ((mpsPlayer_spectrumPeakHold[8] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[8] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (24 * spectrumBarSegmentWidthP) + (8 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (25 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[9] * 2; i >= ((mpsPlayer_spectrumPeakHold[9] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[9] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (27 * spectrumBarSegmentWidthP) + (9 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (28 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[10] * 2; i >= ((mpsPlayer_spectrumPeakHold[10] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[10] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (30 * spectrumBarSegmentWidthP) + (10 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (31 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[11] * 2; i >= ((mpsPlayer_spectrumPeakHold[11] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[11] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (33 * spectrumBarSegmentWidthP) + (11 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (34 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            for (int i = mpsPlayer_spectrumPeakHold[12] * 2; i >= ((mpsPlayer_spectrumPeakHold[12] * 2) - 1); i--)
            {
                if (mpsPlayer_spectrumPeakHold[12] == 0) break;
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (36 * spectrumBarSegmentWidthP) + (12 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                if (i <= 8) graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakBaseColor), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
                else graphics_mpsPlayerInterface.FillRectangle(new SolidBrush(class_mpsPlayerSkinHandler.color_spectrumPeakHighColor), spectrumBarX0P + (37 * spectrumBarSegmentWidthP) + (13 * spectrumBarSegmentDeltaP), spectrumBarY0P - ((i * spectrumBarSegmentHeightP) + ((i - 1) * spectrumBarSegmentHeightP - (spectrumBarHeightGapReducerP * i))), spectrumBarSegmentWidthP, spectrumBarSegmentHeightP);
            }
            pictureBox_mpsPlayer.Image = bitmap_mpsPlayerInterface;
        }

        /// <summary>
        /// Отрисовка статуса аудиопроцессора в виде индикаторов
        /// </summary>
        public void DrawAudioProcessorStatus()
        {
            if (subcodeSyncIndicatorCount >= 4) { DataHandler.subcodeSync = false; subcodeSyncIndicatorCount = 0; }
            if (subcodeSyncErrorIndicatorCount >= 4) { DataHandler.subcodeSyncError = false; subcodeSyncErrorIndicatorCount = 0; }
            if (subcodeTimecodeIndicatorCount >= 4) { DataHandler.subcodeTimecode = false; subcodeTimecodeIndicatorCount = 0; }
            if (subcodeTOCIndicatorCount >= 4) { DataHandler.subcodeTOC = false; subcodeTOCIndicatorCount = 0; }
            if (audioInterpolationIndicatorCount >= 4) { DataHandler.interpolation = false; audioInterpolationIndicatorCount = 0; }
            if (audioMutingIndicatorCount >= 4) { DataHandler.mute = false; audioMutingIndicatorCount = 0; }
            if (packetLossIndicatorCount >= 4) { DataHandler.packetLoss = false; packetLossIndicatorCount = 0; }
            label_subcodeSync.Image = null;
            label_subcodeTimecode.Image = null;
            label_subcodeTOC.Image = null;
            label_interpolation.Image = null;
            label_mute.Image = null;
            label_packetLoss.Image = null;
            if (DataHandler.subcodeSync == false) graphics_subcodeSyncIndicator.FillEllipse(new SolidBrush(Color.Gray), 4, 0, 11, 11); else { graphics_subcodeSyncIndicator.FillEllipse(new SolidBrush(Color.Green), 4, 0, 11, 11); subcodeSyncIndicatorCount++; }
            if (DataHandler.subcodeSyncError == true) graphics_subcodeSyncIndicator.FillEllipse(new SolidBrush(Color.Red), 4, 0, 11, 11); subcodeSyncErrorIndicatorCount++;
            if (DataHandler.subcodeTimecode == false) graphics_subcodeTimecodeIndicator.FillEllipse(new SolidBrush(Color.Gray), 4, 0, 11, 11); else { graphics_subcodeTimecodeIndicator.FillEllipse(new SolidBrush(Color.Green), 4, 0, 11, 11); subcodeTimecodeIndicatorCount++; }
            if (DataHandler.subcodeTOC == false) graphics_subcodeTOCIndicator.FillEllipse(new SolidBrush(Color.Gray), 4, 0, 11, 11); else { graphics_subcodeTOCIndicator.FillEllipse(new SolidBrush(Color.Green), 4, 0, 11, 11); subcodeTOCIndicatorCount++; }
            if (DataHandler.interpolation == false) graphics_audioInterpolationIndicator.FillEllipse(new SolidBrush(Color.Gray), 4, 0, 11, 11); else { graphics_audioInterpolationIndicator.FillEllipse(new SolidBrush(Color.Red), 4, 0, 11, 11); audioInterpolationIndicatorCount++; }
            if (DataHandler.mute == false) graphics_audioMutingIndicator.FillEllipse(new SolidBrush(Color.Gray), 4, 0, 11, 11); else { graphics_audioMutingIndicator.FillEllipse(new SolidBrush(Color.Red), 4, 0, 11, 11); audioMutingIndicatorCount++; }
            if (DataHandler.packetLoss == false) graphics_packetLossIndicator.FillEllipse(new SolidBrush(Color.Gray), 4, 0, 11, 11); else { graphics_packetLossIndicator.FillEllipse(new SolidBrush(Color.Red), 4, 0, 11, 11); packetLossIndicatorCount++; }
            label_subcodeSync.Image = bitmap_subcodeSyncIndicator;
            label_subcodeTimecode.Image = bitmap_subcodeTimecodeIndicator;
            label_subcodeTOC.Image = bitmap_subcodeTOCIndicator;
            label_interpolation.Image = bitmap_audioInterpolationIndicator;
            label_mute.Image = bitmap_audioMutingIndicator;
            label_packetLoss.Image = bitmap_packetLossIndicator;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Settings.Load();
            class_mpsPlayerSkinHandler.Load();           
            MpsPlayerInterfaceInitialize();
            bitmap_waveGraphL = new Bitmap(pictureBox_waveGraphL.Width, pictureBox_waveGraphL.Height);
            bitmap_waveGraphR = new Bitmap(pictureBox_waveGraphR.Width, pictureBox_waveGraphR.Height);
            graphics_waveGraphL = Graphics.FromImage(bitmap_waveGraphL); //Инициализация графики
            graphics_waveGraphR = Graphics.FromImage(bitmap_waveGraphR); //Инициализация графики
            bitmap_mpsPlayerInterface = new Bitmap(pictureBox_mpsPlayer.Width, pictureBox_mpsPlayer.Height);
            graphics_mpsPlayerInterface = Graphics.FromImage(bitmap_mpsPlayerInterface);

            bitmap_subcodeSyncIndicator = new Bitmap(label_subcodeSync.Width, label_subcodeSync.Height);
            bitmap_subcodeTimecodeIndicator = new Bitmap(label_subcodeTimecode.Width, label_subcodeTimecode.Height);
            bitmap_subcodeTOCIndicator = new Bitmap(label_subcodeTOC.Width, label_subcodeTOC.Height);
            bitmap_audioInterpolationIndicator = new Bitmap(label_interpolation.Width, label_interpolation.Height);
            bitmap_audioMutingIndicator = new Bitmap(label_mute.Width, label_mute.Height);
            bitmap_packetLossIndicator = new Bitmap(label_packetLoss.Width, label_packetLoss.Height);

            graphics_subcodeSyncIndicator = Graphics.FromImage(bitmap_subcodeSyncIndicator);
            graphics_subcodeTimecodeIndicator = Graphics.FromImage(bitmap_subcodeTimecodeIndicator);
            graphics_subcodeTOCIndicator = Graphics.FromImage(bitmap_subcodeTOCIndicator);
            graphics_audioInterpolationIndicator = Graphics.FromImage(bitmap_audioInterpolationIndicator);
            graphics_audioMutingIndicator = Graphics.FromImage(bitmap_audioMutingIndicator);
            graphics_packetLossIndicator = Graphics.FromImage(bitmap_packetLossIndicator);

            DrawAudioProcessorStatus();
            AudioIO.GraphCaptureInit();
            MpsPlayerRunningIndicatorStop();         
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIO.GraphCaptureClose();
            AudioIO.SignalCaptureClose();
            Settings.Save();
            //Close();
            Environment.Exit(0);
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            DrawAudioProcessorStatus();
            label_fixedErrorCount.Text = "Исправлено: " + Decoder.fixedErrorCount.ToString();
            label_unfixedErrorCount.Text = "Неисправимые: " + Decoder.unfixedErrorCount.ToString();
            label_frameSyncErrorCount.Text = "Кадровая синхр.: " + Decoder.frameSyncErrorCount.ToString();
            label_signalQuality.Text = "Качество сигнала: " + Decoder.signalQuality.ToString() + "%";
            label_decodedPacketSize.Text = "Размер пакета: " + DataHandler.packetSize.ToString() + " байт";
            label_audioBufferSize.Text = "Аудио буфер: ";
            label_trackNumber.Text = "Дорожка: " + mpsPlayer_currentTrackNumber.ToString();
            label_trackCount.Text = "Всего дорожек: " + mpsPlayer_trackCount.ToString();
            this.Text = new string(DataHandler.artist) + " / " + new string(DataHandler.track);
            try
            {
                if (DataHandler.ms != null)
                {
                    int audioBufferSamples = (int)(0.5 * ((int)DataHandler.ms.Length - (int)DataHandler.ms.Position));
                    label_audioBufferSize.Text += ((double)audioBufferSamples / 48000).ToString() + " сек";
                    if (audioBufferSamples <= progressBar_audioBuffer.Maximum) progressBar_audioBuffer.Value = audioBufferSamples; else progressBar_audioBuffer.Value = progressBar_audioBuffer.Maximum;
                }
            }
            catch { }

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

            if (mpsPlayer_skinEdit && mpsPlayer_control != null && mpsPlayer_controlFree)
            {
                PictureBox pb = (PictureBox)mpsPlayer_control;
                var pos = pictureBox_mpsPlayer.PointToClient(Cursor.Position);
                pb.Location = new Point(pos.X, pos.Y);
            }
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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

        }

        private void form_main_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                bitmap_waveGraphL = new Bitmap(pictureBox_waveGraphL.Width, pictureBox_waveGraphL.Height);
                bitmap_waveGraphR = new Bitmap(pictureBox_waveGraphR.Width, pictureBox_waveGraphR.Height);
                graphics_waveGraphL = Graphics.FromImage(bitmap_waveGraphL); //Инициализация графики
                graphics_waveGraphR = Graphics.FromImage(bitmap_waveGraphR); //Инициализация графики
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
            pictureBox_waveGraphL.MouseWheel += new MouseEventHandler(pictureBox_waveGraph_MouseWheel);
        }

        private void pictureBox_waveGraph_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_waveGraphL.MouseWheel -= new MouseEventHandler(pictureBox_waveGraph_MouseWheel);
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
            if (e.Button == MouseButtons.Middle) { scope_horizontalBIAS = 0; scope_horizontalScale = 1; scope_verticalScale = 32767; scope_verticalBIAS = pictureBox_waveGraphL.Height / 2; scope_additionalHorizontalScale = 1; }
        }

        private void button_buffMp3_Click(object sender, EventArgs e)
        {
            if (!Decoder.decoderActive)
            {
                Decoder.decoderActive = true;
                AudioIO.SignalCaptureInit();
                DataHandler.StartMp3Listening();
                Decoder.Start();
                mpsPlayer_liveSpectrum = new int[] { 6, 5, 3, 1, 2, 1, 3, 4, 3, 2, 3, 5, 6 };             
                timer_mpsPlayerHandler.Enabled = true;
                timer_mpsPlayerSpectrumHandler.Enabled = true;
                timer_mpsPlayerSpectrumUpdater.Enabled = true;
                timer_mpsPlayerTimeUpdater.Enabled = true;
                timer_signalQualityUpdater.Enabled = true;
                mpsPlayer_currentTrackNumber = 1;
                mpsPlayer_trackCount = 16;
                DataHandler.fs_decodedAudio = new FileStream("DecodedAudio.wav", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                DataHandler.fs_decodedAudio.Seek(44, SeekOrigin.Begin);
            }
        }

        public void MpsPlayerRunningIndicatorPlay()
        {
            timer_mpsPlayerRunningIndicatorHandler.Interval = 85;
            timer_mpsPlayerRunningIndicatorHandler.Enabled = true;
            window_main.pictureBox_playPause.Image = class_mpsPlayerSkinHandler.image_CD[9];
        }

        public void MpsPlayerRunningIndicatorSeek()
        {
            if (mpsPlayer_tapeSkin == false)
            {
                timer_mpsPlayerRunningIndicatorHandler.Interval = 45;
                timer_mpsPlayerRunningIndicatorHandler.Enabled = true;
            }
            else { timer_mpsPlayerRunningIndicatorHandler.Enabled = false; pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.runningIndicator_stop; }
        }

        public void MpsPlayerRunningIndicatorStop()
        {
            timer_mpsPlayerRunningIndicatorHandler.Enabled = false;
            mpsPlayer_mode = "stop";
            window_main.pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[0];
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

        /// <summary>
        /// Инициализация графики интерфейса плеера
        /// </summary>
        public void MpsPlayerInterfaceInitialize()
        {
            symbolImages = new Image[] { class_mpsPlayerSkinHandler.image_symbols[0], class_mpsPlayerSkinHandler.image_symbols[1], class_mpsPlayerSkinHandler.image_symbols[2], class_mpsPlayerSkinHandler.image_symbols[3], class_mpsPlayerSkinHandler.image_symbols[4], class_mpsPlayerSkinHandler.image_symbols[5], class_mpsPlayerSkinHandler.image_symbols[6], class_mpsPlayerSkinHandler.image_symbols[7], class_mpsPlayerSkinHandler.image_symbols[8], class_mpsPlayerSkinHandler.image_symbols[9] };
            mpsPlayerWidth = 810;
            mpsPlayerHeight = 335;
            spectrumBarWidthP = (int)Math.Ceiling(mpsPlayerWidth * spectrumBarWidth);
            spectrumBarHeightP = (int)Math.Ceiling((double)mpsPlayerHeight * (double)spectrumBarHeight);
            spectrumBarSegmentWidthP = (int)Math.Ceiling((double)spectrumBarWidthP / spectrumBarSegmentWidthCount);
            spectrumBarSegmentHeightP = (int)Math.Floor((double)spectrumBarHeightP / spectrumBarSegmentHeightCount);
            spectrumBarSegmentDeltaP = (int)Math.Ceiling((double)spectrumBarSegmentWidthP / spectrumBarSegmentDeltaCount);
            spectrumBarHeightGapReducerP = -1;

            pictureBox_timeSymbols = new PictureBox[] { window_main.pictureBox_symbol7, window_main.pictureBox_symbol8, window_main.pictureBox_symbol9, window_main.pictureBox_symbol10 };
            pictureBox_trackNumberSymbols = new PictureBox[] { window_main.pictureBox_symbol4, window_main.pictureBox_symbol5 };
            pictureBox_textSymbols = new PictureBox[] { window_main.pictureBox_1, window_main.pictureBox_2, window_main.pictureBox_3, window_main.pictureBox_4, window_main.pictureBox_5, window_main.pictureBox_6, window_main.pictureBox_7, window_main.pictureBox_8, window_main.pictureBox_9, window_main.pictureBox_10, window_main.pictureBox_11, window_main.pictureBox_12, window_main.pictureBox_13, window_main.pictureBox_14, window_main.pictureBox_15, window_main.pictureBox_16, window_main.pictureBox_17, window_main.pictureBox_18, window_main.pictureBox_19, window_main.pictureBox_20 };
            pictureBox_spectrumBorder1.Image = class_mpsPlayerSkinHandler.image_misc[0];
            pictureBox_spectrumBorder2.Image = class_mpsPlayerSkinHandler.image_misc[0];
            window_main.pictureBox_dots.Image = class_mpsPlayerSkinHandler.image_symbols[18];
            if (mpsPlayer_tapeSkin == false)
            {
                pictureBox_playPause.Visible = true;
                pictureBox_symbol1.Image = class_mpsPlayerSkinHandler.image_symbols[10];
                pictureBox_symbol2.Image = class_mpsPlayerSkinHandler.image_symbols[11];
                pictureBox_symbol3.Image = null;
                pictureBox_symbol4.Image = class_mpsPlayerSkinHandler.image_symbols[0];
                pictureBox_symbol5.Image = class_mpsPlayerSkinHandler.image_symbols[0];
                pictureBox_symbol6.Image = null;
                pictureBox_symbol7.Image = null;
                pictureBox_symbol8.Image = class_mpsPlayerSkinHandler.image_symbols[0];
                pictureBox_symbol9.Image = class_mpsPlayerSkinHandler.image_symbols[0];
                pictureBox_symbol10.Image = class_mpsPlayerSkinHandler.image_symbols[0];
                pictureBox_track1.Image = class_mpsPlayerSkinHandler.image_trackCalendar[0];
                pictureBox_track2.Image = class_mpsPlayerSkinHandler.image_trackCalendar[1];
                pictureBox_track3.Image = class_mpsPlayerSkinHandler.image_trackCalendar[2];
                pictureBox_track4.Image = class_mpsPlayerSkinHandler.image_trackCalendar[3];
                pictureBox_track5.Image = class_mpsPlayerSkinHandler.image_trackCalendar[4];
                pictureBox_track6.Image = class_mpsPlayerSkinHandler.image_trackCalendar[5];
                pictureBox_track7.Image = class_mpsPlayerSkinHandler.image_trackCalendar[6];
                pictureBox_track8.Image = class_mpsPlayerSkinHandler.image_trackCalendar[7];
                pictureBox_track9.Image = class_mpsPlayerSkinHandler.image_trackCalendar[8];
                pictureBox_track10.Image = class_mpsPlayerSkinHandler.image_trackCalendar[9];
                pictureBox_track11.Image = class_mpsPlayerSkinHandler.image_trackCalendar[10];
                pictureBox_track12.Image = class_mpsPlayerSkinHandler.image_trackCalendar[11];
                pictureBox_track13.Image = class_mpsPlayerSkinHandler.image_trackCalendar[12];
                pictureBox_track14.Image = class_mpsPlayerSkinHandler.image_trackCalendar[13];
                pictureBox_track15.Image = class_mpsPlayerSkinHandler.image_trackCalendar[14];
                pictureBox_track16.Image = class_mpsPlayerSkinHandler.image_trackCalendar[15];
                pictureBox_dots.Visible = true;
                pictureBox_disc1.Visible = true;
                pictureBox_disc2.Visible = true;
                pictureBox_disc3.Visible = true;
                pictureBox_cassette.Image = null;
                pictureBox_disc1.Image = class_mpsPlayerSkinHandler.image_CD[2];
                pictureBox_disc2.Image = class_mpsPlayerSkinHandler.image_CD[4];
                pictureBox_disc3.Image = class_mpsPlayerSkinHandler.image_CD[7];
            }
            else
            {
                MpsPlayerTrackCalendarSetAmount(0);
                pictureBox_playPause.Visible = false;
                pictureBox_symbol1.Image = class_mpsPlayerSkinHandler.image_symbols[14];
                pictureBox_symbol2.Image = class_mpsPlayerSkinHandler.image_symbols[15];
                pictureBox_symbol3.Image = class_mpsPlayerSkinHandler.image_symbols[13];
                pictureBox_symbol4.Image = class_mpsPlayerSkinHandler.image_symbols[16];
                pictureBox_symbol5.Image = null;
                pictureBox_symbol6.Image = null;
                pictureBox_symbol7.Image = class_mpsPlayerSkinHandler.image_symbols[0];
                pictureBox_dots.Visible = false;
                pictureBox_disc1.Visible = false;
                pictureBox_disc2.Visible = false;
                pictureBox_disc3.Visible = false;
            }
        }

        /// <summary>
        /// Включает отладочный режим отображения дисплея MPS
        /// </summary>
        public void MpsPlayerInterfaceDebug()
        {
            symbolImages = new Image[] { class_mpsPlayerSkinHandler.image_symbols[0], class_mpsPlayerSkinHandler.image_symbols[1], class_mpsPlayerSkinHandler.image_symbols[2], class_mpsPlayerSkinHandler.image_symbols[3], class_mpsPlayerSkinHandler.image_symbols[4], class_mpsPlayerSkinHandler.image_symbols[5], class_mpsPlayerSkinHandler.image_symbols[6], class_mpsPlayerSkinHandler.image_symbols[7], class_mpsPlayerSkinHandler.image_symbols[8], class_mpsPlayerSkinHandler.image_symbols[9] };
            mpsPlayerWidth = 810;
            mpsPlayerHeight = 335;
            spectrumBarWidthP = (int)Math.Ceiling(mpsPlayerWidth * spectrumBarWidth);
            spectrumBarHeightP = (int)Math.Ceiling((double)mpsPlayerHeight * (double)spectrumBarHeight);
            spectrumBarSegmentWidthP = (int)Math.Ceiling((double)spectrumBarWidthP / spectrumBarSegmentWidthCount);
            spectrumBarSegmentHeightP = (int)Math.Floor((double)spectrumBarHeightP / spectrumBarSegmentHeightCount);
            spectrumBarSegmentDeltaP = (int)Math.Ceiling((double)spectrumBarSegmentWidthP / spectrumBarSegmentDeltaCount);
            spectrumBarHeightGapReducerP = -1;

            pictureBox_timeSymbols = new PictureBox[] { window_main.pictureBox_symbol7, window_main.pictureBox_symbol8, window_main.pictureBox_symbol9, window_main.pictureBox_symbol10 };
            pictureBox_trackNumberSymbols = new PictureBox[] { window_main.pictureBox_symbol4, window_main.pictureBox_symbol5 };
            pictureBox_spectrumBorder1.Image = class_mpsPlayerSkinHandler.image_misc[0];
            pictureBox_spectrumBorder2.Image = class_mpsPlayerSkinHandler.image_misc[0];
            window_main.pictureBox_dots.Image = class_mpsPlayerSkinHandler.image_symbols[18];
            pictureBox_playPause.Visible = true;
            pictureBox_playPause.Image = class_mpsPlayerSkinHandler.image_CD[9];
            pictureBox_symbol1.Image = class_mpsPlayerSkinHandler.image_symbols[0];
            pictureBox_symbol2.Image = class_mpsPlayerSkinHandler.image_symbols[1];
            pictureBox_symbol3.Image = class_mpsPlayerSkinHandler.image_symbols[2];
            pictureBox_symbol4.Image = class_mpsPlayerSkinHandler.image_symbols[3];
            pictureBox_symbol5.Image = class_mpsPlayerSkinHandler.image_symbols[4];
            pictureBox_symbol6.Image = class_mpsPlayerSkinHandler.image_symbols[5];
            pictureBox_symbol7.Image = class_mpsPlayerSkinHandler.image_symbols[6];
            pictureBox_symbol8.Image = class_mpsPlayerSkinHandler.image_symbols[7];
            pictureBox_symbol9.Image = class_mpsPlayerSkinHandler.image_symbols[8];
            pictureBox_symbol10.Image = class_mpsPlayerSkinHandler.image_symbols[9];
            pictureBox_track1.Image = class_mpsPlayerSkinHandler.image_trackCalendar[0];
            pictureBox_track2.Image = class_mpsPlayerSkinHandler.image_trackCalendar[1];
            pictureBox_track3.Image = class_mpsPlayerSkinHandler.image_trackCalendar[2];
            pictureBox_track4.Image = class_mpsPlayerSkinHandler.image_trackCalendar[3];
            pictureBox_track5.Image = class_mpsPlayerSkinHandler.image_trackCalendar[4];
            pictureBox_track6.Image = class_mpsPlayerSkinHandler.image_trackCalendar[5];
            pictureBox_track7.Image = class_mpsPlayerSkinHandler.image_trackCalendar[6];
            pictureBox_track8.Image = class_mpsPlayerSkinHandler.image_trackCalendar[7];
            pictureBox_track9.Image = class_mpsPlayerSkinHandler.image_trackCalendar[8];
            pictureBox_track10.Image = class_mpsPlayerSkinHandler.image_trackCalendar[9];
            pictureBox_track11.Image = class_mpsPlayerSkinHandler.image_trackCalendar[10];
            pictureBox_track12.Image = class_mpsPlayerSkinHandler.image_trackCalendar[11];
            pictureBox_track13.Image = class_mpsPlayerSkinHandler.image_trackCalendar[12];
            pictureBox_track14.Image = class_mpsPlayerSkinHandler.image_trackCalendar[13];
            pictureBox_track15.Image = class_mpsPlayerSkinHandler.image_trackCalendar[14];
            pictureBox_track16.Image = class_mpsPlayerSkinHandler.image_trackCalendar[15];
            pictureBox_dots.Visible = true;
            pictureBox_disc1.Visible = true;
            pictureBox_disc2.Visible = true;
            pictureBox_disc3.Visible = true;
            pictureBox_cassette.Image = class_mpsPlayerSkinHandler.image_tape[0];
            pictureBox_disc1.Image = class_mpsPlayerSkinHandler.image_CD[0];
            pictureBox_disc2.Image = class_mpsPlayerSkinHandler.image_CD[3];
            pictureBox_disc3.Image = class_mpsPlayerSkinHandler.image_CD[6];

            foreach (PictureBox pb in pictureBox_textSymbols) pb.Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\symbol.png")];
        }

        private void timer_mpsPlayerHandler_Tick(object sender, EventArgs e)
        { 
            if (mpsPlayer_tapeSkin == false)
            {
                //Отображение текущего времени воспроизведения
                if (mpsPlayer_remainingTime == false)
                {
                    pictureBox_symbol6.Image = null;
                    pictureBox_symbol7.Image = null;
                    mpsPlayer_time = Decoder.GetTimeFromSeconds(mpsPlayer_timeSeconds);
                }
                else
                {
                    if (mpsPlayer_timeDurationSeconds - mpsPlayer_timeSeconds >= 600) pictureBox_symbol6.Image = class_mpsPlayerSkinHandler.image_symbols[17];
                    else
                    {
                        pictureBox_symbol6.Image = null;
                        pictureBox_symbol7.Image = class_mpsPlayerSkinHandler.image_symbols[17];
                    }
                    mpsPlayer_time = Decoder.GetTimeFromSeconds(mpsPlayer_timeDurationSeconds - mpsPlayer_timeSeconds);
                }
                if (mpsPlayer_showTime == true)
                {
                    window_main.pictureBox_dots.Image = class_mpsPlayerSkinHandler.image_symbols[18];
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
                    pictureBox_trackNumberSymbols[0].Image = class_mpsPlayerSkinHandler.image_symbols[0];
                    pictureBox_trackNumberSymbols[1].Image = class_mpsPlayerSkinHandler.image_symbols[0];
                }
                if (mpsPlayer_disc1Detected == true) pictureBox_disc1.Image = class_mpsPlayerSkinHandler.image_CD[0];
                else pictureBox_disc1.Image = class_mpsPlayerSkinHandler.image_CD[2];
            }
            else
            {

                if (mpsPlayer_disc1Detected == true) pictureBox_cassette.Image = class_mpsPlayerSkinHandler.image_tape[0];
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

        private void timer_mpsPlayerSpectrumHandler_Tick(object sender, EventArgs e)
        {
            if (mpsPlayer_peakHoldTimeCount >= mpsPlayer_peakHoldTimeDelay)
            {
                mpsPlayer_peakHoldTimeCount = 0;
                mpsPlayer_spectrumPeakHold = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            for (int i = 0; i < mpsPlayer_instantSpectrum.Length; i++)
            {
                if (mpsPlayer_instantSpectrum[i] > mpsPlayer_liveSpectrum[i] && mpsPlayer_liveSpectrum[i] < 9) mpsPlayer_liveSpectrum[i]++;
                else if (mpsPlayer_liveSpectrum[i] > 0) mpsPlayer_liveSpectrum[i]--;
            }
            for (int i = 0; i < mpsPlayer_liveSpectrum.Length; i++) if (mpsPlayer_spectrumPeakHold[i] < mpsPlayer_liveSpectrum[i]) mpsPlayer_spectrumPeakHold[i] = mpsPlayer_liveSpectrum[i];
            if (mpsPlayer_spectrumMode == "noPeak") { mpsPlayer_spectrumPeakHold = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; задержкаПиковToolStripMenuItem.Checked = false; }
            if (mpsPlayer_spectrumMode == "peakHold") задержкаПиковToolStripMenuItem.Checked = true;
            if (mpsPlayer_spectrumMode == "off") { отображатьСпектрToolStripMenuItem.Checked = false; mpsPlayer_spectrumPeakHold = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; mpsPlayer_liveSpectrum = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; mpsPlayer_instantSpectrum = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; }
            if (mpsPlayer_spectrumMode != "off") { отображатьСпектрToolStripMenuItem.Checked = true; }
            DrawMPSPlayerInterface();
            mpsPlayer_peakHoldTimeCount++;
        }

        private void timer_mpsPlayerSpectrumUpdater_Tick(object sender, EventArgs e)
        {
            //Подсос RAW PCM декодирвоанного аудио для спектроанализатора
            AudioIO.buff_fftSamples = new double[form_main.mpsPlayer_fftSize];
            List<double> tempSamples = new List<double>();
            byte[] PCMbytes = new byte[form_main.mpsPlayer_fftSize * 2];
            long msPos = DataHandler.ms.Position;
            DataHandler.ms.Read(PCMbytes, 0, PCMbytes.Length);
            DataHandler.ms.Seek(msPos, SeekOrigin.Begin);
            for (int k = 0; k < (AudioIO.buff_fftSamples.Length * 2) - 1; k += 2) tempSamples.Add(BitConverter.ToInt16(new byte[] { PCMbytes[k], PCMbytes[k + 1] }, 0));
            AudioIO.buff_fftSamples = tempSamples.ToArray();
            double[] paddedAudio = FftSharp.Pad.ZeroPad(AudioIO.buff_fftSamples);
            double[] fftMag = FftSharp.Transform.FFTpower(paddedAudio);
            AudioIO.buff_fftValues = new double[fftMag.Length];
            //LogHandler.WriteStatus("DataHandler/AudioBuffer", "fft");

            if (AudioIO.buff_fftValues != null)
            {
                paddedAudio = FftSharp.Pad.ZeroPad(AudioIO.buff_fftSamples);
                System.Numerics.Complex[] complex = FftSharp.FFT.Forward(paddedAudio);
                fftMag = FftSharp.FFT.Magnitude(complex);
                double[] frequencyValues = FftSharp.FFT.FrequencyScale(fftMag.Length, 48000);
                Array.Copy(fftMag, AudioIO.buff_fftValues, fftMag.Length);
                double[] RAWspectrumSelection = new double[mpsPlayer_instantSpectrum.Length];
                double[] RAWspectrumSelectionKenwood = new double[mpsPlayer_spectrumFreq.Length];

                //Выборка заданных частот
                for (int i = 0, k = 0; i < AudioIO.buff_fftValues.Length && k < mpsPlayer_spectrumFreq.Length;)
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
                for (int i = 0, k = 0, j = 1; (i < RAWspectrumSelectionKenwood.Length); i++, k += 2, j += 2)
                {
                    RAWspectrumSelection[k] = RAWspectrumSelectionKenwood[i];
                    if (j < RAWspectrumSelection.Length) RAWspectrumSelection[j] = 0.5 * (RAWspectrumSelectionKenwood[i] + RAWspectrumSelectionKenwood[i + 1]);
                }

                //Преобразование уровня спектра к шкале 0-9
                for (int i = 0; i < mpsPlayer_instantSpectrum.Length; i++)
                {
                    //Фильтры чистого спектра
                    if (i == 0) RAWspectrumSelection[i] *= 0.5;
                    if (i == 8) RAWspectrumSelection[i] *= 2.0;
                    if (i == 9) RAWspectrumSelection[i] *= 4.0;
                    if (i == 10) RAWspectrumSelection[i] *= 11.0;
                    if (i == 11) RAWspectrumSelection[i] *= 20.0;
                    if (i == 12) RAWspectrumSelection[i] *= 25.0;
                    if (RAWspectrumSelection[i] > 3000) RAWspectrumSelection[i] = 3000;
                    mpsPlayer_instantSpectrum[i] = (int)Math.Floor((9 * RAWspectrumSelection[i]) / 3000.0);
                }
            }
            else for (int i = 0; i < mpsPlayer_instantSpectrum.Length; i++) mpsPlayer_instantSpectrum[i] = 0;
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

        private void groupBox_signalCapture_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox_playDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Decoder.decoderActive = false;
            Decoder.Stop();
            AudioIO.SignalCaptureClose();
            DataHandler.StopMp3Listening();
            MpsPlayerRunningIndicatorStop();
            MpsPlayerTrackCalendarSetAmount(16);
            MpsPlayerTrackCalendarSetCurrentTrack(0);
            timer_mpsPlayerHandler.Enabled = false;
            timer_mpsPlayerSpectrumHandler.Enabled = false;
            timer_mpsPlayerSpectrumUpdater.Enabled = false;
            timer_mpsPlayerTimeUpdater.Enabled = false;
            timer_signalQualityUpdater.Enabled = false;
            Decoder.ClearBuffers();
            MpsPlayerInterfaceInitialize();
            DataHandler.fs_decodedAudio.Seek(0, SeekOrigin.Begin);
            Encoder.WriteHeader(DataHandler.fs_decodedAudio, 48000, 1);
            DataHandler.fs_decodedAudio.Close();
            DataHandler.fs_decodedAudio.Dispose();
        }

        private void timer_signalQualityUpdater_Tick(object sender, EventArgs e)
        {
            if (Decoder.signalQuality >= 80)
            {
                if (form_main.mpsPlayer_mode != "play") { MpsPlayerRunningIndicatorPlay(); Decoder.fixedErrorCount = 0; Decoder.frameSyncErrorCount = 0; Decoder.unfixedErrorCount = 0; }
                form_main.mpsPlayer_mode = "play";
                form_main.mpsPlayer_disc1Detected = true;
            }
            else
            {
                if (form_main.mpsPlayer_mode != "seek") MpsPlayerRunningIndicatorSeek();
                form_main.mpsPlayer_mode = "seek";
                form_main.mpsPlayer_disc1Detected = false;
                Decoder.fixedErrorCount = 0;
                Decoder.frameSyncErrorCount = 0;
                Decoder.unfixedErrorCount = 0;
            }
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (window_settings != null)
            {
                window_settings.Dispose();
                window_settings = new form_settings();
            }
            window_settings.Show();
        }

        private void отладкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (form_main.window_debug != null)
            {
                form_main.window_debug.Close();
                form_main.window_debug = new form_debug();
            }
            form_main.window_debug.Show();
        }


        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string bitSequence = "s01110010s"; // Задайте свою последовательность бит с синхроимпульсами
            int sampleRate = 82000; // Частота дискретизации
            int frequency = 10000; // Частота сигнала

            short[] amSignal = GenerateAMSignal(bitSequence, sampleRate, frequency);
            short[] filteredSignal = LowPassFilter(amSignal, sampleRate, frequency);

            SaveWav("am_signal.wav", filteredSignal, sampleRate);
        }

        private void trackBar_spectrumGain_Scroll(object sender, EventArgs e)
        {
            
        }

        static short[] GenerateAMSignal(string bitSequence, int sampleRate, int frequency)
        {
            int amplitudeSync = short.MaxValue; // Амплитуда для синхроимпульса
            int amplitudeOne = amplitudeSync / 2; // Амплитуда для единицы
            int amplitudeZero = amplitudeOne / 2; // Амплитуда для нуля
            double period = 1.0 / frequency; // Длительность одного периода в секундах
            int samplesPerBit = (int)(sampleRate * period); // Количество отсчетов на один бит
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
            int filterOrder = 5; // Порядок фильтра
            double alpha = 1.0 / (1.0 + (sampleRate / (2 * Math.PI * cutoffFrequency)));

            short[] filteredSignal = new short[signal.Length];
            filteredSignal[0] = signal[0]; // Инициализация первого значения

            for (int i = 1; i < signal.Length; i++)
            {
                filteredSignal[i] = (short)(alpha * signal[i] + (1 - alpha) * filteredSignal[i - 1]);
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
                bw.Write((short)1); // Количество каналов
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

        private void timer_mpsPlayerRunningIndicatorHandler_Tick(object sender, EventArgs e)
        {
            if (mpsPlayer_tapeSkin == false)
            {
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 0) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[1];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 1) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[2];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 2) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[3];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 3) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[4];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 4) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[5];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 5) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[6];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 6) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[7];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 7) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[8];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 8) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[9];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 9) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[10];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 10) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[11];
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 11) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[12];
                mpsPlayer_runningIndicatorAnimationFrameIndex++;
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 12) mpsPlayer_runningIndicatorAnimationFrameIndex = 0;
            }
            else
            {
                if (mpsPlayer_remainingTime == false)
                {
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 0) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[13];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 1) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[14];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 2) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[15];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 3) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[16];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 4) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[17];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 5) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[18];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 6) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[19];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 7) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[20];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 8) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[21];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 9) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[22];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 10) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[23];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 11) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[24];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 12) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[25];
                }
                else
                {
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 0) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[26];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 1) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[27];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 2) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[28];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 3) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[29];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 4) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[30];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 5) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[31];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 6) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[32];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 7) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[33];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 8) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[34];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 9) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[35];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 10) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[36];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 11) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[37];
                    if (mpsPlayer_runningIndicatorAnimationFrameIndex == 12) pictureBox_runningIndicator.Image = class_mpsPlayerSkinHandler.image_runningIndicator[38];
                }
                mpsPlayer_runningIndicatorAnimationFrameIndex++;
                if (mpsPlayer_runningIndicatorAnimationFrameIndex == 13) mpsPlayer_runningIndicatorAnimationFrameIndex = 0;
            }
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!редактироватьToolStripMenuItem1.Checked)
            {
                редактироватьToolStripMenuItem1.Checked = true;
                редактированиеToolStripMenuItem.Checked = true;
                задатьЭлементДляВыравниванияToolStripMenuItem.Enabled = true;
                выровнятьПоВертикалиToolStripMenuItem.Enabled = true;
                выровнятьПоГоризонталиToolStripMenuItem.Enabled = true;
                уменьшитьToolStripMenuItem.Enabled = true;
                увеличитьToolStripMenuItem.Enabled = true;
                выстроитьСогласноСкинуToolStripMenuItem.Enabled = true;
                выстроитьВНулевоеПоложениеToolStripMenuItem.Enabled = true;
                перемещениеToolStripMenuItem.Enabled = true;
                toolStripButton_alignX.Enabled = true;
                toolStripButton_alignY.Enabled = true;
                toolStripButton_zoomIn.Enabled = true;
                toolStripButton_zoomOut.Enabled = true;
                увеличитьВысотуToolStripMenuItem.Enabled = true;
                toolStripButton_heightUp.Enabled = true;
                уменьшитьВысотуToolStripMenuItem.Enabled = true;
                toolStripButton_heightDown.Enabled = true;
                увеличитьДлинуToolStripMenuItem.Enabled = true;
                toolStripButton_widthUp.Enabled = true;
                уменьшитьДлинуToolStripMenuItem.Enabled = true;
                toolStripButton_widthDown.Enabled = true;
                выбранныйЭлементToolStripMenuItem.Enabled = true;
                сдвинутьВсеЭлементыВверхToolStripMenuItem.Enabled = true;
                сдвинутьВсеЭлементыВнизToolStripMenuItem.Enabled = true;
                сдвинутьВсеЭлементыВлевоToolStripMenuItem.Enabled = true;
                сдвинутьВсеЭлементыВправоToolStripMenuItem.Enabled = true;
                mpsPlayer_skinEdit = true;
                timer_mpsPlayerHandler.Enabled = false;              
                //timer_mpsPlayerSpectrumUpdater.Enabled = false;
                mpsPlayer_instantSpectrum = new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
                mpsPlayer_liveSpectrum = new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
                timer_mpsPlayerRunningIndicatorHandler.Enabled = false;
                timer_signalQualityUpdater.Enabled = false;
                timer_mpsPlayerTextHandler.Enabled = false;
                MpsPlayerTrackCalendarSetAmount(16);
                MpsPlayerTrackCalendarSetCurrentTrack(1);
                MpsPlayerRunningIndicatorStop();
                MpsPlayerInterfaceDebug();
            }
            else
            {
                редактироватьToolStripMenuItem1.Checked = false;
                редактированиеToolStripMenuItem.Checked = false;
                задатьЭлементДляВыравниванияToolStripMenuItem.Enabled = false;
                выровнятьПоВертикалиToolStripMenuItem.Enabled = false;
                выровнятьПоГоризонталиToolStripMenuItem.Enabled = false;
                уменьшитьToolStripMenuItem.Enabled = false;
                увеличитьToolStripMenuItem.Enabled = false;
                выстроитьСогласноСкинуToolStripMenuItem.Enabled = false;
                выстроитьВНулевоеПоложениеToolStripMenuItem.Enabled = false;
                перемещениеToolStripMenuItem.Enabled = false;
                toolStripButton_alignX.Enabled = false;
                toolStripButton_alignY.Enabled = false;
                toolStripButton_zoomIn.Enabled = false;
                toolStripButton_zoomOut.Enabled = false;
                увеличитьВысотуToolStripMenuItem.Enabled = false;
                toolStripButton_heightUp.Enabled = false;
                уменьшитьВысотуToolStripMenuItem.Enabled = false;
                toolStripButton_heightDown.Enabled = false;
                увеличитьДлинуToolStripMenuItem.Enabled = false;
                toolStripButton_widthUp.Enabled = false;
                уменьшитьДлинуToolStripMenuItem.Enabled = false;
                toolStripButton_widthDown.Enabled = false;
                выбранныйЭлементToolStripMenuItem.Enabled = false;
                сдвинутьВсеЭлементыВверхToolStripMenuItem.Enabled = false;
                сдвинутьВсеЭлементыВнизToolStripMenuItem.Enabled = false;
                сдвинутьВсеЭлементыВлевоToolStripMenuItem.Enabled = false;
                сдвинутьВсеЭлементыВправоToolStripMenuItem.Enabled = false;
                if (mpsPlayer_selectedControl != null)
                {
                    PictureBox pb = (PictureBox)mpsPlayer_selectedControl;
                    pb.BorderStyle = BorderStyle.None;
                }
                mpsPlayer_skinEdit = false;
                timer_mpsPlayerHandler.Enabled = true;
                timer_mpsPlayerSpectrumHandler.Enabled = true;
                //timer_mpsPlayerSpectrumUpdater.Enabled = true;
                timer_mpsPlayerTimeUpdater.Enabled = true;
                timer_signalQualityUpdater.Enabled = true;
                timer_mpsPlayerTextHandler.Enabled = true;
                mpsPlayer_currentTrackNumber = 1;
                mpsPlayer_trackCount = 16;
                MpsPlayerInterfaceInitialize();
            }
        }

        private void кодироватьВWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (window_encoder != null)
            {
                window_encoder.Dispose();
                window_encoder = new form_encoder();
            }
            window_encoder.ShowDialog();
        }

        private void записатьНаЛентуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (window_tapeRecordingWizard != null)
            {
                window_tapeRecordingWizard.Dispose();
                window_tapeRecordingWizard = new form_tapeRecordingWizard();
            }
            window_tapeRecordingWizard.ShowDialog();
        }

        private void получитьСЛентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (window_tapeRecoverWizard != null)
            {
                window_tapeRecoverWizard.Dispose();
                window_tapeRecoverWizard = new form_tapeRecoverWizard();
            }
            window_tapeRecoverWizard.ShowDialog();
        }

        private void toolStripButton_alternateScreen_Click(object sender, EventArgs e)
        {
            if (!toolStripButton_alternateScreen.Checked)
            {
                toolStripButton_alternateScreen.Checked = true;
                альтернативныйЭкранToolStripMenuItem.Checked = true;
                mpsPlayer_tapeSkin = true;
                if (form_main.mpsPlayer_mode == "play") MpsPlayerRunningIndicatorPlay();
                else MpsPlayerRunningIndicatorStop();
            }
            else
            {
                toolStripButton_alternateScreen.Checked = false;
                альтернативныйЭкранToolStripMenuItem.Checked = false;
                mpsPlayer_tapeSkin = false;
                if (form_main.mpsPlayer_mode == "play") MpsPlayerRunningIndicatorPlay();
            }
            MpsPlayerInterfaceInitialize();
        }

        private void toolStripButton_remainingTime_Click(object sender, EventArgs e)
        {
            if (!toolStripButton_remainingTime.Checked)
            {
                toolStripButton_remainingTime.Checked = true;
                оставшеесяВремяToolStripMenuItem.Checked = true;
                mpsPlayer_remainingTime = true;
            }
            else
            {
                toolStripButton_remainingTime.Checked = false;
                оставшеесяВремяToolStripMenuItem.Checked = false;
                mpsPlayer_remainingTime = false;
            }
            if (form_main.mpsPlayer_mode == "play" && mpsPlayer_tapeSkin == true) MpsPlayerRunningIndicatorPlay();
        }

        private void toolStripButton_invert_Click(object sender, EventArgs e)
        {
            if (!toolStripButton_invert.Checked)
            {
                toolStripButton_invert.Checked = true;
                инвертироватьСигналToolStripMenuItem.Checked = true;
                AudioIO.audio_invertSignal = true;
            }
            else
            {
                toolStripButton_invert.Checked = false;
                инвертироватьСигналToolStripMenuItem.Checked = false;
                AudioIO.audio_invertSignal = false;
            }
        }

        private void альтернативныйЭкранToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            if (!альтернативныйЭкранToolStripMenuItem.Checked)
            {
                toolStripButton_alternateScreen.Checked = true;
                альтернативныйЭкранToolStripMenuItem.Checked = true;
                mpsPlayer_tapeSkin = true;
                if (form_main.mpsPlayer_mode == "play") MpsPlayerRunningIndicatorPlay();
                else MpsPlayerRunningIndicatorStop();

            }
            else
            {
                toolStripButton_alternateScreen.Checked = false;
                альтернативныйЭкранToolStripMenuItem.Checked = false;
                mpsPlayer_tapeSkin = false;
                if (form_main.mpsPlayer_mode == "play") MpsPlayerRunningIndicatorPlay();
            }
            MpsPlayerInterfaceInitialize();
        }

        private void инвертироватьСигналToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!инвертироватьСигналToolStripMenuItem.Checked)
            {
                toolStripButton_invert.Checked = true;
                инвертироватьСигналToolStripMenuItem.Checked = true;
                AudioIO.audio_invertSignal = true;
            }
            else
            {
                toolStripButton_invert.Checked = false;
                инвертироватьСигналToolStripMenuItem.Checked = false;
                AudioIO.audio_invertSignal = false;
            }
        }

        private void оставшеесяВремяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!оставшеесяВремяToolStripMenuItem.Checked)
            {
                toolStripButton_remainingTime.Checked = true;
                оставшеесяВремяToolStripMenuItem.Checked = true;
                mpsPlayer_remainingTime = true;
            }
            else
            {
                toolStripButton_remainingTime.Checked = false;
                оставшеесяВремяToolStripMenuItem.Checked = false;
                mpsPlayer_remainingTime = false;
            }
            if (form_main.mpsPlayer_mode == "play" && mpsPlayer_tapeSkin == true) MpsPlayerRunningIndicatorPlay();
        }

        private void toolStripButton_rec_Click(object sender, EventArgs e)
        {
            записатьНаЛентуToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_get_Click(object sender, EventArgs e)
        {
            получитьСЛентыToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_opus_Click(object sender, EventArgs e)
        {
            if (window_encoder != null)
            {
                window_encoder.Dispose();
                window_encoder = new form_encoder();
            }
            window_encoder.ShowDialog();
        }

        private void toolStripButton_settings_Click(object sender, EventArgs e)
        {
            настройкиToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_debug_Click(object sender, EventArgs e)
        {
            отладкаToolStripMenuItem_Click(this, null);
        }

        private void mPSOPUSВоспроизведениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Decoder.decoderActive)
            {
                AudioIO.GetPlayDevices();
                Decoder.decoderActive = true;
                AudioIO.SignalCaptureInit();
                DataHandler.StartMp3Listening();
                Decoder.Start();
                mpsPlayer_liveSpectrum = new int[] { 6, 5, 3, 1, 2, 1, 3, 4, 3, 2, 3, 5, 6 };
                timer_mpsPlayerHandler.Enabled = true;
                timer_mpsPlayerSpectrumHandler.Enabled = true;
                timer_mpsPlayerSpectrumUpdater.Enabled = true;
                timer_mpsPlayerTimeUpdater.Enabled = true;
                timer_signalQualityUpdater.Enabled = true;
                timer_mpsPlayerTextHandler.Enabled = true;
                mpsPlayer_currentTrackNumber = 1;
                mpsPlayer_trackCount = 16;
                DataHandler.fs_decodedAudio = new FileStream("DecodedAudio.wav", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                DataHandler.fs_decodedAudio.Seek(44, SeekOrigin.Begin);
            }
        }

        private void mPSOPUSОстановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Decoder.decoderActive = false;
            Decoder.Stop();
            AudioIO.SignalCaptureClose();
            DataHandler.StopMp3Listening();
            MpsPlayerRunningIndicatorStop();
            MpsPlayerTrackCalendarSetAmount(16);
            MpsPlayerTrackCalendarSetCurrentTrack(0);
            timer_mpsPlayerHandler.Enabled = false;
            timer_mpsPlayerSpectrumHandler.Enabled = false;
            timer_mpsPlayerSpectrumUpdater.Enabled = false;
            timer_mpsPlayerTimeUpdater.Enabled = false;
            timer_signalQualityUpdater.Enabled = false;
            Decoder.ClearBuffers();
            MpsPlayerInterfaceInitialize();
            DataHandler.fs_decodedAudio.Seek(0, SeekOrigin.Begin);
            Encoder.WriteHeader(DataHandler.fs_decodedAudio, 48000, 1);
            DataHandler.fs_decodedAudio.Close();
            DataHandler.fs_decodedAudio.Dispose();
        }

        private void toolStripButton_play_Click(object sender, EventArgs e)
        {
            mPSOPUSВоспроизведениеToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_stop_Click(object sender, EventArgs e)
        {
            mPSOPUSОстановитьToolStripMenuItem_Click(this, null);
        }

        private void pictureBox_mpsPlayer_MouseDown(object sender, MouseEventArgs e)
        {
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(pictureBox_mpsPlayer.Controls.OfType<PictureBox>());
            int id = -1;
            foreach (PictureBox control in controls)
            {
                if ((e.Location.X >= control.Location.X && e.Location.X <= (control.Location.X + control.Width)) && (e.Location.Y >= control.Location.Y && e.Location.Y <= (control.Location.Y + control.Height))) id = controls.IndexOf(control);
            }
        }

        private void pictureBox_control_MouseDown(object sender, MouseEventArgs e)
        {
            if (mpsPlayer_skinEdit) 
            {
                PictureBox pb1 = (PictureBox)sender;
                PictureBox pb0 = null;
                if (mpsPlayer_selectedControl != null) pb0 = (PictureBox)mpsPlayer_selectedControl; else pb0 = pb1;
                mpsPlayer_control = sender;                
                mpsPlayer_selectedControl = sender;
                pb1.BorderStyle = BorderStyle.FixedSingle;
                if (pb1.Name != pb0.Name) pb0.BorderStyle = BorderStyle.None;
            }
        }

        private void pictureBox_control_MouseUp(object sender, MouseEventArgs e)
        {
            mpsPlayer_control = null;
        }

        private void редактированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            редактироватьToolStripMenuItem1_Click(this, null);
        }

        private void задержкаПиковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!задержкаПиковToolStripMenuItem.Checked)
            {
                задержкаПиковToolStripMenuItem.Checked = true;
                mpsPlayer_spectrumMode = "peakHold";
            }
            else
            {
                задержкаПиковToolStripMenuItem.Checked = false;
                mpsPlayer_spectrumMode = "noPeak";
            }
        }

        private void отображатьСпектрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!отображатьСпектрToolStripMenuItem.Checked)
            {
                отображатьСпектрToolStripMenuItem.Checked = true;
                if (задержкаПиковToolStripMenuItem.Checked) mpsPlayer_spectrumMode = "peakHold";
                else mpsPlayer_spectrumMode = "noPeak";
            }
            else
            {
                отображатьСпектрToolStripMenuItem.Checked = false;
                mpsPlayer_spectrumMode = "off";
            }
        }

        private void задатьЭлементДляВыравниванияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mpsPlayer_alignmentControl = mpsPlayer_selectedControl;
        }

        private void выровнятьПоВертикалиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mpsPlayer_selectedControl != null && mpsPlayer_alignmentControl != null)
            {
                PictureBox selectedPB = (PictureBox)mpsPlayer_selectedControl;
                PictureBox alignmentPB = (PictureBox)mpsPlayer_alignmentControl;
                selectedPB.Location = new Point(alignmentPB.Location.X, selectedPB.Location.Y);
            }
        }

        private void выровнятьПоГоризонталиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mpsPlayer_selectedControl != null && mpsPlayer_alignmentControl != null)
            {
                PictureBox selectedPB = (PictureBox)mpsPlayer_selectedControl;
                PictureBox alignmentPB = (PictureBox)mpsPlayer_alignmentControl;
                selectedPB.Location = new Point(selectedPB.Location.X, alignmentPB.Location.Y);
            }
        }

        private void перемещениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!перемещениеToolStripMenuItem.Checked)
            {
                перемещениеToolStripMenuItem.Checked = true;
                mpsPlayer_controlFree = true;
            }
            else
            {
                перемещениеToolStripMenuItem.Checked = false;
                mpsPlayer_controlFree = false;
            }
        }

        private void выстроитьСогласноСкинуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            class_mpsPlayerSkinHandler.Load();
        }

        private void выстроитьВНулевоеПоложениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Location = new Point(0, 0);
        }

        private void увеличитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)mpsPlayer_selectedControl;
            pb.Size = new Size(pb.Width + 1, pb.Height + 1);
        }

        private void уменьшитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)mpsPlayer_selectedControl;
            pb.Size = new Size(pb.Width - 1, pb.Height - 1);
        }

        private void toolStripButton_alignX_Click(object sender, EventArgs e)
        {
            выровнятьПоГоризонталиToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_alignY_Click(object sender, EventArgs e)
        {
            выровнятьПоВертикалиToolStripMenuItem_Click(this, null);
        }

        private void выровнятьПоГоризонталиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            выровнятьПоГоризонталиToolStripMenuItem_Click(this, null);
        }

        private void выровнятьПоВертикалиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            выровнятьПоВертикалиToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_zoomOut_Click(object sender, EventArgs e)
        {
            уменьшитьToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_zoomInAll_Click(object sender, EventArgs e)
        {
            увеличитьВсеToolStripMenuItem_Click(this, null);
        }

        private void увеличитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            увеличитьToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_zoomOutAll_Click(object sender, EventArgs e)
        {
            уменьшитьВсеToolStripMenuItem_Click(this, null);
        }

        private void уменьшитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            уменьшитьToolStripMenuItem_Click(this, null);
        }

        private void увеличитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Size = new Size(control.Width + 1, control.Height + 1);
        }

        private void уменьшитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Size = new Size(control.Width - 1, control.Height - 1);
        }

        private void toolStripButton_zoomIn_Click(object sender, EventArgs e)
        {
            увеличитьToolStripMenuItem_Click(this, null);
        }

        private void увеличитьВсеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            увеличитьВсеToolStripMenuItem_Click(this, null);
        }

        private void уменьшитьВсеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            уменьшитьВсеToolStripMenuItem_Click(this, null);
        }

        private void увеличитьВысотуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)mpsPlayer_selectedControl;
            pb.Size = new Size(pb.Width, pb.Height + 1);
        }

        private void уменьшитьВысотуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)mpsPlayer_selectedControl;
            pb.Size = new Size(pb.Width, pb.Height - 1);
        }

        private void увеличитьДлинуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)mpsPlayer_selectedControl;
            pb.Size = new Size(pb.Width + 1, pb.Height);
        }

        private void уменьшитьДлинуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)mpsPlayer_selectedControl;
            pb.Size = new Size(pb.Width - 1, pb.Height);
        }

        private void увеличитьВысотуToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            увеличитьВысотуToolStripMenuItem_Click(this, null);
        }

        private void уменьшитьВысотуToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            уменьшитьВысотуToolStripMenuItem_Click(this, null);
        }

        private void увеличитьДлинуToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            увеличитьДлинуToolStripMenuItem_Click(this, null);
        }

        private void уменьшитьДлинуToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            уменьшитьДлинуToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_heightUp_Click(object sender, EventArgs e)
        {
            увеличитьВысотуToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_heightDown_Click(object sender, EventArgs e)
        {
            уменьшитьВысотуToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_widthUp_Click(object sender, EventArgs e)
        {
            увеличитьДлинуToolStripMenuItem_Click(this, null);
        }

        private void toolStripButton_widthDown_Click(object sender, EventArgs e)
        {
            уменьшитьДлинуToolStripMenuItem_Click(this, null);
        }

        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            class_mpsPlayerSkinHandler.WriteLayout();
        }

        private void сохранитьСкинToolStripMenuItem_Click(object sender, EventArgs e)
        {
            сохранитьToolStripMenuItem1_Click(this, null);
        }

        private void timer_mpsPlayerTextHandler_Tick(object sender, EventArgs e)
        {
            if (mpsPlayer_MPSTextLastArtist != new string(DataHandler.artist) || mpsPlayer_MPSTextLastTrack != new string(DataHandler.track)) mpsPlayer_MPSTextChange = true;
            mpsPlayer_MPSTextLastArtist = new string(DataHandler.artist);
            mpsPlayer_MPSTextLastTrack = new string(DataHandler.track);
            string change = mpsPlayer_MPSTextLastArtist + " - " + mpsPlayer_MPSTextLastTrack;
            //for (int i = 0; i < 20; i++) mpsPlayer_MPSTextMatrix[i] = mpsPlayer_MPSTextLastTrack[i];
            if (mpsPlayer_MPSTextChange)
            {
                DataHandler.textUpdatePause = true;
                mpsPlayer_MPSTextMatrix[0] = mpsPlayer_MPSTextMatrix[1];
                mpsPlayer_MPSTextMatrix[1] = mpsPlayer_MPSTextMatrix[2];
                mpsPlayer_MPSTextMatrix[2] = mpsPlayer_MPSTextMatrix[3];
                mpsPlayer_MPSTextMatrix[3] = mpsPlayer_MPSTextMatrix[4];
                mpsPlayer_MPSTextMatrix[4] = mpsPlayer_MPSTextMatrix[5];
                mpsPlayer_MPSTextMatrix[5] = mpsPlayer_MPSTextMatrix[6];
                mpsPlayer_MPSTextMatrix[6] = mpsPlayer_MPSTextMatrix[7];
                mpsPlayer_MPSTextMatrix[7] = mpsPlayer_MPSTextMatrix[8];
                mpsPlayer_MPSTextMatrix[8] = mpsPlayer_MPSTextMatrix[9];
                mpsPlayer_MPSTextMatrix[9] = mpsPlayer_MPSTextMatrix[10];
                mpsPlayer_MPSTextMatrix[10] = mpsPlayer_MPSTextMatrix[11];
                mpsPlayer_MPSTextMatrix[11] = mpsPlayer_MPSTextMatrix[12];
                mpsPlayer_MPSTextMatrix[12] = mpsPlayer_MPSTextMatrix[13];
                mpsPlayer_MPSTextMatrix[13] = mpsPlayer_MPSTextMatrix[14];
                mpsPlayer_MPSTextMatrix[14] = mpsPlayer_MPSTextMatrix[15];
                mpsPlayer_MPSTextMatrix[15] = mpsPlayer_MPSTextMatrix[16];
                mpsPlayer_MPSTextMatrix[16] = mpsPlayer_MPSTextMatrix[17];
                mpsPlayer_MPSTextMatrix[17] = mpsPlayer_MPSTextMatrix[18];
                mpsPlayer_MPSTextMatrix[18] = mpsPlayer_MPSTextMatrix[19];
                if (mpsPlayer_MPSTextChangeCount < change.Length)
                {
                    mpsPlayer_MPSTextMatrix[19] = change[mpsPlayer_MPSTextChangeCount];
                }
                mpsPlayer_MPSTextChangeCount++;
                if (mpsPlayer_MPSTextChangeCount >= change.Length + 16) { DataHandler.textUpdatePause = false; mpsPlayer_MPSTextChange = false; for (int i = 0; i < 20; i++) mpsPlayer_MPSTextMatrix[i] = mpsPlayer_MPSTextLastTrack[i]; mpsPlayer_MPSTextChangeCount = 0; }
            }
            for (int i = 0; i < 20; i++)
            {
                switch (mpsPlayer_MPSTextMatrix[i])
                {
                    default:
                        pictureBox_textSymbols[i].Image = null;
                        break;
                    case '1':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\1symbol.png")];
                        break;
                    case '2':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\2symbol.png")];
                        break;
                    case '3':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\3symbol.png")];
                        break;
                    case '4':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\4symbol.png")];
                        break;
                    case '5':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\5symbol.png")];
                        break;
                    case '6':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\6symbol.png")];
                        break;
                    case '7':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\7symbol.png")];
                        break;
                    case '8':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\8symbol.png")];
                        break;
                    case '9':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\9symbol.png")];
                        break;
                    case '0':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\0symbol.png")];
                        break;
                    case 'a':
                    case 'A':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Asymbol.png")];
                        break;
                    case 'b':
                    case 'B':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Bsymbol.png")];
                        break;
                    case 'c':
                    case 'C':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Csymbol.png")];
                        break;
                    case 'd':
                    case 'D':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Dsymbol.png")];
                        break;
                    case 'e':
                    case 'E':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Esymbol.png")];
                        break;
                    case 'f':
                    case 'F':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Fsymbol.png")];
                        break;
                    case 'g':
                    case 'G':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Gsymbol.png")];
                        break;
                    case 'h':
                    case 'H':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Hsymbol.png")];
                        break;
                    case 'i':
                    case 'I':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Isymbol.png")];
                        break;
                    case 'j':
                    case 'J':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Jsymbol.png")];
                        break;
                    case 'k':
                    case 'K':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Ksymbol.png")];
                        break;
                    case 'l':
                    case 'L':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Lsymbol.png")];
                        break;
                    case 'm':
                    case 'M':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Msymbol.png")];
                        break;
                    case 'n':
                    case 'N':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Nsymbol.png")];
                        break;
                    case 'o':
                    case 'O':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Osymbol.png")];
                        break;
                    case 'p':
                    case 'P':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Psymbol.png")];
                        break;
                    case 'q':
                    case 'Q':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Qsymbol.png")];
                        break;
                    case 'r':
                    case 'R':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Rsymbol.png")];
                        break;
                    case 's':
                    case 'S':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Ssymbol.png")];
                        break;
                    case 't':
                    case 'T':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Tsymbol.png")];
                        break;
                    case 'u':
                    case 'U':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Usymbol.png")];
                        break;
                    case 'v':
                    case 'V':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Vsymbol.png")];
                        break;
                    case 'w':
                    case 'W':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Wsymbol.png")];
                        break;
                    case 'x':
                    case 'X':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Xsymbol.png")];
                        break;
                    case 'y':
                    case 'Y':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Ysymbol.png")];
                        break;
                    case 'z':
                    case 'Z':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\Zsymbol.png")];
                        break;
                    case '-':
                    case '_':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\DASHsymbol.png")];
                        break;
                    case '*':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\ASTERISKsymbol.png")];
                        break;
                    case '/':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\SLASHsymbol.png")];
                        break;
                    case '(':
                    case '[':
                    case '{':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\OPENPARENTHESISsymbol.png")];
                        break;
                    case ')':
                    case ']':
                    case '}':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\CLOSEPARENTHESISsymbol.png")];
                        break;
                    case ',':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\COMMAsymbol.png")];
                        break;
                    case '|':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\PIPEsymbol.png")];
                        break;
                    case '+':
                        pictureBox_textSymbols[i].Image = class_mpsPlayerSkinHandler.image_symbols[Array.IndexOf(class_mpsPlayerSkinHandler.symbols, "Symbols\\PLUSsymbol.png")];
                        break;
                }
            }
        }

        private void списокИзмененийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Location = new Point(control.Location.X + 10, control.Location.Y);
        }

        private void сдвинутьВсеЭлементыВправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Location = new Point(control.Location.X + 10, control.Location.Y);
            spectrumBarX0P += 10;
            
            //foreach (PictureBox control in pictureBox_textSymbols) control.Location = new Point(control.Location.X + 1, control.Location.Y);
        }

        private void сдвинутьВсеЭлементыВлевоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Location = new Point(control.Location.X - 10, control.Location.Y);
            spectrumBarX0P -= 10;
            
            //foreach (PictureBox control in pictureBox_textSymbols) control.Location = new Point(control.Location.X - 1, control.Location.Y);
        }

        private void сдвинутьВсеЭлементыВверхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Location = new Point(control.Location.X, control.Location.Y - 10);
            spectrumBarY0P -= 10;
            
            //foreach (PictureBox control in pictureBox_textSymbols) control.Location = new Point(control.Location.X, control.Location.Y - 1);
        }

        private void сдвинутьВсеЭлементыВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            List<PictureBox> controls = new List<PictureBox>();
            controls.AddRange(tabPage_graphicalView.Controls.OfType<PictureBox>());
            foreach (PictureBox control in controls) control.Location = new Point(control.Location.X, control.Location.Y + 10);
            spectrumBarY0P += 10;
            
            //foreach (PictureBox control in pictureBox_textSymbols) control.Location = new Point(control.Location.X, control.Location.Y + 1);
        }
    }
}
