using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AudioDataInterface
{
    /// <summary>
    /// Обработчик потоков
    /// </summary>
    public class ThreadHandler
    {
        /// <summary>
        /// Получает статус работы процесса thread
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public static string GetThreadStatus(Thread thread)
        {
            if (thread == null)
                return "null";
            else
                return thread.ThreadState.ToString();
        }
    }
}
