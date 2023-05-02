using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class form_logMonitor : Form
    {
        public static List<string> buff_log = new List<string>(); //Буфер ошибок для вывода

        public form_logMonitor()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            while (buff_log.Count > 0)
            {
                richTextBox.AppendText(buff_log[0]);
                buff_log.RemoveAt(0);
            }
        }

        private void LogMonitorWindow_Load(object sender, EventArgs e)
        {
            LogHandler.logListening = true;
            buff_log.AddRange(LogHandler.list_log); //Вписать list_log в буфер ошибок
        }

        private void LogMonitorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogHandler.logListening = false;
        }
    }
}
