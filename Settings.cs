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
            }
            catch (Exception ex)
            {
                LogHandler.WriteError("Settings.cs", ex.Message);
            }
        }
    }
}
