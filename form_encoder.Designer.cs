
namespace AudioDataInterface
{
    partial class form_encoder
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
            this.groupBox_file = new System.Windows.Forms.GroupBox();
            this.groupBox_properties = new System.Windows.Forms.GroupBox();
            this.checkBox_longLeadIn = new System.Windows.Forms.CheckBox();
            this.trackBar_trackCount = new System.Windows.Forms.TrackBar();
            this.label_trackNumber = new System.Windows.Forms.Label();
            this.label_trackCount = new System.Windows.Forms.Label();
            this.trackBar_trackNumber = new System.Windows.Forms.TrackBar();
            this.textBox = new System.Windows.Forms.TextBox();
            this.button_select = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button_convert = new System.Windows.Forms.Button();
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.button_clear = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip.SuspendLayout();
            this.groupBox_file.SuspendLayout();
            this.groupBox_properties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_trackCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_trackNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.label_encoding,
            this.progressBar,
            this.label_percent});
            this.statusStrip.Location = new System.Drawing.Point(0, 187);
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
            this.button_ok.Location = new System.Drawing.Point(358, 161);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // groupBox_file
            // 
            this.groupBox_file.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox_file.Controls.Add(this.groupBox_properties);
            this.groupBox_file.Controls.Add(this.textBox);
            this.groupBox_file.Controls.Add(this.button_select);
            this.groupBox_file.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_file.Location = new System.Drawing.Point(0, 0);
            this.groupBox_file.Name = "groupBox_file";
            this.groupBox_file.Size = new System.Drawing.Size(445, 209);
            this.groupBox_file.TabIndex = 12;
            this.groupBox_file.TabStop = false;
            this.groupBox_file.Text = "MPS-SDI";
            // 
            // groupBox_properties
            // 
            this.groupBox_properties.Controls.Add(this.checkBox_longLeadIn);
            this.groupBox_properties.Controls.Add(this.trackBar_trackCount);
            this.groupBox_properties.Controls.Add(this.label_trackNumber);
            this.groupBox_properties.Controls.Add(this.label_trackCount);
            this.groupBox_properties.Controls.Add(this.trackBar_trackNumber);
            this.groupBox_properties.Location = new System.Drawing.Point(4, 46);
            this.groupBox_properties.Name = "groupBox_properties";
            this.groupBox_properties.Size = new System.Drawing.Size(435, 106);
            this.groupBox_properties.TabIndex = 7;
            this.groupBox_properties.TabStop = false;
            this.groupBox_properties.Text = "Настройки";
            // 
            // checkBox_longLeadIn
            // 
            this.checkBox_longLeadIn.AutoSize = true;
            this.checkBox_longLeadIn.Location = new System.Drawing.Point(8, 72);
            this.checkBox_longLeadIn.Name = "checkBox_longLeadIn";
            this.checkBox_longLeadIn.Size = new System.Drawing.Size(171, 17);
            this.checkBox_longLeadIn.TabIndex = 6;
            this.checkBox_longLeadIn.Text = "Длинная входящая дорожка";
            this.checkBox_longLeadIn.UseVisualStyleBackColor = true;
            this.checkBox_longLeadIn.CheckedChanged += new System.EventHandler(this.checkBox_longLeadIn_CheckedChanged);
            // 
            // trackBar_trackCount
            // 
            this.trackBar_trackCount.Location = new System.Drawing.Point(170, 50);
            this.trackBar_trackCount.Maximum = 99;
            this.trackBar_trackCount.Minimum = 1;
            this.trackBar_trackCount.Name = "trackBar_trackCount";
            this.trackBar_trackCount.Size = new System.Drawing.Size(259, 45);
            this.trackBar_trackCount.TabIndex = 5;
            this.trackBar_trackCount.TickFrequency = 5;
            this.trackBar_trackCount.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_trackCount.Value = 1;
            this.trackBar_trackCount.Scroll += new System.EventHandler(this.trackBar_trackCount_Scroll);
            // 
            // label_trackNumber
            // 
            this.label_trackNumber.AutoSize = true;
            this.label_trackNumber.Location = new System.Drawing.Point(6, 16);
            this.label_trackNumber.Name = "label_trackNumber";
            this.label_trackNumber.Size = new System.Drawing.Size(100, 13);
            this.label_trackNumber.TabIndex = 2;
            this.label_trackNumber.Text = "Номер дорожки: 1";
            // 
            // label_trackCount
            // 
            this.label_trackCount.AutoSize = true;
            this.label_trackCount.Location = new System.Drawing.Point(6, 50);
            this.label_trackCount.Name = "label_trackCount";
            this.label_trackCount.Size = new System.Drawing.Size(125, 13);
            this.label_trackCount.TabIndex = 4;
            this.label_trackCount.Text = "Количество дорожек: 1";
            // 
            // trackBar_trackNumber
            // 
            this.trackBar_trackNumber.Location = new System.Drawing.Point(170, 16);
            this.trackBar_trackNumber.Maximum = 99;
            this.trackBar_trackNumber.Minimum = 1;
            this.trackBar_trackNumber.Name = "trackBar_trackNumber";
            this.trackBar_trackNumber.Size = new System.Drawing.Size(259, 45);
            this.trackBar_trackNumber.TabIndex = 3;
            this.trackBar_trackNumber.TickFrequency = 5;
            this.trackBar_trackNumber.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_trackNumber.Value = 1;
            this.trackBar_trackNumber.Scroll += new System.EventHandler(this.trackBar_trackNumber_Scroll);
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.SystemColors.Window;
            this.textBox.Location = new System.Drawing.Point(6, 19);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(350, 20);
            this.textBox.TabIndex = 0;
            // 
            // button_select
            // 
            this.button_select.Location = new System.Drawing.Point(362, 17);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(75, 23);
            this.button_select.TabIndex = 6;
            this.button_select.Text = "Выбрать";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // button_convert
            // 
            this.button_convert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_convert.Location = new System.Drawing.Point(253, 161);
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
            this.button_clear.Location = new System.Drawing.Point(13, 161);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(75, 23);
            this.button_clear.TabIndex = 7;
            this.button_clear.Text = "Очистить";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // form_encoder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(445, 209);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_convert);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox_file);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(461, 248);
            this.Name = "form_encoder";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Data Interface - Энкодер";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EncoderWindow_FormClosing);
            this.Load += new System.EventHandler(this.EncoderWindow_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox_file.ResumeLayout(false);
            this.groupBox_file.PerformLayout();
            this.groupBox_properties.ResumeLayout(false);
            this.groupBox_properties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_trackCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_trackNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel label_encoding;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel label_percent;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.GroupBox groupBox_file;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox_properties;
        private System.Windows.Forms.Button button_convert;
        private System.Windows.Forms.Timer timer_controlHandler;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TrackBar trackBar_trackCount;
        private System.Windows.Forms.Label label_trackNumber;
        private System.Windows.Forms.Label label_trackCount;
        private System.Windows.Forms.TrackBar trackBar_trackNumber;
        private System.Windows.Forms.CheckBox checkBox_longLeadIn;
    }
}