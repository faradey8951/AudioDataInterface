﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class form_debug : Form
    {
        public form_debug()
        {
            InitializeComponent();
        }

        private void timer_controlHandler_tick(object sender, EventArgs e)
        {
            listView_buffers.Items[0].SubItems[1].Text = AudioIO.buff_graphSamples.Count().ToString();
            listView_buffers.Items[1].SubItems[1].Text = AudioIO.buff_signalSamplesL.Count().ToString();
            listView_buffers.Items[2].SubItems[1].Text = LogHandler.list_log.Count().ToString();
            listView_buffers.Items[3].SubItems[1].Text = Decoder.buff_signalAmplitudesL.Count().ToString();
            listView_buffers.Items[4].SubItems[1].Text = Decoder.buff_decodedData.Count().ToString();
            listView_buffers.Items[5].SubItems[1].Text = AudioIO.buff_signalBytes.Count().ToString();
            listView_threads.Items[0].SubItems[1].Text = ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStereoStream);
            listView_threads.Items[1].SubItems[1].Text = ThreadHandler.GetThreadStatus(Decoder.thread_amplitudeDecoderL);
            listView_threads.Items[2].SubItems[1].Text = ThreadHandler.GetThreadStatus(Decoder.thread_amplitudeDecoderR);
            listView_threads.Items[3].SubItems[1].Text = ThreadHandler.GetThreadStatus(Decoder.thread_samplesDecoderStereo);
            listView_threads.Items[4].SubItems[1].Text = ThreadHandler.GetThreadStatus(Decoder.thread_binaryDecoderStereo);
            listView_threads.Items[5].SubItems[1].Text = ThreadHandler.GetThreadStatus(DataHandler.thread_bufferMp3);
            listView_threads.Items[6].SubItems[1].Text = ThreadHandler.GetThreadStatus(DataHandler.thread_playMp3);
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ProgressHandler.GetPercent(25, 100).ToString());
        }

        private void button_logMonitor_Click(object sender, EventArgs e)
        {
            if (form_main.window_logMonitor != null)
            {
                form_main.window_logMonitor.Close();
                form_main.window_logMonitor = new form_logMonitor();
            }
            form_main.window_logMonitor.Show();
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {

        }

        private void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (form_main.window_logMonitor != null)
                form_main.window_logMonitor.Close();
        }

        private void listView_threads_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
