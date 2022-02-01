using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioDataInterface
{
    /// <summary>
    /// Обработчик ошибок
    /// </summary>
    class LogHandler
    {
        public static List<string> list_log = new List<string>(); //Журнал ошибок
        public static bool logListening = false; //Указывает прослушивается ли журнал ошибок через LogMonitorWindow

        public static void WriteError(string source, string message)
        {
            list_log.Add("-[ERROR] in ( " + source + " ): " + message + "\r\n");
            //В случае прослушивания журнала
            if (logListening == true)
                LogMonitorWindow.buff_log.Add("-[ERROR] in ( " + source + " ): " + message + "\r\n");
        }

        public static void WriteStatus(string source, string message)
        {
            list_log.Add("-[STATUS] in ( " + source + " ): " + message + "\r\n");
            //В случае прослушивания журнала
            if (logListening == true)
                LogMonitorWindow.buff_log.Add("-[STATUS] in ( " + source + " ): " + message + "\r\n");
        }

    }
}
