using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class EncoderWindow : Form
    {
        public EncoderWindow()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            //Доступность и видимость контролов
            if (richTextBox.Text != "" || textBox.Text != "")
                button_convert.Enabled = true;
            else
                button_convert.Enabled = false;
            if (ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStream) == "Running")
            {
                button_clear.Enabled = false;
                button_convert.Enabled = false;
                button_select.Enabled = false;
                label_encoding.Visible = true;
                progressBar.Visible = true;
                label_percent.Visible = true;
            }
            else
            {
                button_clear.Enabled = true;
                button_select.Enabled = true;
                label_encoding.Visible = false;
                progressBar.Visible = false;
                label_percent.Visible = false;
            }

            //Обновление прогресс бара
            progressBar.Value = Encoder.encoder_progress;
            label_percent.Text = Encoder.encoder_progress.ToString() + "%";
        }

        private void button_select_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog.ShowDialog();
                Encoder.encoder_inputFilePath = openFileDialog.FileName;
                if (Encoder.encoder_inputFilePath != "" && Encoder.encoder_inputFilePath != null)
                    textBox.Text = Encoder.encoder_inputFilePath;
            }
            catch (Exception ex)
            {
                LogHandler.WriteError("EncoderWindow.cs->button_select_Click", ex.Message);
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            richTextBox.Text = "";
            textBox.Text = "";          
        }

        private void button_convert_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton_file.Checked == true)
                {
                    if (Encoder.encoder_ADIFShell == false)
                    {
                        folderBrowserDialog.ShowDialog();
                        Encoder.encoder_outputFilePath = folderBrowserDialog.SelectedPath + "\\" + Path.GetFileNameWithoutExtension(Encoder.encoder_inputFilePath) + ".wav";
                        if (Encoder.encoder_outputFilePath == "" || Encoder.encoder_outputFilePath == null)
                            Encoder.encoder_outputFilePath = "output.wav";
                        Encoder.thread_encodeFileStream = new Thread(Encoder.EncodeFileStream);
                        Encoder.thread_encodeFileStream.Start();
                    }
                    else { }
                }
                else { }
            }
            catch (Exception ex)
            {
                LogHandler.WriteError("EncoderWindow.cs->button_convert_Click", ex.Message);
            }
        }

        private void radioButton_text_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

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

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStream) == "Running")
                Encoder.encoder_forceStop = true;
        }

        private void EncoderWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MainWindow.class_debugWindow != null)
                MainWindow.class_debugWindow.Close();
        }

        private void EncoderWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
