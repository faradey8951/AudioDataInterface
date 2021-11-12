using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public class ProgressHandler
    {
        /// <summary>
        /// Выдает процент part от full
        /// </summary>
        /// <param name="full"></param>
        /// <param name="part"></param>
        public static int GetPercent(long full, long part)
        {
            double percent = (double)((100 * (double)part) / (double)full);
            if (percent > 100)
                percent = 100;
            return Convert.ToInt32(percent);
        }
    }
}
