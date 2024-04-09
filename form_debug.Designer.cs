
namespace AudioDataInterface
{
    partial class form_debug
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "graphSamples",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "signalSamples",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "log",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "signalAmplitudes",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "decodedData",
            "0"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "EncodeFileStream",
            "status"}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "AmplitudeDecoderL",
            "status"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "AmplitudeDecoderR",
            "status"}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "SamplesDecoderStereo",
            "status"}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "BinaryDecoderStereo",
            "status"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_debug));
            this.listView_buffers = new System.Windows.Forms.ListView();
            this.column_buffer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox_buffers = new System.Windows.Forms.GroupBox();
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.groupBox_threads = new System.Windows.Forms.GroupBox();
            this.listView_threads = new System.Windows.Forms.ListView();
            this.column_thread = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.button_test = new System.Windows.Forms.ToolStripButton();
            this.button_logMonitor = new System.Windows.Forms.ToolStripButton();
            this.groupBox_buffers.SuspendLayout();
            this.groupBox_threads.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_buffers
            // 
            this.listView_buffers.BackColor = System.Drawing.SystemColors.Window;
            this.listView_buffers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_buffer,
            this.column_size});
            this.listView_buffers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_buffers.GridLines = true;
            this.listView_buffers.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            this.listView_buffers.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.listView_buffers.Location = new System.Drawing.Point(3, 16);
            this.listView_buffers.Name = "listView_buffers";
            this.listView_buffers.Size = new System.Drawing.Size(361, 153);
            this.listView_buffers.TabIndex = 1;
            this.listView_buffers.UseCompatibleStateImageBehavior = false;
            this.listView_buffers.View = System.Windows.Forms.View.Details;
            // 
            // column_buffer
            // 
            this.column_buffer.Text = "Буфер";
            this.column_buffer.Width = 150;
            // 
            // column_size
            // 
            this.column_size.Text = "Размер";
            this.column_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_size.Width = 75;
            // 
            // groupBox_buffers
            // 
            this.groupBox_buffers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_buffers.Controls.Add(this.listView_buffers);
            this.groupBox_buffers.Location = new System.Drawing.Point(12, 35);
            this.groupBox_buffers.Name = "groupBox_buffers";
            this.groupBox_buffers.Size = new System.Drawing.Size(367, 172);
            this.groupBox_buffers.TabIndex = 2;
            this.groupBox_buffers.TabStop = false;
            this.groupBox_buffers.Text = "Буферы";
            // 
            // timer_controlHandler
            // 
            this.timer_controlHandler.Enabled = true;
            this.timer_controlHandler.Interval = 50;
            this.timer_controlHandler.Tick += new System.EventHandler(this.timer_controlHandler_tick);
            // 
            // groupBox_threads
            // 
            this.groupBox_threads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_threads.Controls.Add(this.listView_threads);
            this.groupBox_threads.Location = new System.Drawing.Point(12, 213);
            this.groupBox_threads.Name = "groupBox_threads";
            this.groupBox_threads.Size = new System.Drawing.Size(367, 172);
            this.groupBox_threads.TabIndex = 3;
            this.groupBox_threads.TabStop = false;
            this.groupBox_threads.Text = "Процессы";
            // 
            // listView_threads
            // 
            this.listView_threads.BackColor = System.Drawing.SystemColors.Window;
            this.listView_threads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_thread,
            this.column_status});
            this.listView_threads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_threads.GridLines = true;
            this.listView_threads.HideSelection = false;
            this.listView_threads.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10});
            this.listView_threads.Location = new System.Drawing.Point(3, 16);
            this.listView_threads.Name = "listView_threads";
            this.listView_threads.Size = new System.Drawing.Size(361, 153);
            this.listView_threads.TabIndex = 1;
            this.listView_threads.UseCompatibleStateImageBehavior = false;
            this.listView_threads.View = System.Windows.Forms.View.Details;
            this.listView_threads.SelectedIndexChanged += new System.EventHandler(this.listView_threads_SelectedIndexChanged);
            // 
            // column_thread
            // 
            this.column_thread.Text = "Процесс";
            this.column_thread.Width = 150;
            // 
            // column_status
            // 
            this.column_status.Text = "Статус";
            this.column_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_status.Width = 75;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_test,
            this.button_logMonitor});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(391, 25);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "toolStrip1";
            // 
            // button_test
            // 
            this.button_test.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_test.Image = ((System.Drawing.Image)(resources.GetObject("button_test.Image")));
            this.button_test.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(74, 22);
            this.button_test.Text = "Test Feature";
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // button_logMonitor
            // 
            this.button_logMonitor.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.button_logMonitor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_logMonitor.Image = ((System.Drawing.Image)(resources.GetObject("button_logMonitor.Image")));
            this.button_logMonitor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_logMonitor.Name = "button_logMonitor";
            this.button_logMonitor.Size = new System.Drawing.Size(77, 22);
            this.button_logMonitor.Text = "Log monitor";
            this.button_logMonitor.ToolTipText = "button_logMonitor";
            this.button_logMonitor.Click += new System.EventHandler(this.button_logMonitor_Click);
            // 
            // form_debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(391, 408);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.groupBox_threads);
            this.Controls.Add(this.groupBox_buffers);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(407, 396);
            this.Name = "form_debug";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DebugWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugWindow_FormClosing);
            this.Load += new System.EventHandler(this.DebugWindow_Load);
            this.groupBox_buffers.ResumeLayout(false);
            this.groupBox_threads.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listView_buffers;
        private System.Windows.Forms.ColumnHeader column_buffer;
        private System.Windows.Forms.ColumnHeader column_size;
        private System.Windows.Forms.GroupBox groupBox_buffers;
        private System.Windows.Forms.Timer timer_controlHandler;
        private System.Windows.Forms.GroupBox groupBox_threads;
        private System.Windows.Forms.ListView listView_threads;
        private System.Windows.Forms.ColumnHeader column_thread;
        private System.Windows.Forms.ColumnHeader column_status;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton button_test;
        private System.Windows.Forms.ToolStripButton button_logMonitor;
    }
}