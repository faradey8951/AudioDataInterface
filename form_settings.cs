using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class form_settings : Form
    {
        string spectrumMode = "";
        double sampleRate = 0;
        double maxBitrate = 0;
        double maxBlockrate = 0;
        double effectiveBlockRate = 0;
        double effectiveBitrate = 0;
        public form_settings()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar_signalHeight_Scroll(object sender, EventArgs e)
        {
            label_signalHeightValue.Text = trackBar_signalHeight.Value.ToString();
        }

        private void trackBar_mp3BuffSize_Scroll(object sender, EventArgs e)
        {
            label_mp3BuffSizeValue.Text = trackBar_mp3BuffSize.Value.ToString();
        }

        private void trackBar_signalGain_Scroll(object sender, EventArgs e)
        {
            label_signalGainValue.Text = trackBar_signalGain.Value.ToString();
        }

        private void trackBar_silenceSeconds_Scroll(object sender, EventArgs e)
        {
            label_silenceSecondsValue.Text = trackBar_silenceSeconds.Value.ToString();
        }

        private void trackBar_leadInOutSubcodesAmount_Scroll(object sender, EventArgs e)
        {
            label_leadInOutSubcodesAmountValue.Text = trackBar_leadInOutSubcodesAmount.Value.ToString();
        }

        private void trackBar_mpsPlayerSubcodeInterval_Scroll(object sender, EventArgs e)
        {
            label_mpsPlayerSubcodeIntervalValue.Text = trackBar_mpsPlayerSubcodeInterval.Value.ToString();
        }

        private void trackBar_encodingSampleRate_Scroll(object sender, EventArgs e)
        {
            sampleRate = trackBar_encodingSampleRate.Value;
            maxBitrate = 2 * sampleRate / 8.0;
            maxBlockrate = maxBitrate / 39.0;
            effectiveBlockRate = maxBitrate / 41.0;
            effectiveBitrate = Math.Round(((effectiveBlockRate * 32.0) / 1000),2);
            label_encodingDensityValue.Text = effectiveBitrate.ToString();
        }

        private void trackBar_fftSize_Scroll(object sender, EventArgs e)
        {
            label_fftSizeValue.Text = trackBar_fftSize.Value.ToString();
        }

        private void trackBar_spectrumVescosity_Scroll(object sender, EventArgs e)
        {
            label_spectrumVescosityValue.Text = trackBar_spectrumVescosity.Value.ToString();
        }

        private void form_settings_Load(object sender, EventArgs e)
        {
            trackBar_signalHeight.Value = AudioIO.audio_signalHeight;
            trackBar_signalHeight_Scroll(this, EventArgs.Empty);
            trackBar_mp3BuffSize.Value = DataHandler.mp3_buffSize;
            trackBar_mp3BuffSize_Scroll(this, EventArgs.Empty);
            trackBar_encodingSampleRate.Value = Encoder.encoder_sampleRate;
            trackBar_encodingSampleRate_Scroll(this, EventArgs.Empty);
            trackBar_signalGain.Value = Encoder.encoder_signalGain;
            trackBar_signalGain_Scroll(this, EventArgs.Empty);
            trackBar_silenceSeconds.Value = Encoder.encoder_silenceSeconds;
            trackBar_silenceSeconds_Scroll(this, EventArgs.Empty);
            trackBar_leadInOutSubcodesAmount.Value = Encoder.encoder_leadInOutSubcodesAmount;
            trackBar_leadInOutSubcodesAmount_Scroll(this, EventArgs.Empty);
            trackBar_mpsPlayerSubcodeInterval.Value = Encoder.encoder_mpsPlayerSubCodeInterval;
            trackBar_mpsPlayerSubcodeInterval_Scroll(this, EventArgs.Empty);
            trackBar_fftSize.Value = form_main.mpsPlayer_fftSize;
            trackBar_fftSize_Scroll(this, EventArgs.Empty);
            trackBar_spectrumVescosity.Value = form_main.mpsPlayer_spectrumVescosity;
            trackBar_spectrumVescosity_Scroll(this, EventArgs.Empty);
            spectrumMode = form_main.mpsPlayer_spectrumMode;
            if (spectrumMode == "peakHold") radioButton_peakHoldMode.Checked = true;
            if (spectrumMode == "noPeak") radioButton_noPeakMode.Checked = true;
            if (spectrumMode == "off") radioButton_offMode.Checked = true;
            textBox_Ffmpeg1Cmd.Text = Encoder.encoder_ffmpeg1Cmd;
            textBox_Ffmpeg2Cmd.Text = Encoder.encoder_ffmpeg2Cmd;
            textBox_Ffmpeg2EffectCmd.Text = Encoder.encoder_ffmpeg2EffectCmd;
            form_main.window_main.timer_mpsPlayerSpectrumHandler.Interval = form_main.mpsPlayer_spectrumVescosity;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AudioIO.audio_signalHeight = trackBar_signalHeight.Value;
            DataHandler.mp3_buffSize = trackBar_mp3BuffSize.Value;
            Encoder.encoder_sampleRate = trackBar_encodingSampleRate.Value;
            Encoder.encoder_signalGain = trackBar_signalGain.Value;
            Encoder.encoder_silenceSeconds = trackBar_silenceSeconds.Value;
            Encoder.encoder_leadInOutSubcodesAmount = trackBar_leadInOutSubcodesAmount.Value;
            Encoder.encoder_mpsPlayerSubCodeInterval = trackBar_mpsPlayerSubcodeInterval.Value;
            form_main.mpsPlayer_fftSize = trackBar_fftSize.Value;
            form_main.mpsPlayer_spectrumMode = spectrumMode;
            form_main.mpsPlayer_spectrumVescosity = trackBar_spectrumVescosity.Value;
            Encoder.encoder_ffmpeg1Cmd = textBox_Ffmpeg1Cmd.Text;
            Encoder.encoder_ffmpeg2Cmd = textBox_Ffmpeg2Cmd.Text;
            Encoder.encoder_ffmpeg2EffectCmd = textBox_Ffmpeg2EffectCmd.Text;

            form_main.window_main.timer_mpsPlayerSpectrumHandler.Interval = form_main.mpsPlayer_spectrumVescosity;
            form_main.mpsPlayer_liveSpectrum = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            form_main.mpsPlayer_spectrumPeakHold = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            form_main.window_main.DrawMPSPlayerInterface();

            Settings.Save();
        }

        private void radioButton_peakHoldMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_peakHoldMode.Checked) spectrumMode = "peakHold";
        }

        private void radioButton_noPeakMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_noPeakMode.Checked) spectrumMode = "noPeak";
        }

        private void radioButton_offMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_offMode.Checked) spectrumMode = "off";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.Default();
            form_settings_Load(this, EventArgs.Empty);
        }
    }
}
