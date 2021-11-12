
namespace AudioDataInterface
{
    partial class EncoderWindow
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.label_encoding = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.label_percent = new System.Windows.Forms.ToolStripStatusLabel();
            this.button_ok = new System.Windows.Forms.Button();
            this.groupBox_text = new System.Windows.Forms.GroupBox();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox_file = new System.Windows.Forms.GroupBox();
            this.groupBox_properties = new System.Windows.Forms.GroupBox();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.radioButton_stereo = new System.Windows.Forms.RadioButton();
            this.radioButton_mono = new System.Windows.Forms.RadioButton();
            this.button_select = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label_encodingDensity = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button_convert = new System.Windows.Forms.Button();
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.button_clear = new System.Windows.Forms.Button();
            this.radioButton_text = new System.Windows.Forms.RadioButton();
            this.radioButton_file = new System.Windows.Forms.RadioButton();
            this.groupBox_mode = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.button_debug = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip.SuspendLayout();
            this.groupBox_text.SuspendLayout();
            this.groupBox_file.SuspendLayout();
            this.groupBox_properties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.groupBox_mode.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.label_encoding,
            this.progressBar,
            this.label_percent});
            this.statusStrip.Location = new System.Drawing.Point(0, 447);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(445, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // label_encoding
            // 
            this.label_encoding.Name = "label_encoding";
            this.label_encoding.Size = new System.Drawing.Size(87, 17);
            this.label_encoding.Text = "Конвертация...";
            this.label_encoding.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            this.progressBar.Visible = false;
            // 
            // label_percent
            // 
            this.label_percent.Name = "label_percent";
            this.label_percent.Size = new System.Drawing.Size(35, 17);
            this.label_percent.Text = "100%";
            this.label_percent.Visible = false;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(358, 421);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // groupBox_text
            // 
            this.groupBox_text.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_text.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox_text.Controls.Add(this.richTextBox);
            this.groupBox_text.Location = new System.Drawing.Point(13, 33);
            this.groupBox_text.Name = "groupBox_text";
            this.groupBox_text.Size = new System.Drawing.Size(420, 186);
            this.groupBox_text.TabIndex = 2;
            this.groupBox_text.TabStop = false;
            this.groupBox_text.Text = "Текст";
            // 
            // richTextBox
            // 
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(3, 16);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(414, 167);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // groupBox_file
            // 
            this.groupBox_file.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_file.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox_file.Controls.Add(this.groupBox_properties);
            this.groupBox_file.Controls.Add(this.button_select);
            this.groupBox_file.Controls.Add(this.textBox);
            this.groupBox_file.Location = new System.Drawing.Point(13, 225);
            this.groupBox_file.Name = "groupBox_file";
            this.groupBox_file.Size = new System.Drawing.Size(420, 124);
            this.groupBox_file.TabIndex = 3;
            this.groupBox_file.TabStop = false;
            this.groupBox_file.Text = "Файл";
            // 
            // groupBox_properties
            // 
            this.groupBox_properties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_properties.Controls.Add(this.checkBox);
            this.groupBox_properties.Controls.Add(this.radioButton_stereo);
            this.groupBox_properties.Controls.Add(this.radioButton_mono);
            this.groupBox_properties.Location = new System.Drawing.Point(6, 46);
            this.groupBox_properties.Name = "groupBox_properties";
            this.groupBox_properties.Size = new System.Drawing.Size(408, 73);
            this.groupBox_properties.TabIndex = 7;
            this.groupBox_properties.TabStop = false;
            this.groupBox_properties.Text = "Настройки";
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Location = new System.Drawing.Point(326, 20);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(76, 17);
            this.checkBox.TabIndex = 2;
            this.checkBox.Text = "ADI-FShell";
            this.checkBox.UseVisualStyleBackColor = true;
            // 
            // radioButton_stereo
            // 
            this.radioButton_stereo.AutoSize = true;
            this.radioButton_stereo.Location = new System.Drawing.Point(6, 42);
            this.radioButton_stereo.Name = "radioButton_stereo";
            this.radioButton_stereo.Size = new System.Drawing.Size(97, 17);
            this.radioButton_stereo.TabIndex = 1;
            this.radioButton_stereo.Text = "стерео-режим";
            this.radioButton_stereo.UseVisualStyleBackColor = true;
            // 
            // radioButton_mono
            // 
            this.radioButton_mono.AutoSize = true;
            this.radioButton_mono.Checked = true;
            this.radioButton_mono.Location = new System.Drawing.Point(6, 19);
            this.radioButton_mono.Name = "radioButton_mono";
            this.radioButton_mono.Size = new System.Drawing.Size(88, 17);
            this.radioButton_mono.TabIndex = 0;
            this.radioButton_mono.TabStop = true;
            this.radioButton_mono.Text = "моно-режим";
            this.radioButton_mono.UseVisualStyleBackColor = true;
            // 
            // button_select
            // 
            this.button_select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_select.Location = new System.Drawing.Point(337, 17);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(75, 23);
            this.button_select.TabIndex = 6;
            this.button_select.Text = "Выбрать";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BackColor = System.Drawing.SystemColors.Window;
            this.textBox.Location = new System.Drawing.Point(6, 19);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(325, 20);
            this.textBox.TabIndex = 0;
            // 
            // trackBar
            // 
            this.trackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBar.Location = new System.Drawing.Point(6, 390);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(266, 45);
            this.trackBar.TabIndex = 4;
            // 
            // label_encodingDensity
            // 
            this.label_encodingDensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_encodingDensity.AutoSize = true;
            this.label_encodingDensity.Location = new System.Drawing.Point(12, 374);
            this.label_encodingDensity.Name = "label_encodingDensity";
            this.label_encodingDensity.Size = new System.Drawing.Size(172, 13);
            this.label_encodingDensity.TabIndex = 5;
            this.label_encodingDensity.Text = "Скорость потока данных: 1 KB/s";
            // 
            // button_convert
            // 
            this.button_convert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_convert.Location = new System.Drawing.Point(253, 421);
            this.button_convert.Name = "button_convert";
            this.button_convert.Size = new System.Drawing.Size(99, 23);
            this.button_convert.TabIndex = 6;
            this.button_convert.Text = "Конвертировать";
            this.button_convert.UseVisualStyleBackColor = true;
            this.button_convert.Click += new System.EventHandler(this.button_convert_Click);
            // 
            // timer_controlHandler
            // 
            this.timer_controlHandler.Enabled = true;
            this.timer_controlHandler.Tick += new System.EventHandler(this.timer_controlHandler_Tick);
            // 
            // button_clear
            // 
            this.button_clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_clear.Location = new System.Drawing.Point(13, 421);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(75, 23);
            this.button_clear.TabIndex = 7;
            this.button_clear.Text = "Очистить";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // radioButton_text
            // 
            this.radioButton_text.AutoSize = true;
            this.radioButton_text.Location = new System.Drawing.Point(6, 19);
            this.radioButton_text.Name = "radioButton_text";
            this.radioButton_text.Size = new System.Drawing.Size(138, 17);
            this.radioButton_text.TabIndex = 8;
            this.radioButton_text.Text = "конвертировать текст";
            this.radioButton_text.UseVisualStyleBackColor = true;
            this.radioButton_text.CheckedChanged += new System.EventHandler(this.radioButton_text_CheckedChanged);
            // 
            // radioButton_file
            // 
            this.radioButton_file.AutoSize = true;
            this.radioButton_file.Checked = true;
            this.radioButton_file.Location = new System.Drawing.Point(6, 38);
            this.radioButton_file.Name = "radioButton_file";
            this.radioButton_file.Size = new System.Drawing.Size(136, 17);
            this.radioButton_file.TabIndex = 9;
            this.radioButton_file.TabStop = true;
            this.radioButton_file.Text = "конвертировать файл";
            this.radioButton_file.UseVisualStyleBackColor = true;
            this.radioButton_file.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox_mode
            // 
            this.groupBox_mode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_mode.Controls.Add(this.radioButton_text);
            this.groupBox_mode.Controls.Add(this.radioButton_file);
            this.groupBox_mode.Location = new System.Drawing.Point(278, 350);
            this.groupBox_mode.Name = "groupBox_mode";
            this.groupBox_mode.Size = new System.Drawing.Size(155, 65);
            this.groupBox_mode.TabIndex = 10;
            this.groupBox_mode.TabStop = false;
            this.groupBox_mode.Text = "Режим";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_debug});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(445, 25);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // button_debug
            // 
            this.button_debug.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.button_debug.Image = global::AudioDataInterface.Properties.Resources.debug;
            this.button_debug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_debug.Name = "button_debug";
            this.button_debug.Size = new System.Drawing.Size(72, 22);
            this.button_debug.Text = "Отладка";
            this.button_debug.Click += new System.EventHandler(this.button_debug_Click);
            // 
            // EncoderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(445, 469);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox_mode);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_convert);
            this.Controls.Add(this.label_encodingDensity);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.groupBox_file);
            this.Controls.Add(this.groupBox_text);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.statusStrip);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(461, 508);
            this.MinimumSize = new System.Drawing.Size(461, 508);
            this.Name = "EncoderWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Data Interface - Энкодер";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EncoderWindow_FormClosing);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox_text.ResumeLayout(false);
            this.groupBox_file.ResumeLayout(false);
            this.groupBox_file.PerformLayout();
            this.groupBox_properties.ResumeLayout(false);
            this.groupBox_properties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.groupBox_mode.ResumeLayout(false);
            this.groupBox_mode.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel label_encoding;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel label_percent;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.GroupBox groupBox_text;
        private System.Windows.Forms.GroupBox groupBox_file;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label_encodingDensity;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox_properties;
        private System.Windows.Forms.RadioButton radioButton_stereo;
        private System.Windows.Forms.RadioButton radioButton_mono;
        private System.Windows.Forms.Button button_convert;
        private System.Windows.Forms.Timer timer_controlHandler;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.CheckBox checkBox;
        private System.Windows.Forms.RadioButton radioButton_text;
        private System.Windows.Forms.RadioButton radioButton_file;
        private System.Windows.Forms.GroupBox groupBox_mode;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton button_debug;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}