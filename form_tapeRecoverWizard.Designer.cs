namespace AudioDataInterface
{
    partial class form_tapeRecoverWizard
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
            this.column_warnings = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.button_recover = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_sheet = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.column_sector = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_progress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox_file = new System.Windows.Forms.GroupBox();
            this.label_fileName = new System.Windows.Forms.Label();
            this.label_fileSize = new System.Windows.Forms.Label();
            this.label_filePath = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.label_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.button_resetFile = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox_sheet.SuspendLayout();
            this.groupBox_file.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // column_warnings
            // 
            this.column_warnings.Text = "Предупреждения";
            this.column_warnings.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_warnings.Width = 200;
            // 
            // button_recover
            // 
            this.button_recover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_recover.Location = new System.Drawing.Point(695, 487);
            this.button_recover.Name = "button_recover";
            this.button_recover.Size = new System.Drawing.Size(146, 23);
            this.button_recover.TabIndex = 9;
            this.button_recover.Text = "Восстановить с ленты";
            this.button_recover.UseVisualStyleBackColor = true;
            this.button_recover.Click += new System.EventHandler(this.button_recover_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox_sheet);
            this.panel1.Controls.Add(this.groupBox_file);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(853, 479);
            this.panel1.TabIndex = 10;
            // 
            // groupBox_sheet
            // 
            this.groupBox_sheet.Controls.Add(this.listView1);
            this.groupBox_sheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_sheet.Location = new System.Drawing.Point(0, 66);
            this.groupBox_sheet.Name = "groupBox_sheet";
            this.groupBox_sheet.Size = new System.Drawing.Size(851, 411);
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
            this.column_progress,
            this.column_warnings});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 16);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(845, 392);
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
            // groupBox_file
            // 
            this.groupBox_file.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_file.Controls.Add(this.label_fileName);
            this.groupBox_file.Controls.Add(this.label_fileSize);
            this.groupBox_file.Controls.Add(this.label_filePath);
            this.groupBox_file.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_file.Location = new System.Drawing.Point(0, 0);
            this.groupBox_file.Name = "groupBox_file";
            this.groupBox_file.Size = new System.Drawing.Size(851, 66);
            this.groupBox_file.TabIndex = 1;
            this.groupBox_file.TabStop = false;
            this.groupBox_file.Text = "Файл";
            this.groupBox_file.Enter += new System.EventHandler(this.groupBox_file_Enter);
            // 
            // label_fileName
            // 
            this.label_fileName.AutoSize = true;
            this.label_fileName.Location = new System.Drawing.Point(64, 20);
            this.label_fileName.Name = "label_fileName";
            this.label_fileName.Size = new System.Drawing.Size(127, 13);
            this.label_fileName.TabIndex = 4;
            this.label_fileName.Text = "имя файла.расширение";
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
            // label_filePath
            // 
            this.label_filePath.AutoSize = true;
            this.label_filePath.Location = new System.Drawing.Point(13, 20);
            this.label_filePath.Name = "label_filePath";
            this.label_filePath.Size = new System.Drawing.Size(36, 13);
            this.label_filePath.TabIndex = 0;
            this.label_filePath.Text = "Файл";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // label_status
            // 
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(43, 17);
            this.label_status.Text = "Статус";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.label_status,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 518);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(853, 22);
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";
            // 
            // button_resetFile
            // 
            this.button_resetFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_resetFile.Location = new System.Drawing.Point(592, 487);
            this.button_resetFile.Name = "button_resetFile";
            this.button_resetFile.Size = new System.Drawing.Size(97, 23);
            this.button_resetFile.TabIndex = 11;
            this.button_resetFile.Text = "Сбросить файл";
            this.button_resetFile.UseVisualStyleBackColor = true;
            // 
            // form_tapeRecoverWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(853, 540);
            this.Controls.Add(this.button_resetFile);
            this.Controls.Add(this.button_recover);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(869, 579);
            this.Name = "form_tapeRecoverWizard";
            this.ShowIcon = false;
            this.Text = "Мастер восстановления данных с ленты";
            this.panel1.ResumeLayout(false);
            this.groupBox_sheet.ResumeLayout(false);
            this.groupBox_file.ResumeLayout(false);
            this.groupBox_file.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader column_warnings;
        private System.Windows.Forms.Timer timer_controlHandler;
        private System.Windows.Forms.Button button_recover;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox_sheet;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader column_sector;
        private System.Windows.Forms.ColumnHeader column_size;
        private System.Windows.Forms.ColumnHeader column_status;
        private System.Windows.Forms.ColumnHeader column_progress;
        private System.Windows.Forms.GroupBox groupBox_file;
        private System.Windows.Forms.Label label_fileSize;
        private System.Windows.Forms.Label label_filePath;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel label_status;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Label label_fileName;
        private System.Windows.Forms.Button button_resetFile;
    }
}