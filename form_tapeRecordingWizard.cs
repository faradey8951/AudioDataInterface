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
        Thread t = new Thread(TapeRecord);
        public static bool progressShow = false;
        public static string status = "";
        public static int progress = 0;
        public static bool prepareDone = false;
        public form_tapeRecordingWizard()
        {
            InitializeComponent();
        }

        private void form_tapeRecordingWizard_Load(object sender, EventArgs e)
        {

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
            fileInfo = new FileInfo(filePath);
            fileSize = (int)fileInfo.Length / 1024;
            label_fileSize.Text = fileSize.ToString() + " КБ";
        }

        public static void TapeRecord()
        {
            progressShow = true;
            //Разбиение файла на секторы            
            int fileSectorSize = (int)fileInfo.Length / fileSectorsCount; //Размер сектора в байтах
            FileStream fs_input = new FileStream(filePath, FileMode.Open);
            FileStream fs_output;
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
            }
            fs_input.Close();
            fs_input.Dispose();

            //Формирование заголовка
            status = "Формирование заголовка...";
            progress = 0;
            string header = Path.GetFileName(filePath) + ";" + fileSize.ToString() + ";" + fileSectorsCount + ";"; //Заголовок потока
            string headerHash = BinaryHandler.GetHash(header);
            header += headerHash;
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            fs_output = new FileStream(projectPath + "\\" + "header.sector", FileMode.Create);
            foreach (byte b in headerBytes) fs_output.WriteByte((byte)b);
            fs_output.Close();
            fs_output.Dispose();
            progress = 100;
            Thread.Sleep(2000);

            //Расчет контрольных сумм секторов
            status = "Расчет контрольных сумм секторов...";
            progress = 0;
            string sectorHashes = "";
            for (int i = 0; i < fileSectorsCount; i++) { sectorHashes += BinaryHandler.GetFileHash(projectPath + "\\" + (i + 1).ToString() + ".sector") + ";"; progress = ProgressHandler.GetPercent(fileSectorsCount, i + 1); }
            string sectorHashesHash = BinaryHandler.GetHash(sectorHashes);
            sectorHashes += sectorHashesHash;
            byte[] sectorHashesBytes = Encoding.UTF8.GetBytes(sectorHashes);
            fs_output = new FileStream(projectPath + "\\" + "sectorHashes.sector", FileMode.Create);
            foreach (byte b in sectorHashesBytes) fs_output.WriteByte((byte)b);
            fs_output.Close();
            fs_output.Dispose();
            progress = 100;
            Thread.Sleep(2000);

            progressShow = false;
            prepareDone = true;
            //Создание представления
        }
        private void button_record_Click(object sender, EventArgs e)
        {
            t.Start();
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

            if (prepareDone)
            {
                prepareDone = false;
                listView1.Items.Add("Заголовок");
                listView1.Items[0].SubItems.Add(new FileInfo(projectPath + "\\" + "header.sector").Length.ToString());
                listView1.Items[0].SubItems.Add("Подготовка");
                listView1.Items[0].SubItems.Add("100%");
                listView1.Items.Add("Контр.суммы");
                listView1.Items[1].SubItems.Add(new FileInfo(projectPath + "\\" + "sectorHashes.sector").Length.ToString());
                listView1.Items[1].SubItems.Add("Подготовка");
                listView1.Items[1].SubItems.Add("100%");
                for (int i = 0; i < fileSectorsCount; i++)
                {
                    listView1.Items.Add((i + 1).ToString());
                    listView1.Items[i + 2].SubItems.Add(new FileInfo(projectPath + "\\" + (i + 1).ToString() + ".sector").Length.ToString());
                    listView1.Items[i + 2].SubItems.Add("Подготовка");
                    listView1.Items[i + 2].SubItems.Add("100%");
                }

                Encoder.encoder_mode = "sector_header";
                Encoder.encoder_inputFilePath = projectPath + "\\" + "header.sector";
                Encoder.encoder_outputFilePath = projectPath + "\\" + "header.wav";
                Encoder.thread_encodeFileStream = new Thread(Encoder.EncodeFileStereoStream);
                Encoder.thread_encodeFileStream.Start();

            }
        }
    }
}
