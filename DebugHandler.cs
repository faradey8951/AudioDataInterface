using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioDataInterface
{
    /// <summary>
    /// Обработчик ошибок
    /// </summary>
    class DebugHandler
    {
        public static List<string> list_log = new List<string>(); //Журнал ошибок

        /// <summary>
        /// Сохраняет ошибку в список list_log. errSrc - источник ошибки, errMsg - сообщение об ошибке
        /// </summary>
        /// <param name="errSrc"></param>
        /// <param name="errMsg"></param>
        public static void Write(string errSrc, string errMsg)
        {
            list_log.Add("[" + errSrc + "]: " + errMsg);
        }
    }
}
