using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class CaptureWindow : Form
    {
        //Графика
        //Префикс: "graphics_; bitmap_"
        //////////////////////////////////////////////////////////////////////////////////////
        static Graphics graphics_waveGraph = null;
        static Bitmap bitmap_waveGraph = null;
        //////////////////////////////////////////////////////////////////////////////////////
        
        public CaptureWindow()
        {
            InitializeComponent();
        }

        private void CaptureWindow_Load(object sender, EventArgs e)
        {
            bitmap_waveGraph = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics_waveGraph = Graphics.FromImage(bitmap_waveGraph); //Инициализация графики
            try
            {
                comboBox_recDevices.Items.AddRange(AudioIO.GetRecDevices());
                comboBox_recDevices.Text = AudioIO.GetRecDevices()[AudioIO.audio_recDeviceId];
            }
            catch 
            {
                Properties.Settings.Default.Reset();
                Settings.Load();
            }
            AudioIO.GraphCaptureInit();
            AudioIO.SignalCaptureInit();
            Decoder.Start();
        }

        /// <summary>
        /// Отрисовка кадра сигналограммы
        /// </summary>
        public void DrawWaveGraphFrame()
        {
            if (this.WindowState != FormWindowState.Minimized && AudioIO.buff_graphSamples.Count > 0)
            {
                try
                {
                    graphics_waveGraph.Clear(Color.LightGray);
                    PointF[] points = new PointF[pictureBox.Width]; //Массив точек кадра сигналограммы
                    graphics_waveGraph.DrawLine(new Pen(Color.Green), 0, pictureBox.Height / 2, pictureBox.Width, pictureBox.Height / 2); //Отрисовать горизонтальную ось
                    for (int i = 0, k = 0; i < pictureBox.Width; i++, k += MainWindow.class_captureWindow.trackBar.Value)
                        points[i] = new PointF(i, (((pictureBox.Height / 2) * -AudioIO.buff_graphSamples[k]) / 32767) + (pictureBox.Height / 2));
                    graphics_waveGraph.DrawLines(new Pen(Color.Blue), points);
                    pictureBox.Image = bitmap_waveGraph;
                    AudioIO.buff_graphSamples.RemoveRange(0, AudioIO.buff_graphSamples.Count - 512);
                }
                catch (Exception ex)
                {
                    LogHandler.WriteError("CaptureWindow.cs->DrawGraphFrame()", ex.Message);
                }
            }
        }

        private void CaptureWindow_ResizeEnd(object sender, EventArgs e)
        {
            bitmap_waveGraph = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics_waveGraph = Graphics.FromImage(bitmap_waveGraph); //Инициализация графики
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

        private void button_ok_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CaptureWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIO.GraphCaptureClose();
            AudioIO.SignalCaptureClose();
            if (MainWindow.class_debugWindow != null)
                MainWindow.class_debugWindow.Close();
        }

        private void button_debug_Click(object sender, EventArgs e)
        {
            if (MainWindow.class_debugWindow != null)
            {
                MainWindow.class_debugWindow.Dispose();
                MainWindow.class_debugWindow = new DebugWindow();
            }
            MainWindow.class_debugWindow.Show();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {

        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {

        }
    }
}
