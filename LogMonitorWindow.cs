using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class LogMonitorWindow : Form
    {
        public LogMonitorWindow()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {
            richTextBox.Clear();
            foreach (string s in DebugHandler.list_log.ToArray())
                richTextBox.AppendText(s);
        }
    }
}
