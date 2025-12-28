using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class form_debug : Form
    {
        public static List<string> buff_log = new List<string>(); //Буфер ошибок для вывода
        public static List<int> buff_quality = new List<int>();
        public static Graphics graphics_signal = null;
        public static Bitmap bitmap_signal = null;
        public static Graphics graphics_sortedDerivative = null;
        public static Bitmap bitmap_sortedDerivative = null;
        public static Graphics graphics_decodingQuality = null;
        public static Bitmap bitmap_decodingQuality = null;

        public static int avgQuality = 0;

        public form_debug()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            while (buff_log.Count > 0)
            {
                richTextBox.AppendText(buff_log[0]);
                buff_log.RemoveAt(0);
            }
        }

        private void LogMonitorWindow_Load(object sender, EventArgs e)
        {
            LogHandler.logListening = true;
            buff_log.AddRange(LogHandler.list_log); //Вписать list_log в буфер ошибок
            bitmap_signal = new Bitmap(pictureBox_signal.Width, pictureBox_signal.Height);
            graphics_signal = Graphics.FromImage(bitmap_signal);
            bitmap_sortedDerivative = new Bitmap(pictureBox_sortedDerivative.Width, pictureBox_sortedDerivative.Height);
            graphics_sortedDerivative = Graphics.FromImage(bitmap_sortedDerivative);
            bitmap_decodingQuality = new Bitmap(pictureBox_decodingQuality.Width, pictureBox_decodingQuality.Height);
            graphics_decodingQuality = Graphics.FromImage(bitmap_decodingQuality);
            Decoder.debugEnabled = true;
            timer_drawGraph.Enabled = true;
            for (int i = 0; i < 200; i++) buff_quality.Add(0);
        }

        private void LogMonitorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer_drawGraph.Enabled = false;
            LogHandler.logListening = false;
            Decoder.debugEnabled = false;
        }

        private void timer_drawGraph_Tick(object sender, EventArgs e)
        {
            graphics_signal.Clear(Color.FromArgb(34, 31, 31));
            graphics_sortedDerivative.Clear(Color.FromArgb(34, 31, 31));
            graphics_decodingQuality.Clear(Color.FromArgb(34, 31, 31));
            if (Decoder.debug_derivative != null && Decoder.debug_derivative.Length > 0)
            {
                PointF[] points = new PointF[38];
                double[] derivative = Decoder.debug_derivative;
                double[] fixedDerivative = Decoder.debug_fixedDerivative;
                double[] sortedFirstDerivative = Decoder.debug_sortedFirstDerivative;
                double[] sortedSecondDerivative = Decoder.debug_sortedSecondDerivative;
                int delta = pictureBox_signal.Width / derivative.Length;
                int xPoint = delta / 2;
                for (int i = 0; i < derivative.Length; i++, xPoint += delta) { points[i] = new PointF(xPoint, Convert.ToInt16(SwapNumberRange(derivative[i], derivative.Min(), derivative.Max(), 10, pictureBox_signal.Height - 5))); graphics_signal.FillEllipse(new SolidBrush(Color.FromArgb(77,128,77)), points[i].X, points[i].Y, (int)pictureBox_signal.Height / 25, (int)pictureBox_signal.Height / 25); }
                graphics_signal.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                xPoint = delta / 2;
                points = new PointF[38];
                for (int i = 0; i < fixedDerivative.Length; i++, xPoint += delta) { points[i] = new PointF(xPoint, Convert.ToInt16(SwapNumberRange(fixedDerivative[i], derivative.Min(), derivative.Max(), 10, pictureBox_signal.Height - 5))); graphics_signal.FillEllipse(new SolidBrush(Color.FromArgb(127, 145, 19)), points[i].X, points[i].Y, (int)pictureBox_signal.Height / 25, (int)pictureBox_signal.Height / 25); }
                graphics_signal.DrawLines(new Pen(Color.FromArgb(222, 255, 33)), points);
                delta = pictureBox_sortedDerivative.Width / sortedFirstDerivative.Length;
                pictureBox_signal.Image = bitmap_signal;

                xPoint = delta / 2;
                points = new PointF[38];
                for (int i = 0; i < sortedFirstDerivative.Length; i++, xPoint += delta) { points[i] = new PointF(xPoint, Convert.ToInt16(SwapNumberRange(sortedFirstDerivative[i], sortedFirstDerivative.Min(), sortedFirstDerivative.Max(), 10, pictureBox_sortedDerivative.Height - 5))); graphics_sortedDerivative.FillEllipse(new SolidBrush(Color.FromArgb(0, 105, 150)), points[i].X, points[i].Y, (int)pictureBox_sortedDerivative.Height / 25, (int)pictureBox_sortedDerivative.Height / 25); }
                graphics_sortedDerivative.DrawLines(new Pen(Color.FromArgb(0, 162, 232)), points);               
                xPoint = delta / 2;
                points = new PointF[37];
                for (int i = 0; i < sortedSecondDerivative.Length; i++, xPoint += delta) { points[i] = new PointF(xPoint, Convert.ToInt16(SwapNumberRange(sortedSecondDerivative[i], sortedSecondDerivative.Min(), sortedSecondDerivative.Max(), 10, pictureBox_sortedDerivative.Height - 5))); graphics_sortedDerivative.FillEllipse(new SolidBrush(Color.FromArgb(148, 13, 13)), points[i].X, points[i].Y, (int)pictureBox_sortedDerivative.Height / 25, (int)pictureBox_sortedDerivative.Height / 25); }
                graphics_sortedDerivative.DrawLines(new Pen(Color.FromArgb(230, 20, 20)), points);
                pictureBox_sortedDerivative.Image = bitmap_sortedDerivative;

                List<double> sortedSecondDerivativeList = new List<double>();
                sortedSecondDerivativeList.AddRange(sortedSecondDerivative);
                double firstPeak = sortedSecondDerivativeList.Max();
                sortedSecondDerivativeList.Remove(firstPeak);
                double secondPeak = sortedSecondDerivativeList.Max();
                int quality = 100 - (int)((100 * secondPeak) / firstPeak);
                buff_quality.RemoveAt(0);
                buff_quality.Add(quality);

                delta = (int)Math.Ceiling((double)pictureBox_decodingQuality.Width / ((double)buff_quality.Count + 10));
                xPoint = delta / 2;
                points = new PointF[buff_quality.Count];
                for (int i = 0; i < buff_quality.Count; i++, xPoint += delta)
                { 
                    points[i] = new PointF(xPoint, Convert.ToInt16(SwapNumberRange(buff_quality[i], 0, 100, 10, pictureBox_decodingQuality.Height - 5)));
                    if (buff_quality[i] >= 70) graphics_decodingQuality.FillEllipse(new SolidBrush(Color.FromArgb(0, 86, 237)), points[i].X, points[i].Y, (int)pictureBox_decodingQuality.Height / 40, (int)pictureBox_decodingQuality.Height / 40);
                    else graphics_decodingQuality.FillEllipse(new SolidBrush(Color.FromArgb(148, 13, 13)), points[i].X, points[i].Y, (int)pictureBox_decodingQuality.Height / 40, (int)pictureBox_decodingQuality.Height / 40);
                }
                graphics_decodingQuality.DrawLines(new Pen(Color.FromArgb(2, 100, 148)), points);              

                avgQuality = (buff_quality[buff_quality.Count - 1] + buff_quality[buff_quality.Count - 2] + buff_quality[buff_quality.Count - 3] + buff_quality[buff_quality.Count - 4] + buff_quality[buff_quality.Count - 5] + buff_quality[buff_quality.Count - 6]) / 6;
                int greenLevel = (255 * avgQuality) / 100;
                int redLevel = 255 - greenLevel;
                graphics_decodingQuality.DrawLine(new Pen(Color.FromArgb(redLevel, greenLevel, 0)), 0, Convert.ToInt16(SwapNumberRange(avgQuality, 0, 100, 10, pictureBox_decodingQuality.Height - 5)), pictureBox_decodingQuality.Width, Convert.ToInt16(SwapNumberRange(avgQuality, 0, 100, 10, pictureBox_decodingQuality.Height - 5)));
                graphics_decodingQuality.DrawString(avgQuality + "%", new Font("Times New Roman", 8), Brushes.LightYellow, pictureBox_decodingQuality.Width - 50, Convert.ToInt16(SwapNumberRange(avgQuality, 0, 100, 10, pictureBox_decodingQuality.Height - 5)));
                pictureBox_decodingQuality.Image = bitmap_decodingQuality;
            }
        }

        void DrawGraph()
        {
            while (true)
            {

            }
        }

        static double SwapNumberRange(double number, double sourceRangeMin, double sourceRangeMax, double targetRangeMin, double targetRangeMax)
        {
            double delta = (targetRangeMax - targetRangeMin) / (sourceRangeMax - sourceRangeMin);
            double result = number * delta;
            if (result < targetRangeMin) result = targetRangeMin + 5;
            if (result > targetRangeMax) result = targetRangeMax - 5;
            return (int)targetRangeMax-result;
        }

        private void pictureBox_signal_SizeChanged(object sender, EventArgs e)
        {
            bitmap_signal = new Bitmap(pictureBox_signal.Width, pictureBox_signal.Height);
            graphics_signal = Graphics.FromImage(bitmap_signal);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_slow.Checked) timer_drawGraph.Interval = 500;
        }

        private void radioButton_fast_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_fast.Checked) timer_drawGraph.Interval = 100;
        }

        private void pictureBox_sortedDerivative_SizeChanged(object sender, EventArgs e)
        {
            bitmap_sortedDerivative = new Bitmap(pictureBox_sortedDerivative.Width, pictureBox_sortedDerivative.Height);
            graphics_sortedDerivative = Graphics.FromImage(bitmap_sortedDerivative);
        }

        private void pictureBox_decodingQuality_SizeChanged(object sender, EventArgs e)
        {
            bitmap_decodingQuality = new Bitmap(pictureBox_decodingQuality.Width, pictureBox_decodingQuality.Height);
            graphics_decodingQuality = Graphics.FromImage(bitmap_decodingQuality);
        }

        private void checkBox_pause_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_pause.Checked) timer_drawGraph.Enabled = false;
            else timer_drawGraph.Enabled = true;
        }

        private void checkBox_clipping_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_clipping.Checked) Decoder.debug_clipper = true;
            else Decoder.debug_clipper = false;
        }
    }
}
