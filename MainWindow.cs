using NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
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
    public partial class MainWindow : Form
    {
        //Экземпляры классов
        //Префикс: "class_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static MainWindow class_mainWindow;
        public static TextHandler class_TextHandler = new TextHandler();
        public static AudioIO class_audioIO = new AudioIO();
        public static CaptureWindow class_captureWindow = new CaptureWindow();
        public static DebugWindow class_debugWindow = new DebugWindow();
        public static EncoderWindow class_encoderWindow = new EncoderWindow();
        public static LogMonitorWindow class_logMonitorWindow = new LogMonitorWindow();
        //////////////////////////////////////////////////////////////////////////////////////

        public MainWindow()
        {
            InitializeComponent();
            class_mainWindow = this; //Передача статического доступа классу
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Settings.Load();
        }

        private void button_capture_Click(object sender, EventArgs e)
        {
            if (class_captureWindow != null)
                class_captureWindow = new CaptureWindow();
            class_captureWindow.Show();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save();
        }

        private void button_encoder_Click(object sender, EventArgs e)
        {
            if (class_encoderWindow != null)
                class_encoderWindow = new EncoderWindow();
            class_encoderWindow.ShowDialog();
        }
    }
}
