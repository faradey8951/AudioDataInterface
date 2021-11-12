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
    public partial class DebugWindow : Form
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_tick(object sender, EventArgs e)
        {
            listView_buffers.Items[0].SubItems[1].Text = AudioIO.buff_waveGraphSamples.Count().ToString();
            listView_threads.Items[0].SubItems[1].Text = ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStream);
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ProgressHandler.GetPercent(25, 100).ToString());
        }

        private void button_logMonitor_Click(object sender, EventArgs e)
        {
            if (MainWindow.class_logMonitorWindow != null)
                MainWindow.class_logMonitorWindow = new LogMonitorWindow();
            MainWindow.class_logMonitorWindow.Show();
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {

        }

        private void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MainWindow.class_logMonitorWindow != null)
                MainWindow.class_logMonitorWindow.Close();
        }
    }
}
