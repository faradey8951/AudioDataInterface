
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_main));
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
            this.button_settings = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menu_remove = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_controlHandler = new System.Windows.Forms.Timer(this.components);
            this.tabControl_dataControl = new System.Windows.Forms.TabControl();
            this.tabPage_graphicalView = new System.Windows.Forms.TabPage();
            this.pictureBox_cassette = new System.Windows.Forms.PictureBox();
            this.pictureBox_disc3 = new System.Windows.Forms.PictureBox();
            this.pictureBox_disc2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_disc1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_playPause = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol10 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol9 = new System.Windows.Forms.PictureBox();
            this.pictureBox_dots = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol8 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol7 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol6 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol5 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol4 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol3 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_symbol1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_mpsPlayer = new System.Windows.Forms.PictureBox();
            this.panel_base = new System.Windows.Forms.Panel();
            this.panel_dataControl = new System.Windows.Forms.Panel();
            this.groupBox_info = new System.Windows.Forms.GroupBox();
            this.label_frameSyncErrorCount = new System.Windows.Forms.Label();
            this.label_unfixedErrorCount = new System.Windows.Forms.Label();
            this.label_fixedErrorCount = new System.Windows.Forms.Label();
            this.label_border2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_border1 = new System.Windows.Forms.Label();
            this.label_signalGainL = new System.Windows.Forms.Label();
            this.label_signalGainR = new System.Windows.Forms.Label();
            this.panel_signalCapture = new System.Windows.Forms.Panel();
            this.checkBox_tapeSkin = new System.Windows.Forms.CheckBox();
            this.groupBox_signalCapture = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_playDevices = new System.Windows.Forms.ComboBox();
            this.groupBox_decoderSettings = new System.Windows.Forms.GroupBox();
            this.checkBox_autoGain = new System.Windows.Forms.CheckBox();
            this.checkBox_remainingTime = new System.Windows.Forms.CheckBox();
            this.checkBox_invertSignal = new System.Windows.Forms.CheckBox();
            this.groupBox_BIASAdjust = new System.Windows.Forms.GroupBox();
            this.radioButton_verticalBIAS = new System.Windows.Forms.RadioButton();
            this.radioButton_horizontalBIAS = new System.Windows.Forms.RadioButton();
            this.groupBox_scaleAdjust = new System.Windows.Forms.GroupBox();
            this.radioButton_verticalScale = new System.Windows.Forms.RadioButton();
            this.radioButton_horizontalScale = new System.Windows.Forms.RadioButton();
            this.label_recDevice = new System.Windows.Forms.Label();
            this.pictureBox_waveGraph = new System.Windows.Forms.PictureBox();
            this.comboBox_recDevices = new System.Windows.Forms.ComboBox();
            this.button_buffMp3 = new System.Windows.Forms.Button();
            this.timer_drawWaveGraphFrame = new System.Windows.Forms.Timer(this.components);
            this.timer_mpsPlayerHandler = new System.Windows.Forms.Timer(this.components);
            this.timer_mpsPlayerSpectrumHandler = new System.Windows.Forms.Timer(this.components);
            this.timer_mpsPlayerSpectrumUpdater = new System.Windows.Forms.Timer(this.components);
            this.timer_mpsPlayerTimeUpdater = new System.Windows.Forms.Timer(this.components);
            this.pictureBox_runningIndicator = new System.Windows.Forms.PictureBox();
            this.pictureBox_track1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track2 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track3 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track4 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track5 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track6 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track7 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track8 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track9 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track10 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track11 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track13 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track14 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track15 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track16 = new System.Windows.Forms.PictureBox();
            this.pictureBox_track12 = new System.Windows.Forms.PictureBox();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.tabControl_dataControl.SuspendLayout();
            this.tabPage_graphicalView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_cassette)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disc3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disc2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disc1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_playPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_dots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_mpsPlayer)).BeginInit();
            this.panel_base.SuspendLayout();
            this.panel_dataControl.SuspendLayout();
            this.groupBox_info.SuspendLayout();
            this.panel_signalCapture.SuspendLayout();
            this.groupBox_signalCapture.SuspendLayout();
            this.groupBox_decoderSettings.SuspendLayout();
            this.groupBox_BIASAdjust.SuspendLayout();
            this.groupBox_scaleAdjust.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_waveGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_runningIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track12)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.White;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справкаToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1024, 24);
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
            this.toolStripSeparator2,
            this.button_settings});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1024, 25);
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
            // button_settings
            // 
            this.button_settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_settings.Image = ((System.Drawing.Image)(resources.GetObject("button_settings.Image")));
            this.button_settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(71, 22);
            this.button_settings.Text = "Настройки";
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.statusStrip.Location = new System.Drawing.Point(0, 616);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1024, 22);
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
            this.tabControl_dataControl.Controls.Add(this.tabPage_graphicalView);
            this.tabControl_dataControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl_dataControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl_dataControl.Name = "tabControl_dataControl";
            this.tabControl_dataControl.SelectedIndex = 0;
            this.tabControl_dataControl.Size = new System.Drawing.Size(824, 367);
            this.tabControl_dataControl.TabIndex = 4;
            // 
            // tabPage_graphicalView
            // 
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_cassette);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_disc3);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_disc2);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_disc1);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_playPause);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol10);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol9);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_dots);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol8);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol7);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol6);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol5);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol4);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol3);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol2);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_symbol1);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox2);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox1);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track12);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track16);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track15);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track14);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track13);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track11);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track10);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track9);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track8);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track7);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track6);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track5);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track4);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track3);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track2);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_track1);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_runningIndicator);
            this.tabPage_graphicalView.Controls.Add(this.pictureBox_mpsPlayer);
            this.tabPage_graphicalView.Location = new System.Drawing.Point(4, 22);
            this.tabPage_graphicalView.Name = "tabPage_graphicalView";
            this.tabPage_graphicalView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_graphicalView.Size = new System.Drawing.Size(816, 341);
            this.tabPage_graphicalView.TabIndex = 1;
            this.tabPage_graphicalView.Text = "Графический";
            this.tabPage_graphicalView.UseVisualStyleBackColor = true;
            // 
            // pictureBox_cassette
            // 
            this.pictureBox_cassette.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_cassette.Image = global::AudioDataInterface.Properties.Resources.cassette;
            this.pictureBox_cassette.Location = new System.Drawing.Point(8, 168);
            this.pictureBox_cassette.Name = "pictureBox_cassette";
            this.pictureBox_cassette.Size = new System.Drawing.Size(33, 22);
            this.pictureBox_cassette.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_cassette.TabIndex = 35;
            this.pictureBox_cassette.TabStop = false;
            // 
            // pictureBox_disc3
            // 
            this.pictureBox_disc3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_disc3.Image = global::AudioDataInterface.Properties.Resources.disc3Empty;
            this.pictureBox_disc3.Location = new System.Drawing.Point(576, 112);
            this.pictureBox_disc3.Name = "pictureBox_disc3";
            this.pictureBox_disc3.Size = new System.Drawing.Size(48, 40);
            this.pictureBox_disc3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_disc3.TabIndex = 34;
            this.pictureBox_disc3.TabStop = false;
            // 
            // pictureBox_disc2
            // 
            this.pictureBox_disc2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_disc2.Image = global::AudioDataInterface.Properties.Resources.disc2Empty;
            this.pictureBox_disc2.Location = new System.Drawing.Point(528, 112);
            this.pictureBox_disc2.Name = "pictureBox_disc2";
            this.pictureBox_disc2.Size = new System.Drawing.Size(48, 40);
            this.pictureBox_disc2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_disc2.TabIndex = 33;
            this.pictureBox_disc2.TabStop = false;
            // 
            // pictureBox_disc1
            // 
            this.pictureBox_disc1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_disc1.Image = global::AudioDataInterface.Properties.Resources.disc1Selected;
            this.pictureBox_disc1.Location = new System.Drawing.Point(480, 112);
            this.pictureBox_disc1.Name = "pictureBox_disc1";
            this.pictureBox_disc1.Size = new System.Drawing.Size(48, 40);
            this.pictureBox_disc1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_disc1.TabIndex = 32;
            this.pictureBox_disc1.TabStop = false;
            // 
            // pictureBox_playPause
            // 
            this.pictureBox_playPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_playPause.Image = global::AudioDataInterface.Properties.Resources.play;
            this.pictureBox_playPause.Location = new System.Drawing.Point(488, 80);
            this.pictureBox_playPause.Name = "pictureBox_playPause";
            this.pictureBox_playPause.Size = new System.Drawing.Size(45, 25);
            this.pictureBox_playPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_playPause.TabIndex = 31;
            this.pictureBox_playPause.TabStop = false;
            // 
            // pictureBox_symbol10
            // 
            this.pictureBox_symbol10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol10.Image = global::AudioDataInterface.Properties.Resources._0symbol;
            this.pictureBox_symbol10.Location = new System.Drawing.Point(392, 96);
            this.pictureBox_symbol10.Name = "pictureBox_symbol10";
            this.pictureBox_symbol10.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol10.TabIndex = 30;
            this.pictureBox_symbol10.TabStop = false;
            // 
            // pictureBox_symbol9
            // 
            this.pictureBox_symbol9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol9.Image = global::AudioDataInterface.Properties.Resources._0symbol;
            this.pictureBox_symbol9.Location = new System.Drawing.Point(352, 96);
            this.pictureBox_symbol9.Name = "pictureBox_symbol9";
            this.pictureBox_symbol9.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol9.TabIndex = 29;
            this.pictureBox_symbol9.TabStop = false;
            // 
            // pictureBox_dots
            // 
            this.pictureBox_dots.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_dots.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_dots.Image")));
            this.pictureBox_dots.Location = new System.Drawing.Point(344, 108);
            this.pictureBox_dots.Name = "pictureBox_dots";
            this.pictureBox_dots.Size = new System.Drawing.Size(5, 36);
            this.pictureBox_dots.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_dots.TabIndex = 28;
            this.pictureBox_dots.TabStop = false;
            // 
            // pictureBox_symbol8
            // 
            this.pictureBox_symbol8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol8.Image = global::AudioDataInterface.Properties.Resources._0symbol;
            this.pictureBox_symbol8.Location = new System.Drawing.Point(288, 96);
            this.pictureBox_symbol8.Name = "pictureBox_symbol8";
            this.pictureBox_symbol8.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol8.TabIndex = 27;
            this.pictureBox_symbol8.TabStop = false;
            // 
            // pictureBox_symbol7
            // 
            this.pictureBox_symbol7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol7.Location = new System.Drawing.Point(248, 96);
            this.pictureBox_symbol7.Name = "pictureBox_symbol7";
            this.pictureBox_symbol7.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol7.TabIndex = 26;
            this.pictureBox_symbol7.TabStop = false;
            // 
            // pictureBox_symbol6
            // 
            this.pictureBox_symbol6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol6.Location = new System.Drawing.Point(208, 96);
            this.pictureBox_symbol6.Name = "pictureBox_symbol6";
            this.pictureBox_symbol6.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol6.TabIndex = 25;
            this.pictureBox_symbol6.TabStop = false;
            // 
            // pictureBox_symbol5
            // 
            this.pictureBox_symbol5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol5.Image = global::AudioDataInterface.Properties.Resources.DASHsymbol;
            this.pictureBox_symbol5.Location = new System.Drawing.Point(168, 96);
            this.pictureBox_symbol5.Name = "pictureBox_symbol5";
            this.pictureBox_symbol5.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol5.TabIndex = 24;
            this.pictureBox_symbol5.TabStop = false;
            // 
            // pictureBox_symbol4
            // 
            this.pictureBox_symbol4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol4.Image = global::AudioDataInterface.Properties.Resources.DASHsymbol;
            this.pictureBox_symbol4.Location = new System.Drawing.Point(128, 96);
            this.pictureBox_symbol4.Name = "pictureBox_symbol4";
            this.pictureBox_symbol4.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol4.TabIndex = 23;
            this.pictureBox_symbol4.TabStop = false;
            // 
            // pictureBox_symbol3
            // 
            this.pictureBox_symbol3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol3.Location = new System.Drawing.Point(88, 96);
            this.pictureBox_symbol3.Name = "pictureBox_symbol3";
            this.pictureBox_symbol3.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol3.TabIndex = 22;
            this.pictureBox_symbol3.TabStop = false;
            // 
            // pictureBox_symbol2
            // 
            this.pictureBox_symbol2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol2.Image = global::AudioDataInterface.Properties.Resources.Psymbol;
            this.pictureBox_symbol2.Location = new System.Drawing.Point(48, 96);
            this.pictureBox_symbol2.Name = "pictureBox_symbol2";
            this.pictureBox_symbol2.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol2.TabIndex = 21;
            this.pictureBox_symbol2.TabStop = false;
            this.pictureBox_symbol2.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // pictureBox_symbol1
            // 
            this.pictureBox_symbol1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_symbol1.Image = global::AudioDataInterface.Properties.Resources.Msymbol;
            this.pictureBox_symbol1.Location = new System.Drawing.Point(8, 96);
            this.pictureBox_symbol1.Name = "pictureBox_symbol1";
            this.pictureBox_symbol1.Size = new System.Drawing.Size(40, 55);
            this.pictureBox_symbol1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_symbol1.TabIndex = 20;
            this.pictureBox_symbol1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox2.Image = global::AudioDataInterface.Properties.Resources.border;
            this.pictureBox2.Location = new System.Drawing.Point(580, 196);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(15, 142);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 19;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox1.Image = global::AudioDataInterface.Properties.Resources.border;
            this.pictureBox1.Location = new System.Drawing.Point(3, 197);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 142);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox_mpsPlayer
            // 
            this.pictureBox_mpsPlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_mpsPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_mpsPlayer.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_mpsPlayer.Name = "pictureBox_mpsPlayer";
            this.pictureBox_mpsPlayer.Size = new System.Drawing.Size(810, 335);
            this.pictureBox_mpsPlayer.TabIndex = 0;
            this.pictureBox_mpsPlayer.TabStop = false;
            this.pictureBox_mpsPlayer.Click += new System.EventHandler(this.pictureBox_mpsPlayer_Click);
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
            this.panel_base.Size = new System.Drawing.Size(1024, 567);
            this.panel_base.TabIndex = 5;
            // 
            // panel_dataControl
            // 
            this.panel_dataControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_dataControl.Controls.Add(this.groupBox_info);
            this.panel_dataControl.Controls.Add(this.tabControl_dataControl);
            this.panel_dataControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_dataControl.Location = new System.Drawing.Point(0, 0);
            this.panel_dataControl.Name = "panel_dataControl";
            this.panel_dataControl.Size = new System.Drawing.Size(1022, 369);
            this.panel_dataControl.TabIndex = 5;
            // 
            // groupBox_info
            // 
            this.groupBox_info.Controls.Add(this.label_frameSyncErrorCount);
            this.groupBox_info.Controls.Add(this.label_unfixedErrorCount);
            this.groupBox_info.Controls.Add(this.label_fixedErrorCount);
            this.groupBox_info.Controls.Add(this.label_border2);
            this.groupBox_info.Controls.Add(this.label3);
            this.groupBox_info.Controls.Add(this.label_border1);
            this.groupBox_info.Controls.Add(this.label_signalGainL);
            this.groupBox_info.Controls.Add(this.label_signalGainR);
            this.groupBox_info.Location = new System.Drawing.Point(824, 22);
            this.groupBox_info.Name = "groupBox_info";
            this.groupBox_info.Size = new System.Drawing.Size(192, 339);
            this.groupBox_info.TabIndex = 5;
            this.groupBox_info.TabStop = false;
            this.groupBox_info.Text = "Информация";
            // 
            // label_frameSyncErrorCount
            // 
            this.label_frameSyncErrorCount.AutoSize = true;
            this.label_frameSyncErrorCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_frameSyncErrorCount.Location = new System.Drawing.Point(3, 107);
            this.label_frameSyncErrorCount.Name = "label_frameSyncErrorCount";
            this.label_frameSyncErrorCount.Size = new System.Drawing.Size(94, 13);
            this.label_frameSyncErrorCount.TabIndex = 4;
            this.label_frameSyncErrorCount.Text = "Кадровая синхр.:";
            // 
            // label_unfixedErrorCount
            // 
            this.label_unfixedErrorCount.AutoSize = true;
            this.label_unfixedErrorCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_unfixedErrorCount.Location = new System.Drawing.Point(3, 94);
            this.label_unfixedErrorCount.Name = "label_unfixedErrorCount";
            this.label_unfixedErrorCount.Size = new System.Drawing.Size(88, 13);
            this.label_unfixedErrorCount.TabIndex = 3;
            this.label_unfixedErrorCount.Text = "Неисправимые:";
            // 
            // label_fixedErrorCount
            // 
            this.label_fixedErrorCount.AutoSize = true;
            this.label_fixedErrorCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_fixedErrorCount.Location = new System.Drawing.Point(3, 81);
            this.label_fixedErrorCount.Name = "label_fixedErrorCount";
            this.label_fixedErrorCount.Size = new System.Drawing.Size(72, 13);
            this.label_fixedErrorCount.TabIndex = 2;
            this.label_fixedErrorCount.Text = "Исправлено:";
            // 
            // label_border2
            // 
            this.label_border2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_border2.Location = new System.Drawing.Point(3, 68);
            this.label_border2.Name = "label_border2";
            this.label_border2.Size = new System.Drawing.Size(186, 13);
            this.label_border2.TabIndex = 5;
            this.label_border2.Text = "____________________________";
            this.label_border2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ошибки";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label_border1
            // 
            this.label_border1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_border1.Location = new System.Drawing.Point(3, 42);
            this.label_border1.Name = "label_border1";
            this.label_border1.Size = new System.Drawing.Size(186, 13);
            this.label_border1.TabIndex = 7;
            this.label_border1.Text = "____________________________";
            this.label_border1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_signalGainL
            // 
            this.label_signalGainL.AutoSize = true;
            this.label_signalGainL.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_signalGainL.Location = new System.Drawing.Point(3, 29);
            this.label_signalGainL.Name = "label_signalGainL";
            this.label_signalGainL.Size = new System.Drawing.Size(151, 13);
            this.label_signalGainL.TabIndex = 0;
            this.label_signalGainL.Text = "Коэффициент усиления ЛК: ";
            // 
            // label_signalGainR
            // 
            this.label_signalGainR.AutoSize = true;
            this.label_signalGainR.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_signalGainR.Location = new System.Drawing.Point(3, 16);
            this.label_signalGainR.Name = "label_signalGainR";
            this.label_signalGainR.Size = new System.Drawing.Size(151, 13);
            this.label_signalGainR.TabIndex = 1;
            this.label_signalGainR.Text = "Коэффициент усиления ПК: ";
            // 
            // panel_signalCapture
            // 
            this.panel_signalCapture.BackColor = System.Drawing.SystemColors.Control;
            this.panel_signalCapture.Controls.Add(this.checkBox_tapeSkin);
            this.panel_signalCapture.Controls.Add(this.groupBox_signalCapture);
            this.panel_signalCapture.Controls.Add(this.button_buffMp3);
            this.panel_signalCapture.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_signalCapture.Location = new System.Drawing.Point(0, 369);
            this.panel_signalCapture.Name = "panel_signalCapture";
            this.panel_signalCapture.Size = new System.Drawing.Size(1022, 196);
            this.panel_signalCapture.TabIndex = 6;
            // 
            // checkBox_tapeSkin
            // 
            this.checkBox_tapeSkin.AutoSize = true;
            this.checkBox_tapeSkin.Location = new System.Drawing.Point(848, 8);
            this.checkBox_tapeSkin.Name = "checkBox_tapeSkin";
            this.checkBox_tapeSkin.Size = new System.Drawing.Size(78, 17);
            this.checkBox_tapeSkin.TabIndex = 16;
            this.checkBox_tapeSkin.Text = "TAPE Skin";
            this.checkBox_tapeSkin.UseVisualStyleBackColor = true;
            this.checkBox_tapeSkin.CheckedChanged += new System.EventHandler(this.checkBox_tapeSkin_CheckedChanged);
            // 
            // groupBox_signalCapture
            // 
            this.groupBox_signalCapture.Controls.Add(this.label1);
            this.groupBox_signalCapture.Controls.Add(this.comboBox_playDevices);
            this.groupBox_signalCapture.Controls.Add(this.groupBox_decoderSettings);
            this.groupBox_signalCapture.Controls.Add(this.groupBox_BIASAdjust);
            this.groupBox_signalCapture.Controls.Add(this.groupBox_scaleAdjust);
            this.groupBox_signalCapture.Controls.Add(this.label_recDevice);
            this.groupBox_signalCapture.Controls.Add(this.pictureBox_waveGraph);
            this.groupBox_signalCapture.Controls.Add(this.comboBox_recDevices);
            this.groupBox_signalCapture.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_signalCapture.Location = new System.Drawing.Point(0, 0);
            this.groupBox_signalCapture.Name = "groupBox_signalCapture";
            this.groupBox_signalCapture.Size = new System.Drawing.Size(824, 196);
            this.groupBox_signalCapture.TabIndex = 12;
            this.groupBox_signalCapture.TabStop = false;
            this.groupBox_signalCapture.Text = "Захват сигнала";
            this.groupBox_signalCapture.Enter += new System.EventHandler(this.groupBox_signalCapture_Enter);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(510, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Устройство воспр.:";
            // 
            // comboBox_playDevices
            // 
            this.comboBox_playDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_playDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_playDevices.FormattingEnabled = true;
            this.comboBox_playDevices.Location = new System.Drawing.Point(622, 141);
            this.comboBox_playDevices.Name = "comboBox_playDevices";
            this.comboBox_playDevices.Size = new System.Drawing.Size(194, 21);
            this.comboBox_playDevices.TabIndex = 16;
            this.comboBox_playDevices.SelectedIndexChanged += new System.EventHandler(this.comboBox_playDevices_SelectedIndexChanged);
            // 
            // groupBox_decoderSettings
            // 
            this.groupBox_decoderSettings.Controls.Add(this.checkBox_autoGain);
            this.groupBox_decoderSettings.Controls.Add(this.checkBox_remainingTime);
            this.groupBox_decoderSettings.Controls.Add(this.checkBox_invertSignal);
            this.groupBox_decoderSettings.Location = new System.Drawing.Point(328, 117);
            this.groupBox_decoderSettings.Name = "groupBox_decoderSettings";
            this.groupBox_decoderSettings.Size = new System.Drawing.Size(176, 73);
            this.groupBox_decoderSettings.TabIndex = 15;
            this.groupBox_decoderSettings.TabStop = false;
            this.groupBox_decoderSettings.Text = "Декодер";
            // 
            // checkBox_autoGain
            // 
            this.checkBox_autoGain.AutoSize = true;
            this.checkBox_autoGain.Checked = true;
            this.checkBox_autoGain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autoGain.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBox_autoGain.Location = new System.Drawing.Point(3, 50);
            this.checkBox_autoGain.Name = "checkBox_autoGain";
            this.checkBox_autoGain.Size = new System.Drawing.Size(170, 17);
            this.checkBox_autoGain.TabIndex = 2;
            this.checkBox_autoGain.Text = "Автоусиление";
            this.checkBox_autoGain.UseVisualStyleBackColor = true;
            this.checkBox_autoGain.CheckedChanged += new System.EventHandler(this.checkBox_autoGain_CheckedChanged);
            // 
            // checkBox_remainingTime
            // 
            this.checkBox_remainingTime.AutoSize = true;
            this.checkBox_remainingTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBox_remainingTime.Location = new System.Drawing.Point(3, 33);
            this.checkBox_remainingTime.Name = "checkBox_remainingTime";
            this.checkBox_remainingTime.Size = new System.Drawing.Size(170, 17);
            this.checkBox_remainingTime.TabIndex = 1;
            this.checkBox_remainingTime.Text = "Оставшееся время";
            this.checkBox_remainingTime.UseVisualStyleBackColor = true;
            this.checkBox_remainingTime.CheckedChanged += new System.EventHandler(this.checkBox_remainingTime_CheckedChanged);
            // 
            // checkBox_invertSignal
            // 
            this.checkBox_invertSignal.AutoSize = true;
            this.checkBox_invertSignal.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBox_invertSignal.Location = new System.Drawing.Point(3, 16);
            this.checkBox_invertSignal.Name = "checkBox_invertSignal";
            this.checkBox_invertSignal.Size = new System.Drawing.Size(170, 17);
            this.checkBox_invertSignal.TabIndex = 0;
            this.checkBox_invertSignal.Text = "Инвертировать сигнал";
            this.checkBox_invertSignal.UseVisualStyleBackColor = true;
            this.checkBox_invertSignal.CheckedChanged += new System.EventHandler(this.checkBox_invertSignal_CheckedChanged);
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
            this.label_recDevice.Location = new System.Drawing.Point(510, 120);
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
            this.pictureBox_waveGraph.Size = new System.Drawing.Size(808, 96);
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
            this.comboBox_recDevices.Location = new System.Drawing.Point(622, 117);
            this.comboBox_recDevices.Name = "comboBox_recDevices";
            this.comboBox_recDevices.Size = new System.Drawing.Size(194, 21);
            this.comboBox_recDevices.TabIndex = 10;
            this.comboBox_recDevices.SelectedIndexChanged += new System.EventHandler(this.comboBox_recDevices_SelectedIndexChanged);
            // 
            // button_buffMp3
            // 
            this.button_buffMp3.Location = new System.Drawing.Point(912, 160);
            this.button_buffMp3.Name = "button_buffMp3";
            this.button_buffMp3.Size = new System.Drawing.Size(96, 32);
            this.button_buffMp3.TabIndex = 14;
            this.button_buffMp3.Text = "PLAY";
            this.button_buffMp3.UseVisualStyleBackColor = true;
            this.button_buffMp3.Click += new System.EventHandler(this.button_buffMp3_Click);
            // 
            // timer_drawWaveGraphFrame
            // 
            this.timer_drawWaveGraphFrame.Interval = 40;
            this.timer_drawWaveGraphFrame.Tick += new System.EventHandler(this.timer_drawWaveGraphFrame_Tick);
            // 
            // timer_mpsPlayerHandler
            // 
            this.timer_mpsPlayerHandler.Enabled = true;
            this.timer_mpsPlayerHandler.Interval = 25;
            this.timer_mpsPlayerHandler.Tick += new System.EventHandler(this.timer_mpsPlayerHandler_Tick);
            // 
            // timer_mpsPlayerSpectrumHandler
            // 
            this.timer_mpsPlayerSpectrumHandler.Enabled = true;
            this.timer_mpsPlayerSpectrumHandler.Interval = 25;
            this.timer_mpsPlayerSpectrumHandler.Tick += new System.EventHandler(this.timer_mpsPlayerSpectrumHandler_Tick);
            // 
            // timer_mpsPlayerSpectrumUpdater
            // 
            this.timer_mpsPlayerSpectrumUpdater.Enabled = true;
            this.timer_mpsPlayerSpectrumUpdater.Interval = 25;
            this.timer_mpsPlayerSpectrumUpdater.Tick += new System.EventHandler(this.timer_mpsPlayerSpectrumUpdater_Tick);
            // 
            // timer_mpsPlayerTimeUpdater
            // 
            this.timer_mpsPlayerTimeUpdater.Enabled = true;
            this.timer_mpsPlayerTimeUpdater.Interval = 1000;
            this.timer_mpsPlayerTimeUpdater.Tick += new System.EventHandler(this.timer_mpsPlayerTimeUpdate_Tick);
            // 
            // pictureBox_runningIndicator
            // 
            this.pictureBox_runningIndicator.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_runningIndicator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_runningIndicator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_runningIndicator.Image = global::AudioDataInterface.Properties.Resources.Running_Indicator;
            this.pictureBox_runningIndicator.Location = new System.Drawing.Point(592, 136);
            this.pictureBox_runningIndicator.Name = "pictureBox_runningIndicator";
            this.pictureBox_runningIndicator.Size = new System.Drawing.Size(200, 200);
            this.pictureBox_runningIndicator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_runningIndicator.TabIndex = 1;
            this.pictureBox_runningIndicator.TabStop = false;
            // 
            // pictureBox_track1
            // 
            this.pictureBox_track1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track1.Image = global::AudioDataInterface.Properties.Resources._1_trackNumber;
            this.pictureBox_track1.Location = new System.Drawing.Point(610, 215);
            this.pictureBox_track1.Name = "pictureBox_track1";
            this.pictureBox_track1.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track1.TabIndex = 2;
            this.pictureBox_track1.TabStop = false;
            // 
            // pictureBox_track2
            // 
            this.pictureBox_track2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track2.Image = global::AudioDataInterface.Properties.Resources._2_trackNumber;
            this.pictureBox_track2.Location = new System.Drawing.Point(632, 196);
            this.pictureBox_track2.Name = "pictureBox_track2";
            this.pictureBox_track2.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track2.TabIndex = 3;
            this.pictureBox_track2.TabStop = false;
            // 
            // pictureBox_track3
            // 
            this.pictureBox_track3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track3.Image = global::AudioDataInterface.Properties.Resources._3_trackNumber;
            this.pictureBox_track3.Location = new System.Drawing.Point(657, 180);
            this.pictureBox_track3.Name = "pictureBox_track3";
            this.pictureBox_track3.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track3.TabIndex = 4;
            this.pictureBox_track3.TabStop = false;
            // 
            // pictureBox_track4
            // 
            this.pictureBox_track4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track4.Image = global::AudioDataInterface.Properties.Resources._4_trackNumber;
            this.pictureBox_track4.Location = new System.Drawing.Point(686, 169);
            this.pictureBox_track4.Name = "pictureBox_track4";
            this.pictureBox_track4.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track4.TabIndex = 5;
            this.pictureBox_track4.TabStop = false;
            // 
            // pictureBox_track5
            // 
            this.pictureBox_track5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track5.Image = global::AudioDataInterface.Properties.Resources._5_trackNumber;
            this.pictureBox_track5.Location = new System.Drawing.Point(714, 166);
            this.pictureBox_track5.Name = "pictureBox_track5";
            this.pictureBox_track5.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track5.TabIndex = 6;
            this.pictureBox_track5.TabStop = false;
            // 
            // pictureBox_track6
            // 
            this.pictureBox_track6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track6.Image = global::AudioDataInterface.Properties.Resources._6_trackNumber;
            this.pictureBox_track6.Location = new System.Drawing.Point(742, 173);
            this.pictureBox_track6.Name = "pictureBox_track6";
            this.pictureBox_track6.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track6.TabIndex = 7;
            this.pictureBox_track6.TabStop = false;
            // 
            // pictureBox_track7
            // 
            this.pictureBox_track7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track7.Image = global::AudioDataInterface.Properties.Resources._7_trackNumber;
            this.pictureBox_track7.Location = new System.Drawing.Point(761, 197);
            this.pictureBox_track7.Name = "pictureBox_track7";
            this.pictureBox_track7.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track7.TabIndex = 8;
            this.pictureBox_track7.TabStop = false;
            // 
            // pictureBox_track8
            // 
            this.pictureBox_track8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track8.Image = global::AudioDataInterface.Properties.Resources._8_trackNumber;
            this.pictureBox_track8.Location = new System.Drawing.Point(761, 225);
            this.pictureBox_track8.Name = "pictureBox_track8";
            this.pictureBox_track8.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track8.TabIndex = 9;
            this.pictureBox_track8.TabStop = false;
            // 
            // pictureBox_track9
            // 
            this.pictureBox_track9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track9.Image = global::AudioDataInterface.Properties.Resources._9_trackNumber;
            this.pictureBox_track9.Location = new System.Drawing.Point(751, 255);
            this.pictureBox_track9.Name = "pictureBox_track9";
            this.pictureBox_track9.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track9.TabIndex = 10;
            this.pictureBox_track9.TabStop = false;
            // 
            // pictureBox_track10
            // 
            this.pictureBox_track10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track10.Image = global::AudioDataInterface.Properties.Resources._10_trackNumber;
            this.pictureBox_track10.Location = new System.Drawing.Point(726, 281);
            this.pictureBox_track10.Name = "pictureBox_track10";
            this.pictureBox_track10.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track10.TabIndex = 11;
            this.pictureBox_track10.TabStop = false;
            // 
            // pictureBox_track11
            // 
            this.pictureBox_track11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track11.Image = global::AudioDataInterface.Properties.Resources._11_trackNumber;
            this.pictureBox_track11.Location = new System.Drawing.Point(697, 298);
            this.pictureBox_track11.Name = "pictureBox_track11";
            this.pictureBox_track11.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track11.TabIndex = 12;
            this.pictureBox_track11.TabStop = false;
            // 
            // pictureBox_track13
            // 
            this.pictureBox_track13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track13.Image = global::AudioDataInterface.Properties.Resources._13_trackNumber;
            this.pictureBox_track13.Location = new System.Drawing.Point(633, 305);
            this.pictureBox_track13.Name = "pictureBox_track13";
            this.pictureBox_track13.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track13.TabIndex = 13;
            this.pictureBox_track13.TabStop = false;
            // 
            // pictureBox_track14
            // 
            this.pictureBox_track14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track14.Image = global::AudioDataInterface.Properties.Resources._14_trackNumber;
            this.pictureBox_track14.Location = new System.Drawing.Point(608, 295);
            this.pictureBox_track14.Name = "pictureBox_track14";
            this.pictureBox_track14.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track14.TabIndex = 14;
            this.pictureBox_track14.TabStop = false;
            // 
            // pictureBox_track15
            // 
            this.pictureBox_track15.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track15.Image = global::AudioDataInterface.Properties.Resources._15_trackNumber;
            this.pictureBox_track15.Location = new System.Drawing.Point(595, 270);
            this.pictureBox_track15.Name = "pictureBox_track15";
            this.pictureBox_track15.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track15.TabIndex = 15;
            this.pictureBox_track15.TabStop = false;
            // 
            // pictureBox_track16
            // 
            this.pictureBox_track16.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track16.Image = global::AudioDataInterface.Properties.Resources._16_trackNumber;
            this.pictureBox_track16.Location = new System.Drawing.Point(597, 243);
            this.pictureBox_track16.Name = "pictureBox_track16";
            this.pictureBox_track16.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track16.TabIndex = 16;
            this.pictureBox_track16.TabStop = false;
            // 
            // pictureBox_track12
            // 
            this.pictureBox_track12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_track12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.pictureBox_track12.Image = global::AudioDataInterface.Properties.Resources._12_trackNumber;
            this.pictureBox_track12.Location = new System.Drawing.Point(664, 305);
            this.pictureBox_track12.Name = "pictureBox_track12";
            this.pictureBox_track12.Size = new System.Drawing.Size(28, 23);
            this.pictureBox_track12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_track12.TabIndex = 18;
            this.pictureBox_track12.TabStop = false;
            // 
            // form_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1024, 638);
            this.Controls.Add(this.panel_base);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(1040, 677);
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
            this.tabPage_graphicalView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_cassette)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disc3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disc2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_disc1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_playPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_dots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_symbol1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_mpsPlayer)).EndInit();
            this.panel_base.ResumeLayout(false);
            this.panel_dataControl.ResumeLayout(false);
            this.groupBox_info.ResumeLayout(false);
            this.groupBox_info.PerformLayout();
            this.panel_signalCapture.ResumeLayout(false);
            this.panel_signalCapture.PerformLayout();
            this.groupBox_signalCapture.ResumeLayout(false);
            this.groupBox_signalCapture.PerformLayout();
            this.groupBox_decoderSettings.ResumeLayout(false);
            this.groupBox_decoderSettings.PerformLayout();
            this.groupBox_BIASAdjust.ResumeLayout(false);
            this.groupBox_BIASAdjust.PerformLayout();
            this.groupBox_scaleAdjust.ResumeLayout(false);
            this.groupBox_scaleAdjust.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_waveGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_runningIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_track12)).EndInit();
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
        public System.Windows.Forms.Button button_buffMp3;
        private System.Windows.Forms.PictureBox pictureBox_mpsPlayer;
        private System.Windows.Forms.Timer timer_mpsPlayerHandler;
        private System.Windows.Forms.Timer timer_mpsPlayerSpectrumUpdater;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox_symbol1;
        private System.Windows.Forms.PictureBox pictureBox_symbol2;
        private System.Windows.Forms.PictureBox pictureBox_symbol3;
        private System.Windows.Forms.PictureBox pictureBox_symbol8;
        private System.Windows.Forms.PictureBox pictureBox_symbol7;
        private System.Windows.Forms.PictureBox pictureBox_symbol6;
        private System.Windows.Forms.PictureBox pictureBox_symbol5;
        private System.Windows.Forms.PictureBox pictureBox_symbol4;
        private System.Windows.Forms.PictureBox pictureBox_dots;
        private System.Windows.Forms.PictureBox pictureBox_symbol10;
        private System.Windows.Forms.PictureBox pictureBox_symbol9;
        private System.Windows.Forms.PictureBox pictureBox_playPause;
        private System.Windows.Forms.Timer timer_mpsPlayerTimeUpdater;
        private System.Windows.Forms.GroupBox groupBox_decoderSettings;
        private System.Windows.Forms.CheckBox checkBox_remainingTime;
        public System.Windows.Forms.CheckBox checkBox_invertSignal;
        private System.Windows.Forms.GroupBox groupBox_info;
        private System.Windows.Forms.Label label_signalGainR;
        private System.Windows.Forms.Label label_signalGainL;
        private System.Windows.Forms.Label label_fixedErrorCount;
        private System.Windows.Forms.PictureBox pictureBox_disc1;
        private System.Windows.Forms.PictureBox pictureBox_disc3;
        private System.Windows.Forms.PictureBox pictureBox_disc2;
        private System.Windows.Forms.CheckBox checkBox_autoGain;
        private System.Windows.Forms.CheckBox checkBox_tapeSkin;
        private System.Windows.Forms.PictureBox pictureBox_cassette;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_playDevices;
        private System.Windows.Forms.Label label_frameSyncErrorCount;
        private System.Windows.Forms.Label label_unfixedErrorCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_border2;
        private System.Windows.Forms.Label label_border1;
        private System.Windows.Forms.ToolStripButton button_settings;
        public System.Windows.Forms.Timer timer_mpsPlayerSpectrumHandler;
        private System.Windows.Forms.PictureBox pictureBox_track12;
        private System.Windows.Forms.PictureBox pictureBox_track16;
        private System.Windows.Forms.PictureBox pictureBox_track15;
        private System.Windows.Forms.PictureBox pictureBox_track14;
        private System.Windows.Forms.PictureBox pictureBox_track13;
        private System.Windows.Forms.PictureBox pictureBox_track11;
        private System.Windows.Forms.PictureBox pictureBox_track10;
        private System.Windows.Forms.PictureBox pictureBox_track9;
        private System.Windows.Forms.PictureBox pictureBox_track8;
        private System.Windows.Forms.PictureBox pictureBox_track7;
        private System.Windows.Forms.PictureBox pictureBox_track6;
        private System.Windows.Forms.PictureBox pictureBox_track5;
        private System.Windows.Forms.PictureBox pictureBox_track4;
        private System.Windows.Forms.PictureBox pictureBox_track3;
        private System.Windows.Forms.PictureBox pictureBox_track2;
        private System.Windows.Forms.PictureBox pictureBox_track1;
        public System.Windows.Forms.PictureBox pictureBox_runningIndicator;
    }
}

