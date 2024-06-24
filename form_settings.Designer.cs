namespace AudioDataInterface
{
    partial class form_settings
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label_mp3BuffSizeValue = new System.Windows.Forms.Label();
            this.trackBar_mp3BuffSize = new System.Windows.Forms.TrackBar();
            this.label_mp3BuffSize = new System.Windows.Forms.Label();
            this.label_signalHeightValue = new System.Windows.Forms.Label();
            this.trackBar_signalHeight = new System.Windows.Forms.TrackBar();
            this.label_signalHeight = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox_Ffmpeg2EffectCmd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Ffmpeg2Cmd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Ffmpeg1Cmd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_mpsPlayerSubcodeIntervalValue = new System.Windows.Forms.Label();
            this.trackBar_mpsPlayerSubcodeInterval = new System.Windows.Forms.TrackBar();
            this.label_mpsPlayerSubcodeInterval = new System.Windows.Forms.Label();
            this.label_leadInOutSubcodesAmountValue = new System.Windows.Forms.Label();
            this.trackBar_leadInOutSubcodesAmount = new System.Windows.Forms.TrackBar();
            this.label_leadInOutSubcodesAmount = new System.Windows.Forms.Label();
            this.label_silenceSecondsValue = new System.Windows.Forms.Label();
            this.trackBar_silenceSeconds = new System.Windows.Forms.TrackBar();
            this.label_silenceSeconds = new System.Windows.Forms.Label();
            this.label_signalGainValue = new System.Windows.Forms.Label();
            this.trackBar_signalGain = new System.Windows.Forms.TrackBar();
            this.label_signalGain = new System.Windows.Forms.Label();
            this.label_encodingDensityValue = new System.Windows.Forms.Label();
            this.trackBar_encodingSampleRate = new System.Windows.Forms.TrackBar();
            this.label_encodingDensity = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.comboBox_skins = new System.Windows.Forms.ComboBox();
            this.label_mpsPlayerSkin = new System.Windows.Forms.Label();
            this.radioButton_offMode = new System.Windows.Forms.RadioButton();
            this.radioButton_noPeakMode = new System.Windows.Forms.RadioButton();
            this.radioButton_peakHoldMode = new System.Windows.Forms.RadioButton();
            this.label_spectrumMode = new System.Windows.Forms.Label();
            this.label_fftSizeValue = new System.Windows.Forms.Label();
            this.trackBar_fftSize = new System.Windows.Forms.TrackBar();
            this.label_fftSize = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mp3BuffSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_signalHeight)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mpsPlayerSubcodeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_leadInOutSubcodesAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_silenceSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_signalGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_encodingSampleRate)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_fftSize)).BeginInit();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 360);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(590, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Controls.Add(this.tabPage5);
            this.tabControl.Controls.Add(this.tabPage6);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(590, 315);
            this.tabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label_mp3BuffSizeValue);
            this.tabPage1.Controls.Add(this.trackBar_mp3BuffSize);
            this.tabPage1.Controls.Add(this.label_mp3BuffSize);
            this.tabPage1.Controls.Add(this.label_signalHeightValue);
            this.tabPage1.Controls.Add(this.trackBar_signalHeight);
            this.tabPage1.Controls.Add(this.label_signalHeight);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(582, 289);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Аудио I/O";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label_mp3BuffSizeValue
            // 
            this.label_mp3BuffSizeValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_mp3BuffSizeValue.Location = new System.Drawing.Point(3, 132);
            this.label_mp3BuffSizeValue.Name = "label_mp3BuffSizeValue";
            this.label_mp3BuffSizeValue.Size = new System.Drawing.Size(576, 13);
            this.label_mp3BuffSizeValue.TabIndex = 5;
            this.label_mp3BuffSizeValue.Text = "0";
            this.label_mp3BuffSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_mp3BuffSize
            // 
            this.trackBar_mp3BuffSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_mp3BuffSize.LargeChange = 128;
            this.trackBar_mp3BuffSize.Location = new System.Drawing.Point(3, 87);
            this.trackBar_mp3BuffSize.Maximum = 4096;
            this.trackBar_mp3BuffSize.Minimum = 128;
            this.trackBar_mp3BuffSize.Name = "trackBar_mp3BuffSize";
            this.trackBar_mp3BuffSize.Size = new System.Drawing.Size(576, 45);
            this.trackBar_mp3BuffSize.TabIndex = 4;
            this.trackBar_mp3BuffSize.TickFrequency = 128;
            this.trackBar_mp3BuffSize.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_mp3BuffSize.Value = 128;
            this.trackBar_mp3BuffSize.Scroll += new System.EventHandler(this.trackBar_mp3BuffSize_Scroll);
            // 
            // label_mp3BuffSize
            // 
            this.label_mp3BuffSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_mp3BuffSize.Location = new System.Drawing.Point(3, 74);
            this.label_mp3BuffSize.Name = "label_mp3BuffSize";
            this.label_mp3BuffSize.Size = new System.Drawing.Size(576, 13);
            this.label_mp3BuffSize.TabIndex = 3;
            this.label_mp3BuffSize.Text = "Размер буфера MP3, (byte)";
            this.label_mp3BuffSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_signalHeightValue
            // 
            this.label_signalHeightValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_signalHeightValue.Location = new System.Drawing.Point(3, 61);
            this.label_signalHeightValue.Name = "label_signalHeightValue";
            this.label_signalHeightValue.Size = new System.Drawing.Size(576, 13);
            this.label_signalHeightValue.TabIndex = 2;
            this.label_signalHeightValue.Text = "0";
            this.label_signalHeightValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_signalHeight
            // 
            this.trackBar_signalHeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_signalHeight.LargeChange = 512;
            this.trackBar_signalHeight.Location = new System.Drawing.Point(3, 16);
            this.trackBar_signalHeight.Maximum = 8192;
            this.trackBar_signalHeight.Name = "trackBar_signalHeight";
            this.trackBar_signalHeight.Size = new System.Drawing.Size(576, 45);
            this.trackBar_signalHeight.TabIndex = 1;
            this.trackBar_signalHeight.TickFrequency = 512;
            this.trackBar_signalHeight.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_signalHeight.Scroll += new System.EventHandler(this.trackBar_signalHeight_Scroll);
            // 
            // label_signalHeight
            // 
            this.label_signalHeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_signalHeight.Location = new System.Drawing.Point(3, 3);
            this.label_signalHeight.Name = "label_signalHeight";
            this.label_signalHeight.Size = new System.Drawing.Size(576, 13);
            this.label_signalHeight.TabIndex = 0;
            this.label_signalHeight.Text = "Смещение сигнала";
            this.label_signalHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(582, 289);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Декодер";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.textBox_Ffmpeg2EffectCmd);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.textBox_Ffmpeg2Cmd);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.textBox_Ffmpeg1Cmd);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.label_mpsPlayerSubcodeIntervalValue);
            this.tabPage3.Controls.Add(this.trackBar_mpsPlayerSubcodeInterval);
            this.tabPage3.Controls.Add(this.label_mpsPlayerSubcodeInterval);
            this.tabPage3.Controls.Add(this.label_leadInOutSubcodesAmountValue);
            this.tabPage3.Controls.Add(this.trackBar_leadInOutSubcodesAmount);
            this.tabPage3.Controls.Add(this.label_leadInOutSubcodesAmount);
            this.tabPage3.Controls.Add(this.label_silenceSecondsValue);
            this.tabPage3.Controls.Add(this.trackBar_silenceSeconds);
            this.tabPage3.Controls.Add(this.label_silenceSeconds);
            this.tabPage3.Controls.Add(this.label_signalGainValue);
            this.tabPage3.Controls.Add(this.trackBar_signalGain);
            this.tabPage3.Controls.Add(this.label_signalGain);
            this.tabPage3.Controls.Add(this.label_encodingDensityValue);
            this.tabPage3.Controls.Add(this.trackBar_encodingSampleRate);
            this.tabPage3.Controls.Add(this.label_encodingDensity);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(582, 289);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Энкодер";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox_Ffmpeg2EffectCmd
            // 
            this.textBox_Ffmpeg2EffectCmd.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_Ffmpeg2EffectCmd.Location = new System.Drawing.Point(3, 501);
            this.textBox_Ffmpeg2EffectCmd.Multiline = true;
            this.textBox_Ffmpeg2EffectCmd.Name = "textBox_Ffmpeg2EffectCmd";
            this.textBox_Ffmpeg2EffectCmd.Size = new System.Drawing.Size(559, 52);
            this.textBox_Ffmpeg2EffectCmd.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(3, 488);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(559, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Команда FFMPEG аудио фильтра 2 проход";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Ffmpeg2Cmd
            // 
            this.textBox_Ffmpeg2Cmd.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_Ffmpeg2Cmd.Location = new System.Drawing.Point(3, 436);
            this.textBox_Ffmpeg2Cmd.Multiline = true;
            this.textBox_Ffmpeg2Cmd.Name = "textBox_Ffmpeg2Cmd";
            this.textBox_Ffmpeg2Cmd.Size = new System.Drawing.Size(559, 52);
            this.textBox_Ffmpeg2Cmd.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 423);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(559, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Команда FFMPEG кодера 2 проход";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Ffmpeg1Cmd
            // 
            this.textBox_Ffmpeg1Cmd.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_Ffmpeg1Cmd.Location = new System.Drawing.Point(3, 371);
            this.textBox_Ffmpeg1Cmd.Multiline = true;
            this.textBox_Ffmpeg1Cmd.Name = "textBox_Ffmpeg1Cmd";
            this.textBox_Ffmpeg1Cmd.Size = new System.Drawing.Size(559, 52);
            this.textBox_Ffmpeg1Cmd.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 358);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(559, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Команда FFMPEG кодера 1 проход";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_mpsPlayerSubcodeIntervalValue
            // 
            this.label_mpsPlayerSubcodeIntervalValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_mpsPlayerSubcodeIntervalValue.Location = new System.Drawing.Point(3, 345);
            this.label_mpsPlayerSubcodeIntervalValue.Name = "label_mpsPlayerSubcodeIntervalValue";
            this.label_mpsPlayerSubcodeIntervalValue.Size = new System.Drawing.Size(559, 13);
            this.label_mpsPlayerSubcodeIntervalValue.TabIndex = 17;
            this.label_mpsPlayerSubcodeIntervalValue.Text = "0";
            this.label_mpsPlayerSubcodeIntervalValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_mpsPlayerSubcodeInterval
            // 
            this.trackBar_mpsPlayerSubcodeInterval.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_mpsPlayerSubcodeInterval.LargeChange = 1;
            this.trackBar_mpsPlayerSubcodeInterval.Location = new System.Drawing.Point(3, 300);
            this.trackBar_mpsPlayerSubcodeInterval.Minimum = 1;
            this.trackBar_mpsPlayerSubcodeInterval.Name = "trackBar_mpsPlayerSubcodeInterval";
            this.trackBar_mpsPlayerSubcodeInterval.Size = new System.Drawing.Size(559, 45);
            this.trackBar_mpsPlayerSubcodeInterval.TabIndex = 16;
            this.trackBar_mpsPlayerSubcodeInterval.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_mpsPlayerSubcodeInterval.Value = 1;
            this.trackBar_mpsPlayerSubcodeInterval.Scroll += new System.EventHandler(this.trackBar_mpsPlayerSubcodeInterval_Scroll);
            // 
            // label_mpsPlayerSubcodeInterval
            // 
            this.label_mpsPlayerSubcodeInterval.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_mpsPlayerSubcodeInterval.Location = new System.Drawing.Point(3, 287);
            this.label_mpsPlayerSubcodeInterval.Name = "label_mpsPlayerSubcodeInterval";
            this.label_mpsPlayerSubcodeInterval.Size = new System.Drawing.Size(559, 13);
            this.label_mpsPlayerSubcodeInterval.TabIndex = 15;
            this.label_mpsPlayerSubcodeInterval.Text = "Интервал между субкодами MPS плеера, (сек)";
            this.label_mpsPlayerSubcodeInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_leadInOutSubcodesAmountValue
            // 
            this.label_leadInOutSubcodesAmountValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_leadInOutSubcodesAmountValue.Location = new System.Drawing.Point(3, 274);
            this.label_leadInOutSubcodesAmountValue.Name = "label_leadInOutSubcodesAmountValue";
            this.label_leadInOutSubcodesAmountValue.Size = new System.Drawing.Size(559, 13);
            this.label_leadInOutSubcodesAmountValue.TabIndex = 14;
            this.label_leadInOutSubcodesAmountValue.Text = "0";
            this.label_leadInOutSubcodesAmountValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_leadInOutSubcodesAmount
            // 
            this.trackBar_leadInOutSubcodesAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_leadInOutSubcodesAmount.LargeChange = 128;
            this.trackBar_leadInOutSubcodesAmount.Location = new System.Drawing.Point(3, 229);
            this.trackBar_leadInOutSubcodesAmount.Maximum = 4096;
            this.trackBar_leadInOutSubcodesAmount.Name = "trackBar_leadInOutSubcodesAmount";
            this.trackBar_leadInOutSubcodesAmount.Size = new System.Drawing.Size(559, 45);
            this.trackBar_leadInOutSubcodesAmount.TabIndex = 13;
            this.trackBar_leadInOutSubcodesAmount.TickFrequency = 128;
            this.trackBar_leadInOutSubcodesAmount.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_leadInOutSubcodesAmount.Scroll += new System.EventHandler(this.trackBar_leadInOutSubcodesAmount_Scroll);
            // 
            // label_leadInOutSubcodesAmount
            // 
            this.label_leadInOutSubcodesAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_leadInOutSubcodesAmount.Location = new System.Drawing.Point(3, 216);
            this.label_leadInOutSubcodesAmount.Name = "label_leadInOutSubcodesAmount";
            this.label_leadInOutSubcodesAmount.Size = new System.Drawing.Size(559, 13);
            this.label_leadInOutSubcodesAmount.TabIndex = 12;
            this.label_leadInOutSubcodesAmount.Text = "Количество входящих и выводящих субкодов";
            this.label_leadInOutSubcodesAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_silenceSecondsValue
            // 
            this.label_silenceSecondsValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_silenceSecondsValue.Location = new System.Drawing.Point(3, 203);
            this.label_silenceSecondsValue.Name = "label_silenceSecondsValue";
            this.label_silenceSecondsValue.Size = new System.Drawing.Size(559, 13);
            this.label_silenceSecondsValue.TabIndex = 11;
            this.label_silenceSecondsValue.Text = "0";
            this.label_silenceSecondsValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_silenceSeconds
            // 
            this.trackBar_silenceSeconds.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_silenceSeconds.LargeChange = 1;
            this.trackBar_silenceSeconds.Location = new System.Drawing.Point(3, 158);
            this.trackBar_silenceSeconds.Maximum = 30;
            this.trackBar_silenceSeconds.Name = "trackBar_silenceSeconds";
            this.trackBar_silenceSeconds.Size = new System.Drawing.Size(559, 45);
            this.trackBar_silenceSeconds.TabIndex = 10;
            this.trackBar_silenceSeconds.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_silenceSeconds.Scroll += new System.EventHandler(this.trackBar_silenceSeconds_Scroll);
            // 
            // label_silenceSeconds
            // 
            this.label_silenceSeconds.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_silenceSeconds.Location = new System.Drawing.Point(3, 145);
            this.label_silenceSeconds.Name = "label_silenceSeconds";
            this.label_silenceSeconds.Size = new System.Drawing.Size(559, 13);
            this.label_silenceSeconds.TabIndex = 9;
            this.label_silenceSeconds.Text = "Тишина в начале и конце сигнала, (sec)";
            this.label_silenceSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_signalGainValue
            // 
            this.label_signalGainValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_signalGainValue.Location = new System.Drawing.Point(3, 132);
            this.label_signalGainValue.Name = "label_signalGainValue";
            this.label_signalGainValue.Size = new System.Drawing.Size(559, 13);
            this.label_signalGainValue.TabIndex = 8;
            this.label_signalGainValue.Text = "0";
            this.label_signalGainValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_signalGain
            // 
            this.trackBar_signalGain.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_signalGain.LargeChange = 1;
            this.trackBar_signalGain.Location = new System.Drawing.Point(3, 87);
            this.trackBar_signalGain.Maximum = 4;
            this.trackBar_signalGain.Minimum = 1;
            this.trackBar_signalGain.Name = "trackBar_signalGain";
            this.trackBar_signalGain.Size = new System.Drawing.Size(559, 45);
            this.trackBar_signalGain.TabIndex = 7;
            this.trackBar_signalGain.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_signalGain.Value = 1;
            this.trackBar_signalGain.Scroll += new System.EventHandler(this.trackBar_signalGain_Scroll);
            // 
            // label_signalGain
            // 
            this.label_signalGain.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_signalGain.Location = new System.Drawing.Point(3, 74);
            this.label_signalGain.Name = "label_signalGain";
            this.label_signalGain.Size = new System.Drawing.Size(559, 13);
            this.label_signalGain.TabIndex = 6;
            this.label_signalGain.Text = "Усиление кодируемого сигнала";
            this.label_signalGain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_encodingDensityValue
            // 
            this.label_encodingDensityValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_encodingDensityValue.Location = new System.Drawing.Point(3, 61);
            this.label_encodingDensityValue.Name = "label_encodingDensityValue";
            this.label_encodingDensityValue.Size = new System.Drawing.Size(559, 13);
            this.label_encodingDensityValue.TabIndex = 5;
            this.label_encodingDensityValue.Text = "0";
            this.label_encodingDensityValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_encodingSampleRate
            // 
            this.trackBar_encodingSampleRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_encodingSampleRate.LargeChange = 2000;
            this.trackBar_encodingSampleRate.Location = new System.Drawing.Point(3, 16);
            this.trackBar_encodingSampleRate.Maximum = 328000;
            this.trackBar_encodingSampleRate.Minimum = 40000;
            this.trackBar_encodingSampleRate.Name = "trackBar_encodingSampleRate";
            this.trackBar_encodingSampleRate.Size = new System.Drawing.Size(559, 45);
            this.trackBar_encodingSampleRate.TabIndex = 4;
            this.trackBar_encodingSampleRate.TickFrequency = 10000;
            this.trackBar_encodingSampleRate.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_encodingSampleRate.Value = 40000;
            this.trackBar_encodingSampleRate.Scroll += new System.EventHandler(this.trackBar_encodingSampleRate_Scroll);
            // 
            // label_encodingDensity
            // 
            this.label_encodingDensity.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_encodingDensity.Location = new System.Drawing.Point(3, 3);
            this.label_encodingDensity.Name = "label_encodingDensity";
            this.label_encodingDensity.Size = new System.Drawing.Size(559, 13);
            this.label_encodingDensity.TabIndex = 3;
            this.label_encodingDensity.Text = "Плотность кодирования данных, (kbps)";
            this.label_encodingDensity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.comboBox_skins);
            this.tabPage4.Controls.Add(this.label_mpsPlayerSkin);
            this.tabPage4.Controls.Add(this.radioButton_offMode);
            this.tabPage4.Controls.Add(this.radioButton_noPeakMode);
            this.tabPage4.Controls.Add(this.radioButton_peakHoldMode);
            this.tabPage4.Controls.Add(this.label_spectrumMode);
            this.tabPage4.Controls.Add(this.label_fftSizeValue);
            this.tabPage4.Controls.Add(this.trackBar_fftSize);
            this.tabPage4.Controls.Add(this.label_fftSize);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(582, 289);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Интерфейс";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // comboBox_skins
            // 
            this.comboBox_skins.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox_skins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_skins.FormattingEnabled = true;
            this.comboBox_skins.Location = new System.Drawing.Point(3, 151);
            this.comboBox_skins.Name = "comboBox_skins";
            this.comboBox_skins.Size = new System.Drawing.Size(576, 21);
            this.comboBox_skins.TabIndex = 20;
            this.comboBox_skins.SelectedIndexChanged += new System.EventHandler(this.comboBox_skins_SelectedIndexChanged);
            // 
            // label_mpsPlayerSkin
            // 
            this.label_mpsPlayerSkin.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_mpsPlayerSkin.Location = new System.Drawing.Point(3, 138);
            this.label_mpsPlayerSkin.Name = "label_mpsPlayerSkin";
            this.label_mpsPlayerSkin.Size = new System.Drawing.Size(576, 13);
            this.label_mpsPlayerSkin.TabIndex = 19;
            this.label_mpsPlayerSkin.Text = "Скин MPS плеера";
            this.label_mpsPlayerSkin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButton_offMode
            // 
            this.radioButton_offMode.AutoSize = true;
            this.radioButton_offMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButton_offMode.Location = new System.Drawing.Point(3, 121);
            this.radioButton_offMode.Name = "radioButton_offMode";
            this.radioButton_offMode.Size = new System.Drawing.Size(576, 17);
            this.radioButton_offMode.TabIndex = 15;
            this.radioButton_offMode.TabStop = true;
            this.radioButton_offMode.Text = "Выключен";
            this.radioButton_offMode.UseVisualStyleBackColor = true;
            this.radioButton_offMode.CheckedChanged += new System.EventHandler(this.radioButton_offMode_CheckedChanged);
            // 
            // radioButton_noPeakMode
            // 
            this.radioButton_noPeakMode.AutoSize = true;
            this.radioButton_noPeakMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButton_noPeakMode.Location = new System.Drawing.Point(3, 104);
            this.radioButton_noPeakMode.Name = "radioButton_noPeakMode";
            this.radioButton_noPeakMode.Size = new System.Drawing.Size(576, 17);
            this.radioButton_noPeakMode.TabIndex = 14;
            this.radioButton_noPeakMode.TabStop = true;
            this.radioButton_noPeakMode.Text = "Без пиков";
            this.radioButton_noPeakMode.UseVisualStyleBackColor = true;
            this.radioButton_noPeakMode.CheckedChanged += new System.EventHandler(this.radioButton_noPeakMode_CheckedChanged);
            // 
            // radioButton_peakHoldMode
            // 
            this.radioButton_peakHoldMode.AutoSize = true;
            this.radioButton_peakHoldMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButton_peakHoldMode.Location = new System.Drawing.Point(3, 87);
            this.radioButton_peakHoldMode.Name = "radioButton_peakHoldMode";
            this.radioButton_peakHoldMode.Size = new System.Drawing.Size(576, 17);
            this.radioButton_peakHoldMode.TabIndex = 13;
            this.radioButton_peakHoldMode.TabStop = true;
            this.radioButton_peakHoldMode.Text = "С отображением пиков";
            this.radioButton_peakHoldMode.UseVisualStyleBackColor = true;
            this.radioButton_peakHoldMode.CheckedChanged += new System.EventHandler(this.radioButton_peakHoldMode_CheckedChanged);
            // 
            // label_spectrumMode
            // 
            this.label_spectrumMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_spectrumMode.Location = new System.Drawing.Point(3, 74);
            this.label_spectrumMode.Name = "label_spectrumMode";
            this.label_spectrumMode.Size = new System.Drawing.Size(576, 13);
            this.label_spectrumMode.TabIndex = 12;
            this.label_spectrumMode.Text = "Режим спектроанализатора";
            this.label_spectrumMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_fftSizeValue
            // 
            this.label_fftSizeValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_fftSizeValue.Location = new System.Drawing.Point(3, 61);
            this.label_fftSizeValue.Name = "label_fftSizeValue";
            this.label_fftSizeValue.Size = new System.Drawing.Size(576, 13);
            this.label_fftSizeValue.TabIndex = 11;
            this.label_fftSizeValue.Text = "0";
            this.label_fftSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_fftSizeValue.Click += new System.EventHandler(this.label1_Click);
            // 
            // trackBar_fftSize
            // 
            this.trackBar_fftSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_fftSize.LargeChange = 256;
            this.trackBar_fftSize.Location = new System.Drawing.Point(3, 16);
            this.trackBar_fftSize.Maximum = 4096;
            this.trackBar_fftSize.Minimum = 256;
            this.trackBar_fftSize.Name = "trackBar_fftSize";
            this.trackBar_fftSize.Size = new System.Drawing.Size(576, 45);
            this.trackBar_fftSize.TabIndex = 10;
            this.trackBar_fftSize.TickFrequency = 256;
            this.trackBar_fftSize.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_fftSize.Value = 256;
            this.trackBar_fftSize.Scroll += new System.EventHandler(this.trackBar_fftSize_Scroll);
            // 
            // label_fftSize
            // 
            this.label_fftSize.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_fftSize.Location = new System.Drawing.Point(3, 3);
            this.label_fftSize.Name = "label_fftSize";
            this.label_fftSize.Size = new System.Drawing.Size(576, 13);
            this.label_fftSize.TabIndex = 9;
            this.label_fftSize.Text = "Размер FFT буфера спектроанализатора, (samples)";
            this.label_fftSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_fftSize.Click += new System.EventHandler(this.label2_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(582, 289);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Запись данных на ленту";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(582, 289);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Восстановление данных с ленты";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.button2);
            this.groupBox.Controls.Add(this.button_save);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 315);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(590, 45);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Dock = System.Windows.Forms.DockStyle.Left;
            this.button2.Location = new System.Drawing.Point(3, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 26);
            this.button2.TabIndex = 1;
            this.button2.Text = "По-умолчанию";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_save
            // 
            this.button_save.AutoSize = true;
            this.button_save.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_save.Location = new System.Drawing.Point(512, 16);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 26);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Сохранить";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button1_Click);
            // 
            // form_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 382);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "form_settings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.form_settings_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mp3BuffSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_signalHeight)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mpsPlayerSubcodeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_leadInOutSubcodesAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_silenceSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_signalGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_encodingSampleRate)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_fftSize)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Label label_signalHeightValue;
        private System.Windows.Forms.TrackBar trackBar_signalHeight;
        private System.Windows.Forms.Label label_signalHeight;
        private System.Windows.Forms.Label label_mp3BuffSizeValue;
        private System.Windows.Forms.TrackBar trackBar_mp3BuffSize;
        private System.Windows.Forms.Label label_mp3BuffSize;
        private System.Windows.Forms.Label label_encodingDensityValue;
        private System.Windows.Forms.TrackBar trackBar_encodingSampleRate;
        private System.Windows.Forms.Label label_encodingDensity;
        private System.Windows.Forms.Label label_signalGainValue;
        private System.Windows.Forms.TrackBar trackBar_signalGain;
        private System.Windows.Forms.Label label_signalGain;
        private System.Windows.Forms.Label label_silenceSecondsValue;
        private System.Windows.Forms.TrackBar trackBar_silenceSeconds;
        private System.Windows.Forms.Label label_silenceSeconds;
        private System.Windows.Forms.Label label_mpsPlayerSubcodeIntervalValue;
        private System.Windows.Forms.TrackBar trackBar_mpsPlayerSubcodeInterval;
        private System.Windows.Forms.Label label_mpsPlayerSubcodeInterval;
        private System.Windows.Forms.Label label_leadInOutSubcodesAmountValue;
        private System.Windows.Forms.TrackBar trackBar_leadInOutSubcodesAmount;
        private System.Windows.Forms.Label label_leadInOutSubcodesAmount;
        private System.Windows.Forms.RadioButton radioButton_peakHoldMode;
        private System.Windows.Forms.Label label_spectrumMode;
        private System.Windows.Forms.Label label_fftSizeValue;
        private System.Windows.Forms.TrackBar trackBar_fftSize;
        private System.Windows.Forms.Label label_fftSize;
        private System.Windows.Forms.RadioButton radioButton_offMode;
        private System.Windows.Forms.RadioButton radioButton_noPeakMode;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_Ffmpeg1Cmd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Ffmpeg2Cmd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Ffmpeg2EffectCmd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.ComboBox comboBox_skins;
        private System.Windows.Forms.Label label_mpsPlayerSkin;
    }
}