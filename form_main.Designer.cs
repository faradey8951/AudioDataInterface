
namespace AudioDataInterface
{
    partial class form_main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("0");
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.button_ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.button_encoder = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menu_remove = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.tabControl_dataControl = new System.Windows.Forms.TabControl();
            this.tabPage_tableView = new System.Windows.Forms.TabPage();
            this.listView = new System.Windows.Forms.ListView();
            this.column_block = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_markerCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_decodedData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage_graphicalView = new System.Windows.Forms.TabPage();
            this.panel_base = new System.Windows.Forms.Panel();
            this.panel_dataControl = new System.Windows.Forms.Panel();
            this.panel_signalCapture = new System.Windows.Forms.Panel();
            this.groupBox_signalCapture = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button_buffMp3 = new System.Windows.Forms.Button();
            this.groupBox_BIASAdjust = new System.Windows.Forms.GroupBox();
            this.radioButton_verticalBIAS = new System.Windows.Forms.RadioButton();
            this.radioButton_horizontalBIAS = new System.Windows.Forms.RadioButton();
            this.groupBox_scaleAdjust = new System.Windows.Forms.GroupBox();
            this.radioButton_verticalScale = new System.Windows.Forms.RadioButton();
            this.radioButton_horizontalScale = new System.Windows.Forms.RadioButton();
            this.label_recDevice = new System.Windows.Forms.Label();
            this.pictureBox_waveGraph = new System.Windows.Forms.PictureBox();
            this.comboBox_recDevices = new System.Windows.Forms.ComboBox();
            this.timer_drawWaveGraphFrame = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.tabControl_dataControl.SuspendLayout();
            this.tabPage_tableView.SuspendLayout();
            this.panel_base.SuspendLayout();
            this.panel_dataControl.SuspendLayout();
            this.panel_signalCapture.SuspendLayout();
            this.groupBox_signalCapture.SuspendLayout();
            this.groupBox_BIASAdjust.SuspendLayout();
            this.groupBox_scaleAdjust.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_waveGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.White;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справкаToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(844, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_,
            this.toolStripSeparator1,
            this.button_encoder,
            this.toolStripComboBox,
            this.toolStripSeparator3,
            this.toolStripLabel,
            this.toolStripSeparator2});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(844, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip";
            // 
            // button_
            // 
            this.button_.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_.Name = "button_";
            this.button_.Size = new System.Drawing.Size(50, 22);
            this.button_.Text = "Запись";
            this.button_.Click += new System.EventHandler(this.button_capture_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // button_encoder
            // 
            this.button_encoder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_encoder.Name = "button_encoder";
            this.button_encoder.Size = new System.Drawing.Size(82, 22);
            this.button_encoder.Text = "Кодировщик";
            this.button_encoder.Click += new System.EventHandler(this.button_encoder_Click);
            // 
            // toolStripComboBox
            // 
            this.toolStripComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.toolStripComboBox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripComboBox.Items.AddRange(new object[] {
            "Все блоки",
            "Маркеры",
            "Ошибки",
            "Только неисправимые ошибки"});
            this.toolStripComboBox.Name = "toolStripComboBox";
            this.toolStripComboBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripComboBox.Size = new System.Drawing.Size(195, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel
            // 
            this.toolStripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel.Name = "toolStripLabel";
            this.toolStripLabel.Size = new System.Drawing.Size(143, 22);
            this.toolStripLabel.Text = "Настройка отображения";
            this.toolStripLabel.Click += new System.EventHandler(this.toolStripLabel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.statusStrip.Location = new System.Drawing.Point(0, 616);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(844, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            this.statusStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip_ItemClicked);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_edit,
            this.menuSeparator,
            this.menu_remove});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(192, 54);
            // 
            // menu_edit
            // 
            this.menu_edit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menu_edit.Name = "menu_edit";
            this.menu_edit.Size = new System.Drawing.Size(191, 22);
            this.menu_edit.Text = "Редактировать блок";
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(188, 6);
            // 
            // menu_remove
            // 
            this.menu_remove.Image = global::AudioDataInterface.Properties.Resources.icon_remove;
            this.menu_remove.Name = "menu_remove";
            this.menu_remove.Size = new System.Drawing.Size(191, 22);
            this.menu_remove.Text = "Удалить";
            // 
            // timer_controlHandler
            // 
            this.timer_controlHandler.Enabled = true;
            this.timer_controlHandler.Interval = 25;
            this.timer_controlHandler.Tick += new System.EventHandler(this.timer_controlHandler_Tick);
            // 
            // tabControl_dataControl
            // 
            this.tabControl_dataControl.Controls.Add(this.tabPage_tableView);
            this.tabControl_dataControl.Controls.Add(this.tabPage_graphicalView);
            this.tabControl_dataControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_dataControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl_dataControl.Name = "tabControl_dataControl";
            this.tabControl_dataControl.SelectedIndex = 0;
            this.tabControl_dataControl.Size = new System.Drawing.Size(840, 367);
            this.tabControl_dataControl.TabIndex = 4;
            // 
            // tabPage_tableView
            // 
            this.tabPage_tableView.Controls.Add(this.listView);
            this.tabPage_tableView.Location = new System.Drawing.Point(4, 22);
            this.tabPage_tableView.Name = "tabPage_tableView";
            this.tabPage_tableView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_tableView.Size = new System.Drawing.Size(832, 341);
            this.tabPage_tableView.TabIndex = 0;
            this.tabPage_tableView.Text = "Таблица";
            this.tabPage_tableView.UseVisualStyleBackColor = true;
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_block,
            this.column_data,
            this.column_type,
            this.column_markerCode,
            this.column_decodedData});
            this.listView.ContextMenuStrip = this.contextMenuStrip;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView.Location = new System.Drawing.Point(3, 3);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(826, 335);
            this.listView.TabIndex = 4;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // column_block
            // 
            this.column_block.Text = "Block";
            this.column_block.Width = 50;
            // 
            // column_data
            // 
            this.column_data.Text = "Data";
            this.column_data.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_data.Width = 250;
            // 
            // column_type
            // 
            this.column_type.Text = "Type";
            this.column_type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // column_markerCode
            // 
            this.column_markerCode.Text = "Code";
            this.column_markerCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_markerCode.Width = 50;
            // 
            // column_decodedData
            // 
            this.column_decodedData.Text = "Decoded data";
            this.column_decodedData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.column_decodedData.Width = 210;
            // 
            // tabPage_graphicalView
            // 
            this.tabPage_graphicalView.Location = new System.Drawing.Point(4, 22);
            this.tabPage_graphicalView.Name = "tabPage_graphicalView";
            this.tabPage_graphicalView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_graphicalView.Size = new System.Drawing.Size(832, 341);
            this.tabPage_graphicalView.TabIndex = 1;
            this.tabPage_graphicalView.Text = "Графический";
            this.tabPage_graphicalView.UseVisualStyleBackColor = true;
            // 
            // panel_base
            // 
            this.panel_base.BackColor = System.Drawing.SystemColors.Control;
            this.panel_base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_base.Controls.Add(this.panel_dataControl);
            this.panel_base.Controls.Add(this.panel_signalCapture);
            this.panel_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_base.Location = new System.Drawing.Point(0, 49);
            this.panel_base.Name = "panel_base";
            this.panel_base.Size = new System.Drawing.Size(844, 567);
            this.panel_base.TabIndex = 5;
            // 
            // panel_dataControl
            // 
            this.panel_dataControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_dataControl.Controls.Add(this.tabControl_dataControl);
            this.panel_dataControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_dataControl.Location = new System.Drawing.Point(0, 0);
            this.panel_dataControl.Name = "panel_dataControl";
            this.panel_dataControl.Size = new System.Drawing.Size(842, 369);
            this.panel_dataControl.TabIndex = 5;
            // 
            // panel_signalCapture
            // 
            this.panel_signalCapture.BackColor = System.Drawing.SystemColors.Control;
            this.panel_signalCapture.Controls.Add(this.groupBox_signalCapture);
            this.panel_signalCapture.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_signalCapture.Location = new System.Drawing.Point(0, 369);
            this.panel_signalCapture.Name = "panel_signalCapture";
            this.panel_signalCapture.Size = new System.Drawing.Size(842, 196);
            this.panel_signalCapture.TabIndex = 6;
            // 
            // groupBox_signalCapture
            // 
            this.groupBox_signalCapture.Controls.Add(this.button1);
            this.groupBox_signalCapture.Controls.Add(this.button_buffMp3);
            this.groupBox_signalCapture.Controls.Add(this.groupBox_BIASAdjust);
            this.groupBox_signalCapture.Controls.Add(this.groupBox_scaleAdjust);
            this.groupBox_signalCapture.Controls.Add(this.label_recDevice);
            this.groupBox_signalCapture.Controls.Add(this.pictureBox_waveGraph);
            this.groupBox_signalCapture.Controls.Add(this.comboBox_recDevices);
            this.groupBox_signalCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_signalCapture.Location = new System.Drawing.Point(0, 0);
            this.groupBox_signalCapture.Name = "groupBox_signalCapture";
            this.groupBox_signalCapture.Size = new System.Drawing.Size(842, 196);
            this.groupBox_signalCapture.TabIndex = 12;
            this.groupBox_signalCapture.TabStop = false;
            this.groupBox_signalCapture.Text = "Захват сигнала";
            this.groupBox_signalCapture.Enter += new System.EventHandler(this.groupBox_signalCapture_Enter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(614, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "buff_mp3";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_buffMp3
            // 
            this.button_buffMp3.Location = new System.Drawing.Point(416, 160);
            this.button_buffMp3.Name = "button_buffMp3";
            this.button_buffMp3.Size = new System.Drawing.Size(75, 23);
            this.button_buffMp3.TabIndex = 14;
            this.button_buffMp3.Text = "buff_mp3";
            this.button_buffMp3.UseVisualStyleBackColor = true;
            this.button_buffMp3.Click += new System.EventHandler(this.button_buffMp3_Click);
            // 
            // groupBox_BIASAdjust
            // 
            this.groupBox_BIASAdjust.Controls.Add(this.radioButton_verticalBIAS);
            this.groupBox_BIASAdjust.Controls.Add(this.radioButton_horizontalBIAS);
            this.groupBox_BIASAdjust.Location = new System.Drawing.Point(159, 117);
            this.groupBox_BIASAdjust.Name = "groupBox_BIASAdjust";
            this.groupBox_BIASAdjust.Size = new System.Drawing.Size(145, 73);
            this.groupBox_BIASAdjust.TabIndex = 13;
            this.groupBox_BIASAdjust.TabStop = false;
            this.groupBox_BIASAdjust.Text = "Смещение развертки";
            // 
            // radioButton_verticalBIAS
            // 
            this.radioButton_verticalBIAS.AutoSize = true;
            this.radioButton_verticalBIAS.Location = new System.Drawing.Point(6, 43);
            this.radioButton_verticalBIAS.Name = "radioButton_verticalBIAS";
            this.radioButton_verticalBIAS.Size = new System.Drawing.Size(95, 17);
            this.radioButton_verticalBIAS.TabIndex = 3;
            this.radioButton_verticalBIAS.Text = "По вертикали";
            this.radioButton_verticalBIAS.UseVisualStyleBackColor = true;
            // 
            // radioButton_horizontalBIAS
            // 
            this.radioButton_horizontalBIAS.AutoSize = true;
            this.radioButton_horizontalBIAS.Checked = true;
            this.radioButton_horizontalBIAS.Location = new System.Drawing.Point(6, 20);
            this.radioButton_horizontalBIAS.Name = "radioButton_horizontalBIAS";
            this.radioButton_horizontalBIAS.Size = new System.Drawing.Size(106, 17);
            this.radioButton_horizontalBIAS.TabIndex = 2;
            this.radioButton_horizontalBIAS.TabStop = true;
            this.radioButton_horizontalBIAS.Text = "По горизонтали";
            this.radioButton_horizontalBIAS.UseVisualStyleBackColor = true;
            // 
            // groupBox_scaleAdjust
            // 
            this.groupBox_scaleAdjust.Controls.Add(this.radioButton_verticalScale);
            this.groupBox_scaleAdjust.Controls.Add(this.radioButton_horizontalScale);
            this.groupBox_scaleAdjust.Location = new System.Drawing.Point(8, 117);
            this.groupBox_scaleAdjust.Name = "groupBox_scaleAdjust";
            this.groupBox_scaleAdjust.Size = new System.Drawing.Size(145, 73);
            this.groupBox_scaleAdjust.TabIndex = 12;
            this.groupBox_scaleAdjust.TabStop = false;
            this.groupBox_scaleAdjust.Text = "Масштаб развертки";
            // 
            // radioButton_verticalScale
            // 
            this.radioButton_verticalScale.AutoSize = true;
            this.radioButton_verticalScale.Location = new System.Drawing.Point(7, 43);
            this.radioButton_verticalScale.Name = "radioButton_verticalScale";
            this.radioButton_verticalScale.Size = new System.Drawing.Size(95, 17);
            this.radioButton_verticalScale.TabIndex = 1;
            this.radioButton_verticalScale.Text = "По вертикали";
            this.radioButton_verticalScale.UseVisualStyleBackColor = true;
            // 
            // radioButton_horizontalScale
            // 
            this.radioButton_horizontalScale.AutoSize = true;
            this.radioButton_horizontalScale.Checked = true;
            this.radioButton_horizontalScale.Location = new System.Drawing.Point(7, 20);
            this.radioButton_horizontalScale.Name = "radioButton_horizontalScale";
            this.radioButton_horizontalScale.Size = new System.Drawing.Size(106, 17);
            this.radioButton_horizontalScale.TabIndex = 0;
            this.radioButton_horizontalScale.TabStop = true;
            this.radioButton_horizontalScale.Text = "По горизонтали";
            this.radioButton_horizontalScale.UseVisualStyleBackColor = true;
            // 
            // label_recDevice
            // 
            this.label_recDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_recDevice.AutoSize = true;
            this.label_recDevice.Location = new System.Drawing.Point(413, 120);
            this.label_recDevice.Name = "label_recDevice";
            this.label_recDevice.Size = new System.Drawing.Size(109, 13);
            this.label_recDevice.TabIndex = 11;
            this.label_recDevice.Text = "Устройство записи:";
            // 
            // pictureBox_waveGraph
            // 
            this.pictureBox_waveGraph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_waveGraph.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_waveGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_waveGraph.Location = new System.Drawing.Point(8, 15);
            this.pictureBox_waveGraph.Name = "pictureBox_waveGraph";
            this.pictureBox_waveGraph.Size = new System.Drawing.Size(826, 96);
            this.pictureBox_waveGraph.TabIndex = 9;
            this.pictureBox_waveGraph.TabStop = false;
            this.pictureBox_waveGraph.Click += new System.EventHandler(this.pictureBox_Click);
            this.pictureBox_waveGraph.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_waveGraph_MouseClick);
            this.pictureBox_waveGraph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_waveGraph_MouseDown);
            this.pictureBox_waveGraph.MouseEnter += new System.EventHandler(this.pictureBox_waveGraph_MouseEnter);
            this.pictureBox_waveGraph.MouseLeave += new System.EventHandler(this.pictureBox_waveGraph_MouseLeave);
            this.pictureBox_waveGraph.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_waveGraph_MouseUp);
            // 
            // comboBox_recDevices
            // 
            this.comboBox_recDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_recDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_recDevices.FormattingEnabled = true;
            this.comboBox_recDevices.Location = new System.Drawing.Point(540, 117);
            this.comboBox_recDevices.Name = "comboBox_recDevices";
            this.comboBox_recDevices.Size = new System.Drawing.Size(294, 21);
            this.comboBox_recDevices.TabIndex = 10;
            this.comboBox_recDevices.SelectedIndexChanged += new System.EventHandler(this.comboBox_recDevices_SelectedIndexChanged);
            // 
            // timer_drawWaveGraphFrame
            // 
            this.timer_drawWaveGraphFrame.Interval = 40;
            this.timer_drawWaveGraphFrame.Tick += new System.EventHandler(this.timer_drawWaveGraphFrame_Tick);
            // 
            // form_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(844, 638);
            this.Controls.Add(this.panel_base);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(860, 677);
            this.Name = "form_main";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Data Interface - Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResizeEnd += new System.EventHandler(this.form_main_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.form_main_SizeChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.tabControl_dataControl.ResumeLayout(false);
            this.tabPage_tableView.ResumeLayout(false);
            this.panel_base.ResumeLayout(false);
            this.panel_dataControl.ResumeLayout(false);
            this.panel_signalCapture.ResumeLayout(false);
            this.groupBox_signalCapture.ResumeLayout(false);
            this.groupBox_signalCapture.PerformLayout();
            this.groupBox_BIASAdjust.ResumeLayout(false);
            this.groupBox_BIASAdjust.PerformLayout();
            this.groupBox_scaleAdjust.ResumeLayout(false);
            this.groupBox_scaleAdjust.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_waveGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripButton button_;
        private System.Windows.Forms.ToolStripButton button_encoder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menu_edit;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private System.Windows.Forms.ToolStripMenuItem menu_remove;
        private System.Windows.Forms.Timer timer_controlHandler;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabControl_dataControl;
        private System.Windows.Forms.TabPage tabPage_tableView;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader column_block;
        private System.Windows.Forms.ColumnHeader column_data;
        private System.Windows.Forms.ColumnHeader column_type;
        private System.Windows.Forms.ColumnHeader column_markerCode;
        private System.Windows.Forms.ColumnHeader column_decodedData;
        private System.Windows.Forms.TabPage tabPage_graphicalView;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panel_base;
        private System.Windows.Forms.Panel panel_dataControl;
        private System.Windows.Forms.Panel panel_signalCapture;
        private System.Windows.Forms.PictureBox pictureBox_waveGraph;
        private System.Windows.Forms.GroupBox groupBox_signalCapture;
        private System.Windows.Forms.Label label_recDevice;
        private System.Windows.Forms.ComboBox comboBox_recDevices;
        private System.Windows.Forms.Timer timer_drawWaveGraphFrame;
        private System.Windows.Forms.GroupBox groupBox_BIASAdjust;
        private System.Windows.Forms.RadioButton radioButton_verticalBIAS;
        private System.Windows.Forms.RadioButton radioButton_horizontalBIAS;
        private System.Windows.Forms.GroupBox groupBox_scaleAdjust;
        private System.Windows.Forms.RadioButton radioButton_verticalScale;
        private System.Windows.Forms.RadioButton radioButton_horizontalScale;
        private System.Windows.Forms.Button button_buffMp3;
        private System.Windows.Forms.Button button1;
    }
}

