using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AudioDataInterface
{
    /// <summary>
    /// Работа с настройками программы
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Сохранение настроек программы
        /// </summary>
        public static void Save()
        {
            Properties.Settings.Default.recDeviceId = AudioIO.audio_recDeviceId;
            Properties.Settings.Default.playDeviceId = AudioIO.audio_playDeviceId;
            Properties.Settings.Default.mainWindowMaximized = form_main.window_main.WindowState == FormWindowState.Maximized ? true : false;
            Properties.Settings.Default.mainWindowHeight = form_main.window_main.Height;
            Properties.Settings.Default.mainWindowWidth = form_main.window_main.Width;
            Properties.Settings.Default.invertSignal = AudioIO.audio_invertSignal;

            Properties.Settings.Default.signalHeight = AudioIO.audio_signalHeight;
            Properties.Settings.Default.mp3BufferSize = DataHandler.mp3_buffSize;
            Properties.Settings.Default.encoderSampleRate = Encoder.encoder_sampleRate;
            Properties.Settings.Default.encoderSignalGain = Encoder.encoder_signalGain;
            Properties.Settings.Default.encoderSilenceSeconds = Encoder.encoder_silenceSeconds;
            Properties.Settings.Default.encoderLeadInOutSubcodesAmount = Encoder.encoder_leadInOutSubcodesAmount;
            Properties.Settings.Default.encoderMpsPlayerSubcodeInterval = Encoder.encoder_mpsPlayerSubCodeInterval;
            Properties.Settings.Default.mpsFftSize = form_main.mpsPlayer_fftSize;
            Properties.Settings.Default.mpsSpectrumMode = form_main.mpsPlayer_spectrumMode;
            Properties.Settings.Default.mpsSpectrumVescosity = form_main.mpsPlayer_spectrumVescosity;
            Properties.Settings.Default.encoderFfmpeg1Cmd = Encoder.encoder_ffmpeg1Cmd;
            Properties.Settings.Default.encoderFfmpeg2Cmd = Encoder.encoder_ffmpeg2Cmd;
            Properties.Settings.Default.encoderFfmpeg2EffectCmd = Encoder.encoder_ffmpeg2EffectCmd;
            Properties.Settings.Default.mpsSkin = form_main.class_mpsPlayerSkinHandler.currentSkinName;
            Properties.Settings.Default.Save();
        }

        public static void Load()
        {
            try
            {
                AudioIO.audio_recDeviceId = Properties.Settings.Default.recDeviceId;
                AudioIO.audio_playDeviceId = Properties.Settings.Default.playDeviceId;
                if (Properties.Settings.Default.mainWindowMaximized == true)
                    form_main.window_main.WindowState = FormWindowState.Maximized;
                else
                    form_main.window_main.WindowState = FormWindowState.Normal;
                form_main.window_main.Width = Properties.Settings.Default.mainWindowWidth;
                form_main.window_main.Height = Properties.Settings.Default.mainWindowHeight;
                AudioIO.audio_invertSignal = Properties.Settings.Default.invertSignal;
                form_main.window_main.checkBox_invertSignal.Checked = AudioIO.audio_invertSignal;

                AudioIO.audio_signalHeight = Properties.Settings.Default.signalHeight;
                DataHandler.mp3_buffSize = Properties.Settings.Default.mp3BufferSize;
                Encoder.encoder_sampleRate = Properties.Settings.Default.encoderSampleRate;
                Encoder.encoder_signalGain = Properties.Settings.Default.encoderSignalGain;
                Encoder.encoder_silenceSeconds = Properties.Settings.Default.encoderSilenceSeconds;
                Encoder.encoder_leadInOutSubcodesAmount = Properties.Settings.Default.encoderLeadInOutSubcodesAmount;
                Encoder.encoder_mpsPlayerSubCodeInterval = Properties.Settings.Default.encoderMpsPlayerSubcodeInterval;
                form_main.mpsPlayer_fftSize = Properties.Settings.Default.mpsFftSize;
                form_main.mpsPlayer_spectrumMode = Properties.Settings.Default.mpsSpectrumMode;
                form_main.mpsPlayer_spectrumVescosity = Properties.Settings.Default.mpsSpectrumVescosity;
                Encoder.encoder_ffmpeg1Cmd = Properties.Settings.Default.encoderFfmpeg1Cmd;
                Encoder.encoder_ffmpeg2Cmd = Properties.Settings.Default.encoderFfmpeg2Cmd;
                Encoder.encoder_ffmpeg2EffectCmd = Properties.Settings.Default.encoderFfmpeg2EffectCmd;

                form_main.window_main.timer_mpsPlayerSpectrumHandler.Interval = form_main.mpsPlayer_spectrumVescosity;
                form_main.class_mpsPlayerSkinHandler.currentSkinName = Properties.Settings.Default.mpsSkin;
            }
            catch (Exception ex)
            {
                LogHandler.WriteError("Settings.cs", ex.Message);
            }
        }
        public static void Default()
        {
            Properties.Settings.Default.Reset();
            Load();
        }
    }
}
