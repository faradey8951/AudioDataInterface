using ImageMagick;
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
using System.Diagnostics;
using OpusDotNet;


namespace AudioDataInterface
{
    public partial class form_encoder : Form
    {
        public static int trackNumber = 1;
        public static int trackCount = 1;
        static string status = "";
        static Thread thread_convertWaveToOPUS = new Thread(ConvertWaveToOPUS);
        public form_encoder()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            //Доступность и видимость контролов
            if (ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStereoStream) == "Running" || ThreadHandler.GetThreadStatus(thread_convertWaveToOPUS) == "Running")
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
                button_convert.Enabled = true;
            }
            label_trackNumber.Text = "Номер дорожки: " + trackNumber.ToString();
            label_trackCount.Text = "Количество дорожек: " + trackCount.ToString();
            //Обновление прогресс бара
            progressBar.Value = Encoder.encoder_progress;
            label_percent.Text = Encoder.encoder_progress.ToString() + "%";
            label_encoding.Text = status;
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
            textBox.Text = "";          
        }

        static void ConvertWaveToOPUS()
        {
            status = "Конвертация WAVE -> OPUS...";
            FileStream fs = new FileStream("input.wav", FileMode.Open);
            List<byte> inputPCMBytes = new List<byte>();
            List<short> inputPCMShorts = new List<short>();
            List<byte> outputOpusBytes = new List<byte>();
            DataHandler.ms = new MemoryStream();
            OpusEncoder encoder = new OpusEncoder(OpusDotNet.Application.Audio, 48000, 1);
            OpusDecoder decoder = new OpusDecoder(48000, 1);
            for (int i = 0; i < fs.Length; i++)
            {
                inputPCMBytes.Add((byte)fs.ReadByte());
                if (inputPCMBytes.Count == 20 * (48000 / 1000) * 2)
                {

                    encoder.MaxBandwidth = Bandwidth.FullBand;
                    encoder.Bitrate = 16000;
                    encoder.DTX = true;
                    encoder.VBR = false;
                    encoder.Complexity = 10;
                    byte[] opusBytes = new byte[40];
                    encoder.Encode(inputPCMBytes.ToArray(), inputPCMBytes.Count, opusBytes, opusBytes.Length);
                    outputOpusBytes.AddRange(opusBytes);
                    inputPCMBytes.Clear();
                }
                Encoder.encoder_progress = ProgressHandler.GetPercent(fs.Length, i);
            }
            fs.Close();
            fs = new FileStream("outputOpusEncoded.wav", FileMode.Create);
            fs.Write(outputOpusBytes.ToArray(), 0, outputOpusBytes.Count);
            fs.Close();
            Encoder.encoder_inputFilePath = "outputOpusEncoded.wav";
            if (Encoder.encoder_outputFilePath == "" || Encoder.encoder_outputFilePath == null) Encoder.encoder_outputFilePath = "output.wav";
            status = "Конвертация OPUS -> ADI-OPUS16...";
            Encoder.thread_encodeFileStereoStream = new Thread(Encoder.EncodeFileStereoStream);
            Encoder.thread_encodeFileStereoStream.Start();
        }

        private void button_convert_Click(object sender, EventArgs e)
        {
            //try
            {
                if (Encoder.encoder_ADIFShell == false)
                {
                    folderBrowserDialog.ShowDialog();
                    Encoder.encoder_outputFilePath = folderBrowserDialog.SelectedPath + "\\" + Path.GetFileNameWithoutExtension(Encoder.encoder_inputFilePath) + ".wav";
                    Encoder.encoder_mode = "opus";
                    if (File.Exists("input.wav")) File.Delete("input.wav");
                    if (File.Exists("outputOpusEncoded.wav")) File.Delete("outputOpusEncoded.wav");
                    string firstCmd = "/C ffmpeg -i " + @"""" + Encoder.encoder_inputFilePath + @"""" + " " + "-acodec pcm_s16le -ar 48000 -ac 1 input.wav";
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = firstCmd;
                    p.StartInfo.CreateNoWindow = false;
                    p.Start();
                    p.WaitForExit();
                    if (File.Exists("input.wav"))
                    {
                        if (thread_convertWaveToOPUS != null) thread_convertWaveToOPUS = new Thread(ConvertWaveToOPUS);
                        thread_convertWaveToOPUS.Start();
                    }
                }
                else { }
            }
            //catch (Exception ex)
            {
                //LogHandler.WriteError("EncoderWindow.cs->button_convert_Click", ex.Message);
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
            if (form_main.window_debug != null)
            {
                form_main.window_debug.Dispose();
                form_main.window_debug = new form_debug();
            }
            form_main.window_debug.Show();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStereoStream) == "Running")
                Encoder.encoder_forceStop = true;
        }

        private void EncoderWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (form_main.window_debug != null)
                form_main.window_debug.Close();
        }

        private void EncoderWindow_Load(object sender, EventArgs e)
        {

        }

        private void trackBar_trackNumber_Scroll(object sender, EventArgs e)
        {
            trackNumber = trackBar_trackNumber.Value;
        }

        private void trackBar_trackCount_Scroll(object sender, EventArgs e)
        {
            trackCount = trackBar_trackCount.Value;
        }

        private void checkBox_longLeadIn_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_longLeadIn.Checked) Encoder.encoder_longLeadIn = true; else Encoder.encoder_longLeadIn = false;
        }
    }
}
