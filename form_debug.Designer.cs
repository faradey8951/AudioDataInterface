
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
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_decoderDebug = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pictureBox_signal = new System.Windows.Forms.PictureBox();
            this.pictureBox_sortedDerivative = new System.Windows.Forms.PictureBox();
            this.pictureBox_decodingQuality = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox_clipping = new System.Windows.Forms.CheckBox();
            this.checkBox_pause = new System.Windows.Forms.CheckBox();
            this.radioButton_slow = new System.Windows.Forms.RadioButton();
            this.radioButton_fast = new System.Windows.Forms.RadioButton();
            this.tabPage_log = new System.Windows.Forms.TabPage();
            this.timer_drawGraph = new System.Windows.Forms.Timer(this.components);
            this.tabControl.SuspendLayout();
            this.tabPage_decoderDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_signal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_sortedDerivative)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_decodingQuality)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage_log.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.BackColor = System.Drawing.Color.Black;
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Font = new System.Drawing.Font("Roboto", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox.ForeColor = System.Drawing.Color.Lime;
            this.richTextBox.Location = new System.Drawing.Point(3, 3);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(719, 397);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // timer_controlHandler
            // 
            this.timer_controlHandler.Enabled = true;
            this.timer_controlHandler.Interval = 1000;
            this.timer_controlHandler.Tick += new System.EventHandler(this.timer_controlHandler_Tick);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_decoderDebug);
            this.tabControl.Controls.Add(this.tabPage_log);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(733, 429);
            this.tabControl.TabIndex = 1;
            // 
            // tabPage_decoderDebug
            // 
            this.tabPage_decoderDebug.Controls.Add(this.splitContainer1);
            this.tabPage_decoderDebug.Location = new System.Drawing.Point(4, 22);
            this.tabPage_decoderDebug.Name = "tabPage_decoderDebug";
            this.tabPage_decoderDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_decoderDebug.Size = new System.Drawing.Size(725, 403);
            this.tabPage_decoderDebug.TabIndex = 1;
            this.tabPage_decoderDebug.Text = "Отладка декодера";
            this.tabPage_decoderDebug.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(719, 397);
            this.splitContainer1.SplitterDistance = 611;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox_decodingQuality);
            this.splitContainer2.Size = new System.Drawing.Size(611, 397);
            this.splitContainer2.SplitterDistance = 266;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pictureBox_signal);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.pictureBox_sortedDerivative);
            this.splitContainer.Size = new System.Drawing.Size(611, 266);
            this.splitContainer.SplitterDistance = 136;
            this.splitContainer.TabIndex = 0;
            // 
            // pictureBox_signal
            // 
            this.pictureBox_signal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_signal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_signal.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_signal.Name = "pictureBox_signal";
            this.pictureBox_signal.Size = new System.Drawing.Size(611, 136);
            this.pictureBox_signal.TabIndex = 0;
            this.pictureBox_signal.TabStop = false;
            this.pictureBox_signal.SizeChanged += new System.EventHandler(this.pictureBox_signal_SizeChanged);
            // 
            // pictureBox_sortedDerivative
            // 
            this.pictureBox_sortedDerivative.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_sortedDerivative.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_sortedDerivative.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_sortedDerivative.Name = "pictureBox_sortedDerivative";
            this.pictureBox_sortedDerivative.Size = new System.Drawing.Size(611, 126);
            this.pictureBox_sortedDerivative.TabIndex = 0;
            this.pictureBox_sortedDerivative.TabStop = false;
            this.pictureBox_sortedDerivative.SizeChanged += new System.EventHandler(this.pictureBox_sortedDerivative_SizeChanged);
            // 
            // pictureBox_decodingQuality
            // 
            this.pictureBox_decodingQuality.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_decodingQuality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_decodingQuality.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_decodingQuality.Name = "pictureBox_decodingQuality";
            this.pictureBox_decodingQuality.Size = new System.Drawing.Size(611, 127);
            this.pictureBox_decodingQuality.TabIndex = 0;
            this.pictureBox_decodingQuality.TabStop = false;
            this.pictureBox_decodingQuality.SizeChanged += new System.EventHandler(this.pictureBox_decodingQuality_SizeChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.checkBox_clipping);
            this.panel1.Controls.Add(this.checkBox_pause);
            this.panel1.Controls.Add(this.radioButton_slow);
            this.panel1.Controls.Add(this.radioButton_fast);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(104, 397);
            this.panel1.TabIndex = 0;
            // 
            // checkBox_clipping
            // 
            this.checkBox_clipping.AutoSize = true;
            this.checkBox_clipping.Checked = true;
            this.checkBox_clipping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_clipping.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBox_clipping.Location = new System.Drawing.Point(0, 378);
            this.checkBox_clipping.Name = "checkBox_clipping";
            this.checkBox_clipping.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.checkBox_clipping.Size = new System.Drawing.Size(102, 17);
            this.checkBox_clipping.TabIndex = 3;
            this.checkBox_clipping.Text = "Клиппер";
            this.checkBox_clipping.UseVisualStyleBackColor = true;
            this.checkBox_clipping.CheckedChanged += new System.EventHandler(this.checkBox_clipping_CheckedChanged);
            // 
            // checkBox_pause
            // 
            this.checkBox_pause.AutoSize = true;
            this.checkBox_pause.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBox_pause.Location = new System.Drawing.Point(0, 34);
            this.checkBox_pause.Name = "checkBox_pause";
            this.checkBox_pause.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.checkBox_pause.Size = new System.Drawing.Size(102, 17);
            this.checkBox_pause.TabIndex = 2;
            this.checkBox_pause.Text = "Пауза";
            this.checkBox_pause.UseVisualStyleBackColor = true;
            this.checkBox_pause.CheckedChanged += new System.EventHandler(this.checkBox_pause_CheckedChanged);
            // 
            // radioButton_slow
            // 
            this.radioButton_slow.AutoSize = true;
            this.radioButton_slow.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButton_slow.Location = new System.Drawing.Point(0, 17);
            this.radioButton_slow.Name = "radioButton_slow";
            this.radioButton_slow.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.radioButton_slow.Size = new System.Drawing.Size(102, 17);
            this.radioButton_slow.TabIndex = 1;
            this.radioButton_slow.Text = "Медленно";
            this.radioButton_slow.UseVisualStyleBackColor = true;
            this.radioButton_slow.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton_fast
            // 
            this.radioButton_fast.AutoSize = true;
            this.radioButton_fast.Checked = true;
            this.radioButton_fast.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButton_fast.Location = new System.Drawing.Point(0, 0);
            this.radioButton_fast.Name = "radioButton_fast";
            this.radioButton_fast.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.radioButton_fast.Size = new System.Drawing.Size(102, 17);
            this.radioButton_fast.TabIndex = 0;
            this.radioButton_fast.TabStop = true;
            this.radioButton_fast.Text = "Быстро";
            this.radioButton_fast.UseVisualStyleBackColor = true;
            this.radioButton_fast.CheckedChanged += new System.EventHandler(this.radioButton_fast_CheckedChanged);
            // 
            // tabPage_log
            // 
            this.tabPage_log.Controls.Add(this.richTextBox);
            this.tabPage_log.Location = new System.Drawing.Point(4, 22);
            this.tabPage_log.Name = "tabPage_log";
            this.tabPage_log.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_log.Size = new System.Drawing.Size(725, 403);
            this.tabPage_log.TabIndex = 0;
            this.tabPage_log.Text = "Журнал";
            // 
            // timer_drawGraph
            // 
            this.timer_drawGraph.Interval = 50;
            this.timer_drawGraph.Tick += new System.EventHandler(this.timer_drawGraph_Tick);
            // 
            // form_debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(733, 429);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(749, 344);
            this.Name = "form_debug";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogMonitorWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogMonitorWindow_FormClosing);
            this.Load += new System.EventHandler(this.LogMonitorWindow_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage_decoderDebug.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_signal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_sortedDerivative)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_decodingQuality)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage_log.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Timer timer_controlHandler;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_log;
        private System.Windows.Forms.TabPage tabPage_decoderDebug;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox pictureBox_signal;
        private System.Windows.Forms.PictureBox pictureBox_sortedDerivative;
        private System.Windows.Forms.Timer timer_drawGraph;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pictureBox_decodingQuality;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_slow;
        private System.Windows.Forms.RadioButton radioButton_fast;
        private System.Windows.Forms.CheckBox checkBox_pause;
        private System.Windows.Forms.CheckBox checkBox_clipping;
    }
}