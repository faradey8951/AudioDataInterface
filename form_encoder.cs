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
//using Concentus;
//using Concentus.Celt;
//using Concentus.Common.CPlusPlus;
//using Concentus.Enums;
//using Concentus.Structs;


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
                        FileStream fs = new FileStream("input.wav", FileMode.Open);
                        List<byte> inputPCMBytes = new List<byte>();
                        List<short> inputPCMShorts = new List<short>();
                        List<byte> outputOpusBytes = new List<byte>();
                        DataHandler.ms = new MemoryStream();
                        OpusEncoder encoder = new OpusEncoder(OpusDotNet.Application.Audio, 48000, 1);
                        OpusDecoder decoder = new OpusDecoder(48000, 1);  
                        //OpusEncoder encoder = new OpusEncoder(48000, 1, OpusApplication.OPUS_APPLICATION_AUDIO);
                        //OpusDecoder decoder = new OpusDecoder(48000, 1);
                        for (int i = 0; i < fs.Length; i += 1)
                        {
                            //inputPCMShorts.Add(BitConverter.ToInt16(new byte[2] { (byte)fs.ReadByte(), (byte)fs.ReadByte() }, 0));
                            inputPCMBytes.Add((byte)fs.ReadByte());
                            if (inputPCMBytes.Count == 40 * (48000 / 1000) * 2)
                            {                           
                                encoder.MaxBandwidth = Bandwidth.FullBand;
                                encoder.Bitrate = 16000;
                                encoder.VBR = false;
                                byte[] opusBytes = new byte[80];
                                encoder.Encode(inputPCMBytes.ToArray(), inputPCMBytes.Count, opusBytes, opusBytes.Length);
                                byte[] outputSamples = new byte[40 * (48000 / 1000) * 2];
                                decoder.Decode(opusBytes, opusBytes.Length, outputSamples, outputSamples.Length);                               
                                outputOpusBytes.AddRange(opusBytes);
                                inputPCMBytes.Clear();
                                //encoder.Bitrate = 16000;
                                //encoder.UseVBR = false;
                                //encoder.Application = OpusApplication.OPUS_APPLICATION_AUDIO;
                                //encoder.Bandwidth = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
                                //encoder.SignalType = OpusSignal.OPUS_SIGNAL_AUTO;
                                //encoder.ExpertFrameDuration = OpusFramesize.OPUS_FRAMESIZE_40_MS;
                                //encoder.EnableAnalysis = true;
                                //byte[] opusBytes = new byte[80];
                                //encoder.Encode(inputPCMShorts.ToArray(), inputPCMShorts.Count, opusBytes, opusBytes.Length);
                                //short[] outputSamples = new short[40 * (48000 / 1000) * 1];
                                //List<byte> outputBytes = new List<byte>();
                                //decoder.Decode(opusBytes, outputSamples, outputSamples.Length, false);
                                //for (int k = 0; k < outputSamples.Length; k++) outputBytes.AddRange(BitConverter.GetBytes(outputSamples[k]));
                                //DataHandler.ms.Write(outputBytes.ToArray(), 0, outputBytes.Count);
                                //outputOpusBytes.AddRange(opusBytes);
                                //inputPCMShorts.Clear();
                            }
                        }
                        fs.Close();
                        fs = new FileStream("outputOpusEncoded.wav", FileMode.Create);
                        fs.Write(outputOpusBytes.ToArray(), 0, outputOpusBytes.Count);
                        fs.Close();
                        Encoder.encoder_inputFilePath = "outputOpusEncoded.wav";
                        if (Encoder.encoder_outputFilePath == "" || Encoder.encoder_outputFilePath == null) Encoder.encoder_outputFilePath = "output.wav";
                        if (Encoder.encoder_longLeadIn == false) Encoder.encoder_leadInSubcodesAmount = Encoder.encoder_leadInOutSubcodesAmount; else Encoder.encoder_leadInSubcodesAmount = 2000;
                        Encoder.thread_encodeFileStereoStream = new Thread(Encoder.EncodeFileStereoStream);
                        Encoder.thread_encodeFileStereoStream.Start();
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
