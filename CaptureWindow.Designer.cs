
namespace AudioDataInterface
{
    partial class CaptureWindow
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
            this.comboBox_recDevices = new System.Windows.Forms.ComboBox();
            this.timer_drawWaveGraphFrame = new System.Windows.Forms.Timer(this.components);
            this.label_recDevice = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.button_debug = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_recDevices
            // 
            this.comboBox_recDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_recDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_recDevices.FormattingEnabled = true;
            this.comboBox_recDevices.Location = new System.Drawing.Point(151, 37);
            this.comboBox_recDevices.Name = "comboBox_recDevices";
            this.comboBox_recDevices.Size = new System.Drawing.Size(246, 21);
            this.comboBox_recDevices.TabIndex = 1;
            this.comboBox_recDevices.SelectedIndexChanged += new System.EventHandler(this.comboBox_recDevices_SelectedIndexChanged);
            // 
            // timer_drawWaveGraphFrame
            // 
            this.timer_drawWaveGraphFrame.Tick += new System.EventHandler(this.timer_drawWaveGraphFrame_Tick);
            // 
            // label_recDevice
            // 
            this.label_recDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_recDevice.AutoSize = true;
            this.label_recDevice.Location = new System.Drawing.Point(9, 40);
            this.label_recDevice.Name = "label_recDevice";
            this.label_recDevice.Size = new System.Drawing.Size(109, 13);
            this.label_recDevice.TabIndex = 2;
            this.label_recDevice.Text = "Устройство записи:";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 138);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(385, 296);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // button_clear
            // 
            this.button_clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_clear.Location = new System.Drawing.Point(12, 441);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(75, 23);
            this.button_clear.TabIndex = 4;
            this.button_clear.Text = "Очистить";
            this.button_clear.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(322, 441);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(241, 441);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 6;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(378, 64);
            this.trackBar.Maximum = 8;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar.Size = new System.Drawing.Size(45, 68);
            this.trackBar.TabIndex = 7;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar.Value = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 64);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(360, 68);
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightGray;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_debug});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(409, 25);
            this.toolStrip1.TabIndex = 9;
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
            this.button_debug.ToolTipText = "Отладка";
            this.button_debug.Click += new System.EventHandler(this.button_debug_Click);
            // 
            // CaptureWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(409, 473);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label_recDevice);
            this.Controls.Add(this.comboBox_recDevices);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(425, 483);
            this.Name = "CaptureWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Data Interface - Запись";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CaptureWindow_FormClosing);
            this.Load += new System.EventHandler(this.CaptureWindow_Load);
            this.ResizeEnd += new System.EventHandler(this.CaptureWindow_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox_recDevices;
        private System.Windows.Forms.Timer timer_drawWaveGraphFrame;
        private System.Windows.Forms.Label label_recDevice;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        public System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton button_debug;
    }
}