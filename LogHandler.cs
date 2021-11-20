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

        /// <summary>
        /// Сохраняет ошибку в список list_log. errSrc - источник ошибки, errMsg - сообщение об ошибке
        /// </summary>
        /// <param name="errSrc"></param>
        /// <param name="errMsg"></param>
        public static void Write(string errSrc, string errMsg)
        {
            list_log.Add("[" + errSrc + "]: " + errMsg);

            //В случае прослушивания журнала
            if (logListening == true)
                LogMonitorWindow.buff_log.Add("[" + errSrc + "]: " + errMsg + "\r\n");
        }
    }
}
