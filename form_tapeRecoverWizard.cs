using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AudioDataInterface
{
    public partial class form_tapeRecoverWizard : Form
    {
        static string filePath = "";
        static string projectPath = "Tape Recover Wizard";
        static int fileSize = 0;
        static string fileName = "";
        public static int fileSectorsCount = 0; //Количество секторов для файла
        static FileInfo fileInfo;
        public static Thread thread_tapeRecover = new Thread(TapeRecover);
        public static bool progressShow = false;
        public static string status = "";
        public static int progress = 0;
        public static List<string[]> sectorTable;
        public static bool sectorTableReady = false;
        public static bool errorOccurred = false;
        public static string errorReason = "";
        public static bool fileRecovered = false;
        public static List<string> decodedHashes = new List<string>();
        public form_tapeRecoverWizard()
        {
            InitializeComponent();
        }

        private void groupBox_file_Enter(object sender, EventArgs e)
        {

        }

        public static void TapeRecover()
        {
            progressShow = true;
            progress = 0;
            Decoder.decoderMode = "sector";
            bool sectorRecoverSuccess = false;
            bool headerLeadInDetected = false;
            bool headerLeadOutDetected = false;
            bool hashesLeadInDetected = false;
            bool hashesLeadOutDetected = false;
            int currentSectorIndex = 0;
            int fileSectorSize = 0;
            while (fileRecovered != true)
            {
                while (sectorRecoverSuccess != true) //Получение сектора-заголовка
                {
                    FileStream fs_output = null;
                    status = "Ожидание заголовка...";
                    sectorRecoverSuccess = false;
                    headerLeadInDetected = false;
                    headerLeadOutDetected = false;
                    hashesLeadInDetected = false;
                    hashesLeadOutDetected = false;
                    currentSectorIndex = 0;
                    fileSectorSize = 0;
                    while (headerLeadInDetected == false)
                    {
                        if (Decoder.sectorType == "header") headerLeadInDetected = true;
                        Thread.Sleep(100);
                    }
                    while (headerLeadOutDetected == false && errorOccurred == false)
                    {
                        if (Decoder.sectorGet == false) headerLeadOutDetected = true;
                        Thread.Sleep(100);
                    }
                    if (errorOccurred == false)
                    {
                        string sector = Encoding.UTF8.GetString(Decoder.sector.ToArray());
                        string[] sectorSub = TextHandler.GetStringValues(sector);
                        if (sectorSub.Length == 4)
                        {
                            string header = sectorSub[0] + ";" + sectorSub[1] + ";" + sectorSub[2] + ";";
                            string headerHash = BinaryHandler.GetStringHash(header);
                            if (sectorSub[3].Contains(headerHash))
                            {
                                sectorRecoverSuccess = true;
                                fileSectorsCount = Convert.ToInt16(sectorSub[2]);
                                fileSize = Convert.ToInt16(sectorSub[1]);
                                fileSectorSize = fileSize / fileSectorsCount;
                                fileName = sectorSub[0];
                                while ((fileSectorSize / 8.0) - Math.Truncate(fileSectorSize / 8.0) != 0) fileSectorSize++;
                                for (int k = 0; k < fileSectorsCount; k++) sectorTable.Add(new string[] { (k + 1).ToString(), fileSectorSize.ToString(), "Ожидание", "-", "" });
                                sectorTable.Add(new string[] { "Заголовок", Decoder.sector.ToArray().Length.ToString(), "Получен", "100%", "" });
                                sectorTable.Add(new string[] { "Контр.суммы", Decoder.sector.ToArray().Length.ToString(), "Ожидание", "-", "" });
                                sectorTableReady = true;
                                fs_output = new FileStream(projectPath + "\\" + "header.sector", FileMode.Create);
                                foreach (byte b in Decoder.sector.ToArray()) fs_output.WriteByte(b);
                                fs_output.Close();
                            }
                            else { errorOccurred = true; errorReason = "checksum error"; sectorTable[sectorTable.Count - 2][4] = errorReason; }
                        }
                        else { errorOccurred = true; errorReason = "sector damaged"; sectorTable[sectorTable.Count - 2][4] = errorReason; }

                    }
                }
                sectorRecoverSuccess = false;
                while (sectorRecoverSuccess != true) //Получение сектора-контр.сумм
                {
                    FileStream fs_output = null;
                    status = "Ожидание сектора-контр.сумм...";
                    sectorRecoverSuccess = false;
                    headerLeadInDetected = false;
                    headerLeadOutDetected = false;
                    hashesLeadInDetected = false;
                    hashesLeadOutDetected = false;
                    currentSectorIndex = 0;
                    fileSectorSize = 0;
                    while (hashesLeadInDetected == false)
                    {
                        if (Decoder.sectorType == "hashes") hashesLeadInDetected = true;
                        Thread.Sleep(100);
                    }
                    while (hashesLeadOutDetected == false && errorOccurred == false)
                    {
                        if (Decoder.sectorGet == false) hashesLeadOutDetected = true;
                        Thread.Sleep(100);
                    }
                    if (errorOccurred == false)
                    {
                        string sector = Encoding.UTF8.GetString(Decoder.sector.ToArray());
                        string[] sectorSub = TextHandler.GetStringValues(sector);
                        if (sectorSub.Length == 33)
                        {
                            string hashes = "";
                            for (int i = 0; i < sectorSub.Length - 1; i++) { hashes += sectorSub[i] + ";"; decodedHashes.Add(sectorSub[i]); }
                            string headerHash = BinaryHandler.GetStringHash(hashes);
                            if (sectorSub[32].Contains(headerHash))
                            {
                                sectorRecoverSuccess = true;
                                sectorTable[sectorTable.Count - 1][2] = "Получен";
                                sectorTable[sectorTable.Count - 1][3] = "100%";
                                fs_output = new FileStream(projectPath + "\\" + "sectorHashes.sector", FileMode.Create);
                                foreach (byte b in Decoder.sector.ToArray()) fs_output.WriteByte(b);
                                fs_output.Close();
                            }
                            else { errorOccurred = true; errorReason = "checksum error"; sectorTable[sectorTable.Count - 1][4] = errorReason; }
                        }
                        else { errorOccurred = true; errorReason = "sector damaged"; sectorTable[sectorTable.Count - 1][4] = errorReason; }

                    }
                }
                sectorRecoverSuccess = false;
                while (sectorRecoverSuccess != true) //Получение сектора файла
                {
                    FileStream fs_output = null;
                    status = "Ожидание сектора-файла...";
                    sectorRecoverSuccess = false;
                    currentSectorIndex = 0;
                    fileSectorSize = 0;
                    while (hashesLeadInDetected == false)
                    {
                        if (Decoder.sectorType == "hashes") hashesLeadInDetected = true;
                        Thread.Sleep(100);
                    }
                    while (hashesLeadOutDetected == false && errorOccurred == false)
                    {
                        if (Decoder.sectorGet == false) hashesLeadOutDetected = true;
                        Thread.Sleep(100);
                    }
                    if (errorOccurred == false)
                    {
                        string sector = Encoding.UTF8.GetString(Decoder.sector.ToArray());
                        string[] sectorSub = TextHandler.GetStringValues(sector);
                        if (sectorSub.Length == 33)
                        {
                            string hashes = "";
                            for (int i = 0; i < sectorSub.Length - 1; i++) { hashes += sectorSub[i] + ";"; decodedHashes.Add(sectorSub[i]); }
                            string headerHash = BinaryHandler.GetStringHash(hashes);
                            if (sectorSub[32].Contains(headerHash)) { sectorRecoverSuccess = true; sectorTable[sectorTable.Count - 1][2] = "Получен"; sectorTable[sectorTable.Count - 1][3] = "100%"; }
                            else { errorOccurred = true; errorReason = "checksum error"; sectorTable[sectorTable.Count - 1][4] = errorReason; }
                        }
                        else { errorOccurred = true; errorReason = "sector damaged"; sectorTable[sectorTable.Count - 1][4] = errorReason; }

                    }
                }
            }
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
            label_fileName.Text = fileName;
            label_fileSize.Text = (fileSize / 1024).ToString() + "КБ";
            if (sectorTableReady)
            {
                for (int i = 0; i < sectorTable.Count; i++) for (int k = 0; k < 5; k++) listView1.Items[i].SubItems[k].Text = sectorTable[i][k];
            }
        }

        private void button_recover_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(projectPath)) Directory.Delete(projectPath, true);
            Directory.CreateDirectory(projectPath);
            for (int i = 0; i < 34; i++)
            {
                listView1.Items.Add("");
                listView1.Items[i].SubItems.Add("");
                listView1.Items[i].SubItems.Add("");
                listView1.Items[i].SubItems.Add("");
                listView1.Items[i].SubItems.Add("");
                listView1.Items[i].SubItems.Add("");
            }
            sectorTable = new List<string[]>();
            Decoder.decoderActive = false;
            Decoder.Stop();
            Decoder.ClearBuffers();
            if (thread_tapeRecover != null) thread_tapeRecover = new Thread(TapeRecover);
            thread_tapeRecover.Start();
            Decoder.decoderActive = true;
            AudioIO.SignalCaptureInit();
            Decoder.Start();
        }
    }
}
