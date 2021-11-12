using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Properties.Settings.Default.Save();
        }

        public static void Load()
        {
            AudioIO.audio_recDeviceId = Properties.Settings.Default.recDeviceId;
            AudioIO.audio_playDeviceId = Properties.Settings.Default.playDeviceId;
        }
    }
}
