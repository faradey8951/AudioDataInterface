namespace AudioDataInterface
{
    partial class form_tapeRecordingWizard
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
            this.label_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.groupBox_file = new System.Windows.Forms.GroupBox();
            this.label_fileSize = new System.Windows.Forms.Label();
            this.button_fileSelect = new System.Windows.Forms.Button();
            this.textBox_filePath = new System.Windows.Forms.TextBox();
            this.label_filePath = new System.Windows.Forms.Label();
            this.groupBox_sheet = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.column_sector = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_progress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.button_record = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label_outputDevice = new System.Windows.Forms.Label();
            this.button_convert = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.groupBox_file.SuspendLayout();
            this.groupBox_sheet.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.label_status,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // label_status
            // 
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(43, 17);
            this.label_status.Text = "Статус";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // groupBox_file
            // 
            this.groupBox_file.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_file.Controls.Add(this.label_fileSize);
            this.groupBox_file.Controls.Add(this.button_fileSelect);
            this.groupBox_file.Controls.Add(this.textBox_filePath);
            this.groupBox_file.Controls.Add(this.label_filePath);
            this.groupBox_file.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_file.Location = new System.Drawing.Point(0, 0);
            this.groupBox_file.Name = "groupBox_file";
            this.groupBox_file.Size = new System.Drawing.Size(798, 66);
            this.groupBox_file.TabIndex = 1;
            this.groupBox_file.TabStop = false;
            this.groupBox_file.Text = "Файл";
            // 
            // label_fileSize
            // 
            this.label_fileSize.AutoSize = true;
            this.label_fileSize.Location = new System.Drawing.Point(64, 40);
            this.label_fileSize.Name = "label_fileSize";
            this.label_fileSize.Size = new System.Drawing.Size(75, 13);
            this.label_fileSize.TabIndex = 3;
            this.label_fileSize.Text = "Размер: 0 КБ";
            // 
            // button_fileSelect
            // 
            this.button_fileSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_fileSelect.Location = new System.Drawing.Point(713, 15);
            this.button_fileSelect.Name = "button_fileSelect";
            this.button_fileSelect.Size = new System.Drawing.Size(75, 23);
            this.button_fileSelect.TabIndex = 2;
            this.button_fileSelect.Text = "Выбрать";
            this.button_fileSelect.UseVisualStyleBackColor = true;
            this.button_fileSelect.Click += new System.EventHandler(this.button_fileSelect_Click);
            // 
            // textBox_filePath
            // 
            this.textBox_filePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_filePath.Location = new System.Drawing.Point(67, 17);
            this.textBox_filePath.Name = "textBox_filePath";
            this.textBox_filePath.Size = new System.Drawing.Size(630, 20);
            this.textBox_filePath.TabIndex = 1;
            // 
            // label_filePath
            // 
            this.label_filePath.AutoSize = true;
            this.label_filePath.Location = new System.Drawing.Point(13, 20);
            this.label_filePath.Name = "label_filePath";
            this.label_filePath.Size = new System.Drawing.Size(36, 13);
            this.label_filePath.TabIndex = 0;
            this.label_filePath.Text = "Файл";
            // 
            // groupBox_sheet
            // 
            this.groupBox_sheet.Controls.Add(this.listView1);
            this.groupBox_sheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_sheet.Location = new System.Drawing.Point(0, 66);
            this.groupBox_sheet.Name = "groupBox_sheet";
            this.groupBox_sheet.Size = new System.Drawing.Size(798, 321);
            this.groupBox_sheet.TabIndex = 3;
            this.groupBox_sheet.TabStop = false;
            this.groupBox_sheet.Text = "Представление";
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_sector,
            this.column_size,
            this.column_status,
            this.column_progress});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 16);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(792, 302);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // column_sector
            // 
            this.column_sector.Text = "Сектор";
            this.column_sector.Width = 80;
            // 
            // column_size
            // 
            this.column_size.Text = "Размер, байт";
            this.column_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_size.Width = 100;
            // 
            // column_status
            // 
            this.column_status.Text = "Статус";
            this.column_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_status.Width = 200;
            // 
            // column_progress
            // 
            this.column_progress.Text = "Прогресс";
            this.column_progress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_progress.Width = 80;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // button_record
            // 
            this.button_record.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_record.Location = new System.Drawing.Point(665, 398);
            this.button_record.Name = "button_record";
            this.button_record.Size = new System.Drawing.Size(123, 23);
            this.button_record.TabIndex = 4;
            this.button_record.Text = "Записать на ленту";
            this.button_record.UseVisualStyleBackColor = true;
            this.button_record.Click += new System.EventHandler(this.button_record_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox_sheet);
            this.panel1.Controls.Add(this.groupBox_file);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 389);
            this.panel1.TabIndex = 4;
            // 
            // timer_controlHandler
            // 
            this.timer_controlHandler.Tick += new System.EventHandler(this.timer_controlHandler_Tick);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(191, 400);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(191, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label_outputDevice
            // 
            this.label_outputDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_outputDevice.AutoSize = true;
            this.label_outputDevice.Location = new System.Drawing.Point(12, 403);
            this.label_outputDevice.Name = "label_outputDevice";
            this.label_outputDevice.Size = new System.Drawing.Size(173, 13);
            this.label_outputDevice.TabIndex = 6;
            this.label_outputDevice.Text = "Устройство для записи на ленту";
            // 
            // button_convert
            // 
            this.button_convert.Location = new System.Drawing.Point(544, 398);
            this.button_convert.Name = "button_convert";
            this.button_convert.Size = new System.Drawing.Size(104, 23);
            this.button_convert.TabIndex = 7;
            this.button_convert.Text = "Преобразовать";
            this.button_convert.UseVisualStyleBackColor = true;
            this.button_convert.Click += new System.EventHandler(this.button_convert_Click);
            // 
            // form_tapeRecordingWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_convert);
            this.Controls.Add(this.label_outputDevice);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button_record);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "form_tapeRecordingWizard";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Мастер записи на ленту";
            this.Load += new System.EventHandler(this.form_tapeRecordingWizard_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox_file.ResumeLayout(false);
            this.groupBox_file.PerformLayout();
            this.groupBox_sheet.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox groupBox_file;
        private System.Windows.Forms.Label label_fileSize;
        private System.Windows.Forms.Button button_fileSelect;
        private System.Windows.Forms.TextBox textBox_filePath;
        private System.Windows.Forms.Label label_filePath;
        private System.Windows.Forms.GroupBox groupBox_sheet;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader column_sector;
        private System.Windows.Forms.ColumnHeader column_size;
        private System.Windows.Forms.ColumnHeader column_status;
        private System.Windows.Forms.ColumnHeader column_progress;
        private System.Windows.Forms.ToolStripStatusLabel label_status;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button button_record;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer_controlHandler;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label_outputDevice;
        private System.Windows.Forms.Button button_convert;
    }
}