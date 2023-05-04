using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        //////////////////////////////////////////////////////////////////////////////////////

        public static form_main window_main;

        //Графика
        //Префикс: "graphics_; bitmap_"
        //////////////////////////////////////////////////////////////////////////////////////
        static Graphics graphics_waveGraph = null;
        static Bitmap bitmap_waveGraph = null;
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

        string[] decodedData = null;
        bool worker_timerControlHandler = false;

        public form_main()
        {
            InitializeComponent();
            window_main = this; //Передача статического доступа классу
            scope_verticalBIAS = pictureBox_waveGraph.Height / 2;
            Type type = listView.GetType();
            PropertyInfo propertyInfo = type.GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo.SetValue(listView, true, null);
            decodedData = new string[10];
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
                    graphics_waveGraph.Clear(Color.FromArgb(34,31,31));                   
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
                    graphics_waveGraph.DrawLines(new Pen(Color.FromArgb(153,255,153)), points);
                    points = new PointF[pointsCount];
                    for (int i = 0, k = scope_horizontalBIAS, x = 0; i < pointsCount; i += 1, k += scope_additionalHorizontalScale, x += scope_horizontalScale) points[i] = new PointF(x, (((pictureBox_waveGraph.Height / 2) * -AudioIO.buff_graphSamples[k]) / scope_verticalScale) + scope_verticalBIAS - 1);
                    graphics_waveGraph.DrawLines(new Pen(Color.FromArgb(153, 255, 153)), points);
                    pictureBox_waveGraph.Image = bitmap_waveGraph;
                    AudioIO.buff_graphSamples.RemoveRange(0, AudioIO.buff_graphSamples.Count);
                }
                catch (Exception ex)
                {
                    LogHandler.WriteError("CaptureWindow.cs->DrawGraphFrame()", ex.Message);
                }
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Settings.Load();

            bitmap_waveGraph = new Bitmap(pictureBox_waveGraph.Width, pictureBox_waveGraph.Height);
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

        private void button_capture_Click(object sender, EventArgs e)
        {

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioIO.GraphCaptureClose();
            AudioIO.SignalCaptureClose();
            if (window_debug != null)
                window_debug.Close();
            Settings.Save();
            Close();
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

            /*
            if (Decoder.buff_decodedData.Count() > 17 && worker_timerControlHandler == false)
            {
                worker_timerControlHandler = true;
                int m = 0;
                if (Decoder.buff_decodedData.Count() >= 16)
                    m = 16;
             
                for (int i = 0; i < m; i++)
                {
                    Decoder.buff_decodedData[i].CopyTo(decodedData, 0);
                    listView.Items.Add(listView.Items.Count.ToString());
                    listView.Items[listView.Items.Count - 1].SubItems.Add(decodedData[8]);
                    listView.Items[listView.Items.Count - 1].SubItems.Add("-");
                    listView.Items[listView.Items.Count - 1].SubItems.Add("-");
                    listView.Items[listView.Items.Count - 1].SubItems.Add(decodedData[9]);
                    //listView.Items[listView.Items.Count - 1].EnsureVisible();
                }
                Decoder.buff_decodedData.RemoveRange(0, m);
                worker_timerControlHandler = false;
            }
            */
            if (scope_horizontalBIASInc == true) scope_horizontalBIAS -= 1;
            if (scope_horizontalBIASDec == true) scope_horizontalBIAS += 1;
            if (scope_verticalBIASInc == true) scope_verticalBIAS += 2;
            if (scope_verticalBIASDec == true) scope_verticalBIAS -= 2;
            try
            {
                this.Text = class_dataHandler.mp3_message;
                label1.Text = DataHandler.mp3_currentTime.ToString();
                label2.Text = Decoder.maxAmplitude.ToString();
            }
            catch
            {

            }
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

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

        private void toolStripLabel_Click(object sender, EventArgs e)
        {
            TaskListViewUpdate();
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
            bitmap_waveGraph = new Bitmap(pictureBox_waveGraph.Width, pictureBox_waveGraph.Height);
            graphics_waveGraph = Graphics.FromImage(bitmap_waveGraph); //Инициализация графики
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
            DataHandler.StartMp3Listening();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            class_dataHandler.ClearMp3();
        }

        private void groupBox_signalCapture_Enter(object sender, EventArgs e)
        {

        }
    }
}
