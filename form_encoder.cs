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


namespace AudioDataInterface
{
    public partial class form_encoder : Form
    {
        public static int trackNumber = 1;
        public static int trackCount = 1;
        public form_encoder()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            //Доступность и видимость контролов
            if (ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStereoStream) == "Running")
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

        private void button_convert_Click(object sender, EventArgs e)
        {
            try
            {
                if (Encoder.encoder_ADIFShell == false)
                {
                    folderBrowserDialog.ShowDialog();
                    /*
                    if (Path.GetExtension(Encoder.encoder_inputFilePath) == ".jpg")
                    {
                        try
                        {
                            //Конверсия изображения
                            var img = new MagickImage(Encoder.encoder_inputFilePath);
                            img.Format = MagickFormat.Jpg;
                            img.Strip(); //Убрать мета-данные
                            if (img.Height > 600 && img.Width > 800) img.Resize(800, 600);
                            img.Quality = 50;
                            img.Settings.ColorSpace = ColorSpace.RGB;
                            img.Settings.Interlace = Interlace.Jpeg;
                            img.Settings.Compression = CompressionMethod.JPEG2000;
                            img.Write("temp.jpg");
                            img.Dispose();
                            FileInfo fi = new FileInfo("temp.jpg");
                            Encoder.encoder_inputFilePath = "temp.jpg";
                            Encoder.encoder_mode = "jpeg";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else Encoder.encoder_mode = "mp3";
                    */
                    Encoder.encoder_outputFilePath = folderBrowserDialog.SelectedPath + "\\" + Path.GetFileNameWithoutExtension(Encoder.encoder_inputFilePath) + ".wav";
                    Encoder.encoder_mode = "mp3";
                    
                    if (File.Exists("output.mp3")) File.Delete("output.mp3");
                    if (File.Exists("output2.mp3")) File.Delete("output2.mp3");
                    string secondCmd = "ffmpeg -i output.mp3 " + Encoder.encoder_ffmpeg2Cmd + " -af " + @"""" + Encoder.encoder_ffmpeg2EffectCmd + @"""" + " output2.mp3";
                    string firstCmd = "/C ffmpeg -i " + @"""" + Encoder.encoder_inputFilePath + @"""" + " " + Encoder.encoder_ffmpeg1Cmd + " output.mp3";
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = "cmd.exe";
                    if (Encoder.encoder_ffmpeg2EffectCmd != "" && Encoder.encoder_ffmpeg2Cmd != "") p.StartInfo.Arguments = firstCmd + " && " + secondCmd; else p.StartInfo.Arguments = firstCmd;
                    p.StartInfo.CreateNoWindow = false;
                    p.Start();
                    p.WaitForExit();
                    if (File.Exists("output2.mp3"))
                    {
                        Encoder.encoder_inputFilePath = "output2.mp3";
                        if (Encoder.encoder_outputFilePath == "" || Encoder.encoder_outputFilePath == null) Encoder.encoder_outputFilePath = "output.wav";
                        if (Encoder.encoder_longLeadIn == false) Encoder.encoder_leadInSubcodesAmount = Encoder.encoder_leadInOutSubcodesAmount; else Encoder.encoder_leadInSubcodesAmount = 2000;
                        Encoder.thread_encodeFileStereoStream = new Thread(Encoder.EncodeFileStereoStream);
                        Encoder.thread_encodeFileStereoStream.Start();
                    }
                    else
                    {
                        if (File.Exists("output.mp3"))
                        {
                            Encoder.encoder_inputFilePath = "output.mp3";
                            if (Encoder.encoder_outputFilePath == "" || Encoder.encoder_outputFilePath == null) Encoder.encoder_outputFilePath = "output.wav";
                            if (Encoder.encoder_longLeadIn == false) Encoder.encoder_leadInSubcodesAmount = Encoder.encoder_leadInOutSubcodesAmount; else Encoder.encoder_leadInSubcodesAmount = 2000;
                            Encoder.thread_encodeFileStereoStream = new Thread(Encoder.EncodeFileStereoStream);
                            Encoder.thread_encodeFileStereoStream.Start();
                        }
                        else MessageBox.Show("Не удалось выполнить преобразование указанного файла. Проверьте исходный файл и команды FFMPEG!", "FFMPEG ERROR");
                    }
                    
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
