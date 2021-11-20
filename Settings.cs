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
            Properties.Settings.Default.mainWindowMaximized = MainWindow.class_mainWindow.WindowState == FormWindowState.Maximized ? true : false;
            Properties.Settings.Default.mainWindowHeight = MainWindow.class_mainWindow.Height;
            Properties.Settings.Default.mainWindowWidth = MainWindow.class_mainWindow.Width;
            Properties.Settings.Default.Save();
        }

        public static void Load()
        {
            try
            {
                AudioIO.audio_recDeviceId = Properties.Settings.Default.recDeviceId;
                AudioIO.audio_playDeviceId = Properties.Settings.Default.playDeviceId;
                if (Properties.Settings.Default.mainWindowMaximized == true)
                    MainWindow.class_mainWindow.WindowState = FormWindowState.Maximized;
                else
                    MainWindow.class_mainWindow.WindowState = FormWindowState.Normal;
                MainWindow.class_mainWindow.Width = Properties.Settings.Default.mainWindowWidth;
                MainWindow.class_mainWindow.Height = Properties.Settings.Default.mainWindowHeight;
            }
            catch (Exception ex)
            {
                LogHandler.Write("Settings.cs", ex.Message);
            }
        }
    }
}
