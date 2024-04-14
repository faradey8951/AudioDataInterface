using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioDataInterface
{
    public partial class form_tapeRecordingWizard : Form
    {
        static string filePath = "";
        static string projectPath = "Tape Record Wizard";
        static int fileSize = 0;
        public static int fileSectorsCount = 128; //Количество секторов для файла
        static FileInfo fileInfo;
        public static Thread thread_prepareSectors = new Thread(PrepareSectors);
        public static Thread thread_encodeSectors = new Thread(EncodeSectors);
        public static Thread thread_tapeRecord = new Thread(TapeRecord);
        public static bool progressShow = false;
        public static string status = "";
        public static int progress = 0;
        public static List<string[]> sectorTable;
        public static bool sectorTableReady = false;
        public static bool fileConverted = false;
        public form_tapeRecordingWizard()
        {
            InitializeComponent();
        }

        private void form_tapeRecordingWizard_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(AudioIO.GetPlayDevices());
            comboBox1.Text = AudioIO.GetPlayDevices()[AudioIO.audio_playDeviceId];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button_fileSelect_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            filePath = openFileDialog.FileName;
            textBox_filePath.Text = filePath;
            if (File.Exists(filePath))
            {
                fileInfo = new FileInfo(filePath);
                fileSize = (int)fileInfo.Length / 1024;
                label_fileSize.Text = fileSize.ToString() + " КБ";
            }
        }

        /// <summary>
        /// Разбивает файл на секторы
        /// </summary>
        public static void PrepareSectors()
        {
            progressShow = true;
            sectorTable = new List<string[]>();
            //Разбиение файла на секторы            
            int fileSectorSize = (int)fileInfo.Length / fileSectorsCount; //Размер сектора в байтах
            FileStream fs_input = new FileStream(filePath, FileMode.Open);
            FileStream fs_output = null;
            if (Directory.Exists(projectPath)) Directory.Delete(projectPath, true);
            Directory.CreateDirectory(projectPath);
            for (int i = 0; i < fileSectorsCount; i++)
            {
                fs_output = new FileStream(projectPath + "\\" + (i + 1).ToString() + ".sector", FileMode.Create);
                for (int k = 0; k < fileSectorSize && fs_input.Position < fs_input.Length; k++) fs_output.WriteByte((byte)fs_input.ReadByte());
                fs_output.Close();
                fs_output.Dispose();
                status = "Разбиение на секторы...";
                progress = ProgressHandler.GetPercent(fileSectorsCount, i + 1);
                sectorTable.Add(new string[] { (i + 1).ToString(), new FileInfo(projectPath + "\\" + (i + 1).ToString() + ".sector").Length.ToString(), "Подготовка", "100%", projectPath + "\\" + (i + 1).ToString() + ".sector" });
            }            
            fs_input.Close();
            fs_input.Dispose();
            fs_output.Close();
            fs_output.Dispose();

            //Формирование заголовка
            status = "Формирование заголовка...";
            progress = 0;
            string header = Path.GetFileName(filePath) + ";" + fileSize.ToString() + ";" + fileSectorsCount + ";"; //Заголовок потока
            string headerHash = BinaryHandler.GetHash(header);
            header += headerHash;
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            while ((headerBytes.Length / 4.0) - Math.Truncate(headerBytes.Length / 4.0) != 0)
            {
                Array.Resize(ref headerBytes, headerBytes.Length + 1);
                headerBytes[headerBytes.Length - 1] = 0;
            }
            MessageBox.Show(Encoding.UTF8.GetString(headerBytes));
            fs_output = new FileStream(projectPath + "\\" + "header.sector", FileMode.Create);
            foreach (byte b in headerBytes) fs_output.WriteByte((byte)b);
            fs_output.Close();
            fs_output.Dispose();
            progress = 100;
            sectorTable.Add(new string[] { "Заголовок", new FileInfo(projectPath + "\\" + "header.sector").Length.ToString(), "Подготовка", "100%", projectPath + "\\" + "header.sector" });
            Thread.Sleep(2000);

            //Расчет контрольных сумм секторов
            status = "Расчет контрольных сумм секторов...";
            progress = 0;
            string sectorHashes = "";
            for (int i = 0; i < fileSectorsCount; i++) { sectorHashes += BinaryHandler.GetFileHash(projectPath + "\\" + (i + 1).ToString() + ".sector") + ";"; progress = ProgressHandler.GetPercent(fileSectorsCount, i + 1); }
            string sectorHashesHash = BinaryHandler.GetHash(sectorHashes);
            sectorHashes += sectorHashesHash;
            byte[] sectorHashesBytes = Encoding.UTF8.GetBytes(sectorHashes);
            while ((sectorHashesBytes.Length / 4.0) - Math.Truncate(sectorHashesBytes.Length / 4.0) != 0)
            {
                Array.Resize(ref sectorHashesBytes, sectorHashesBytes.Length + 1);
                sectorHashesBytes[sectorHashesBytes.Length - 1] = 0;
            }
            fs_output = new FileStream(projectPath + "\\" + "sectorHashes.sector", FileMode.Create);
            foreach (byte b in sectorHashesBytes) fs_output.WriteByte((byte)b);
            fs_output.Close();
            fs_output.Dispose();
            progress = 100;
            sectorTable.Add(new string[] { "Контр.суммы", new FileInfo(projectPath + "\\" + "sectorHashes.sector").Length.ToString(), "Подготовка", "100%", projectPath + "\\" + "sectorHashes.sector" });
            Thread.Sleep(2000);

            sectorTableReady = true;
            Thread.Sleep(2000);
            thread_encodeSectors.Start();
            //Создание представления
        }

        public static void EncodeSectors()
        {
            status = "Кодирование секторов...";
            for (int i = 0; i < sectorTable.Count; i++) sectorTable[i][3] = "-";
            for (int i = 0; i < sectorTable.Count; i++)
            {
                progress = ProgressHandler.GetPercent(sectorTable.Count, i + 1);
                sectorTable[i][2] = "Кодирование";
                Encoder.encoder_mode = "sector_file";
                if (sectorTable[i][4].Contains("header")) Encoder.encoder_mode = "sector_header";
                if (sectorTable[i][4].Contains("Hashes")) Encoder.encoder_mode = "sector_hashes";
                Encoder.encoder_sectorFileIndex = i + 1;
                Encoder.encoder_inputFilePath = sectorTable[i][4];
                Encoder.encoder_outputFilePath = Path.GetDirectoryName(sectorTable[i][4]) + "\\" + Path.GetFileNameWithoutExtension(sectorTable[i][4]) + ".wav";
                Encoder.thread_encodeFileStereoStream = new Thread(Encoder.EncodeFileStereoStream);
                Encoder.thread_encodeFileStereoStream.Start();
                while (ThreadHandler.GetThreadStatus(Encoder.thread_encodeFileStereoStream) == "Running") Thread.Sleep(50);
            }
            fileConverted = true;
        }

        public static void TapeRecord()
        {
            int timer = 0;
            bool sectorRecordSuccess = false;
            bool errorOccurred = false;
            status = "Запись секторов на ленту...";
            progress = 0;
            for (int i = 0; i < sectorTable.Count; i++) sectorTable[i][3] = "-";
            Decoder.decoderMode = "sector";
            while (sectorRecordSuccess != true) //Запись сектора-заголовка
            {
                errorOccurred = false;
                sectorRecordSuccess = false;
                timer = 0;
                sectorTable[sectorTable.Count - 2][2] = "Запись сектора...";
                sectorTable[sectorTable.Count - 2][3] = "0%";
                AudioIO.PlayWavFile(projectPath + "\\" + "header.wav");
                while (timer < 5) //Таймаут ожидания lead-in
                {
                    if (Decoder.sectorType == "header") { sectorTable[sectorTable.Count - 2][2] = "Lead-in обнаружен..."; sectorTable[sectorTable.Count - 2][3] = "100%"; break; }
                    Thread.Sleep(1000);
                    timer++;
                    if (timer == 5) { errorOccurred = true; }
                }
                if (errorOccurred == false)
                {
                    timer = 0;
                    while (AudioIO.audio_tapePlayStopped == false) Thread.Sleep(50); //Ожидание завершения отправки сигнала
                    while (timer < 5) //Таймаут ожидания lead-out
                    {
                        if (Decoder.sectorGet == false) { sectorTable[sectorTable.Count - 2][2] = "Lead-out обнаружен..."; sectorTable[sectorTable.Count - 2][3] = "100%"; break; }
                        Thread.Sleep(1000);
                        timer++;
                        if (timer == 5) errorOccurred = true;
                    }
                }
                if (errorOccurred == false)
                {
                    sectorTable[sectorTable.Count - 2][2] = "Проверка контр.сумм...";
                    sectorTable[sectorTable.Count - 2][3] = "0%";
                    string sector = Encoding.UTF8.GetString(Decoder.sector.ToArray());
                    //MessageBox.Show(sector);
                    string[] sectorSub = TextHandler.GetStringValues(sector);
                    if (sectorSub.Length == 4)
                    {
                        string header = sectorSub[0] + ";" + sectorSub[1] + ";" + sectorSub[2] + ";";
                        string headerHash = BinaryHandler.GetHash(header);
                        if (sectorSub[3] == headerHash) sectorRecordSuccess = true;
                        else errorOccurred = true;
                        sectorRecordSuccess = true;
                    }
                    else errorOccurred = true;
                }
            }
            for (int i = 0; i < sectorTable.Count; i++)
            {
                sectorTable[i][2] = "Запись lead-in...";
                sectorTable[i][3] = "0%";
                break;
            }
        }

        private void button_record_Click(object sender, EventArgs e)
        {
            if (thread_tapeRecord != null) thread_tapeRecord = new Thread(TapeRecord);
            thread_tapeRecord.Start();
            Decoder.decoderActive = true;
            AudioIO.SignalCaptureInit();
            Decoder.Start();
            //form_main.window_main.timer_signalQualityUpdater.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (progressShow)
            {
                label_status.Visible = true;
                progressBar.Visible = true;
                label_status.Text = status;
                progressBar.Value = progress;
            }
            else
            {
                label_status.Visible = false;
                progressBar.Visible = false;
            }

            if (fileConverted == false) button_record.Enabled = false;
            else button_record.Enabled = true;

            if (sectorTableReady) for (int i = 0; i < sectorTable.Count; i++) for (int k = 0; k < 4; k++) listView1.Items[i].SubItems[k].Text = sectorTable[i][k];
        }

        private void timer_controlHandler_Tick(object sender, EventArgs e)
        {

        }

        private void button_convert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fileSectorsCount + 2; i++)
            {
                listView1.Items.Add("");
                listView1.Items[i].SubItems.Add("");
                listView1.Items[i].SubItems.Add("");
                listView1.Items[i].SubItems.Add("");
            }
            thread_prepareSectors.Start();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AudioIO.audio_tapePlayDeviceId = comboBox1.SelectedIndex;
        }
    }
}
