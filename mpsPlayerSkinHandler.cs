using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace AudioDataInterface
{
    public class mpsPlayerSkinHandler
    {
        public string[] symbols = null;

        public string currentSkinName = "";

        public Image dots;
        public Image symbol_0;
        public Image symbol_1;
        public Image symbol_2;
        public Image symbol_3;
        public Image symbol_4;
        public Image symbol_5;
        public Image symbol_6;
        public Image symbol_7;
        public Image symbol_8;
        public Image symbol_9;
        public Image symbol_A;
        public Image symbol_B;
        public Image symbol_C;
        public Image symbol_D;
        public Image symbol_E;
        public Image symbol_F;
        public Image symbol_G;
        public Image symbol_H;
        public Image symbol_I;
        public Image symbol_J;
        public Image symbol_K;
        public Image symbol_L;
        public Image symbol_M;
        public Image symbol_N;
        public Image symbol_O;
        public Image symbol_P;
        public Image symbol_Q;
        public Image symbol_R;
        public Image symbol_S;
        public Image symbol_T;
        public Image symbol_U;
        public Image symbol_V;
        public Image symbol_W;
        public Image symbol_X;
        public Image symbol_Y;
        public Image symbol_Z;

        public Image symbol_DASH;
        public Image symbol_ASTERISK;
        public Image symbol_SLASH;
        public Image symbol_PLUS;
        public Image symbol_OPENPARENTHESIS;
        public Image symbol_CLOSEPARENTHESIS;
        public Image symbol_COMMA;
        public Image symbol_APOSTROPHE;
        public Image symbol_PIPE;

        public Image CD_disc1Empty;
        public Image CD_disc1Detected;
        public Image CD_disc1Selected;
        public Image CD_disc2Empty;
        public Image CD_disc2Detected;
        public Image CD_disc2Selected;
        public Image CD_disc3Empty;
        public Image CD_disc3Detected;
        public Image CD_disc3Selected;
        public Image CD_pause;
        public Image CD_play;

        public Image tape_cassette;

        public Image trackCalendar_1;
        public Image trackCalendar_2;
        public Image trackCalendar_3;
        public Image trackCalendar_4;
        public Image trackCalendar_5;
        public Image trackCalendar_6;
        public Image trackCalendar_7;
        public Image trackCalendar_8;
        public Image trackCalendar_9;
        public Image trackCalendar_10;
        public Image trackCalendar_11;
        public Image trackCalendar_12;
        public Image trackCalendar_13;
        public Image trackCalendar_14;
        public Image trackCalendar_15;
        public Image trackCalendar_16;

        public Image spectrum_border;

        public Image runningIndicator_stop;

        public Image runningIndicator_CD_1;
        public Image runningIndicator_CD_2;
        public Image runningIndicator_CD_3;
        public Image runningIndicator_CD_4;
        public Image runningIndicator_CD_5;
        public Image runningIndicator_CD_6;
        public Image runningIndicator_CD_7;
        public Image runningIndicator_CD_8;
        public Image runningIndicator_CD_9;
        public Image runningIndicator_CD_10;
        public Image runningIndicator_CD_11;
        public Image runningIndicator_CD_12;

        public Image runningIndicator_tape_FWD_0;
        public Image runningIndicator_tape_FWD_1;
        public Image runningIndicator_tape_FWD_2;
        public Image runningIndicator_tape_FWD_3;
        public Image runningIndicator_tape_FWD_4;
        public Image runningIndicator_tape_FWD_5;
        public Image runningIndicator_tape_FWD_6;
        public Image runningIndicator_tape_FWD_7;
        public Image runningIndicator_tape_FWD_8;
        public Image runningIndicator_tape_FWD_9;
        public Image runningIndicator_tape_FWD_10;
        public Image runningIndicator_tape_FWD_11;
        public Image runningIndicator_tape_FWD_12;

        public Image runningIndicator_tape_RVS_0;
        public Image runningIndicator_tape_RVS_1;
        public Image runningIndicator_tape_RVS_2;
        public Image runningIndicator_tape_RVS_3;
        public Image runningIndicator_tape_RVS_4;
        public Image runningIndicator_tape_RVS_5;
        public Image runningIndicator_tape_RVS_6;
        public Image runningIndicator_tape_RVS_7;
        public Image runningIndicator_tape_RVS_8;
        public Image runningIndicator_tape_RVS_9;
        public Image runningIndicator_tape_RVS_10;
        public Image runningIndicator_tape_RVS_11;
        public Image runningIndicator_tape_RVS_12;

        public Image[] image_CD;
        public Image[] image_runningIndicator;
        public Image[] image_symbols;
        public Image[] image_tape;
        public Image[] image_trackCalendar;
        public Image[] image_misc;

        public Color color_spectrumBaseColor;
        public Color color_spectrumHighColor;
        public Color color_spectrumPeakBaseColor;
        public Color color_spectrumPeakHighColor;
        public Color color_playerBackColor;


        public static List<string[]> GetSkins()
        {
            List<string[]> skins = new List<string[]>();
            if (!Directory.Exists("mpsPlayerSkins")) Directory.CreateDirectory("mpsPlayerSkins");
            string[] skinDirs = Directory.GetDirectories("mpsPlayerSkins");
            foreach (string dir in skinDirs)
            {
                try
                {
                    if (File.Exists(dir + "\\skin.txt"))
                    {
                        FileStream fs = new FileStream(dir + "\\skin.txt", FileMode.Open);
                        StreamReader sr = new StreamReader(fs);
                        string name = TextHandler.GetLineValue(sr.ReadLine());
                        string author = TextHandler.GetLineValue(sr.ReadLine());
                        fs.Close();
                        sr.Close();
                        skins.Add(new string[] { dir, name, author });
                    }
                }
                catch { }
            }
            return skins;
        }

        /// <summary>
        /// Сохраняет изменения разметки, внесенные редактором, в скин
        /// </summary>
        public void WriteLayout()
        {
            string dir = "";
            var skins = GetSkins();
            if (currentSkinName == "") dir = "mpsPlayerSkins\\Default";
            else dir = currentSkinName;
            if (File.Exists(dir + "\\layout.txt"))
            {
                FileStream fs = new FileStream(dir + "\\layout.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("1symbolPosX=" + form_main.window_main.pictureBox_symbol1.Location.X); sw.WriteLine("1symbolPosY=" + form_main.window_main.pictureBox_symbol1.Location.Y);
                sw.WriteLine("1symbolSizeX=" + form_main.window_main.pictureBox_symbol1.Size.Width); sw.WriteLine("1symbolSizeY=" + form_main.window_main.pictureBox_symbol1.Size.Height);
                sw.WriteLine("2symbolPosX=" + form_main.window_main.pictureBox_symbol2.Location.X); sw.WriteLine("2symbolPosY=" + form_main.window_main.pictureBox_symbol2.Location.Y);
                sw.WriteLine("2symbolSizeX=" + form_main.window_main.pictureBox_symbol2.Size.Width); sw.WriteLine("2symbolSizeY=" + form_main.window_main.pictureBox_symbol2.Size.Height);
                sw.WriteLine("3symbolPosX=" + form_main.window_main.pictureBox_symbol3.Location.X); sw.WriteLine("3symbolPosY=" + form_main.window_main.pictureBox_symbol3.Location.Y);
                sw.WriteLine("3symbolSizeX=" + form_main.window_main.pictureBox_symbol3.Size.Width); sw.WriteLine("3symbolSizeY=" + form_main.window_main.pictureBox_symbol3.Size.Height);
                sw.WriteLine("4symbolPosX=" + form_main.window_main.pictureBox_symbol4.Location.X); sw.WriteLine("4symbolPosY=" + form_main.window_main.pictureBox_symbol4.Location.Y);
                sw.WriteLine("4symbolSizeX=" + form_main.window_main.pictureBox_symbol4.Size.Width); sw.WriteLine("4symbolSizeY=" + form_main.window_main.pictureBox_symbol4.Size.Height);
                sw.WriteLine("5symbolPosX=" + form_main.window_main.pictureBox_symbol5.Location.X); sw.WriteLine("5symbolPosY=" + form_main.window_main.pictureBox_symbol5.Location.Y);
                sw.WriteLine("5symbolSizeX=" + form_main.window_main.pictureBox_symbol5.Size.Width); sw.WriteLine("5symbolSizeY=" + form_main.window_main.pictureBox_symbol5.Size.Height);

                sw.WriteLine("6symbolPosX=" + form_main.window_main.pictureBox_symbol6.Location.X);
                sw.WriteLine("6symbolPosY=" + form_main.window_main.pictureBox_symbol6.Location.Y);
                sw.WriteLine("6symbolSizeX=" + form_main.window_main.pictureBox_symbol6.Size.Width);
                sw.WriteLine("6symbolSizeY=" + form_main.window_main.pictureBox_symbol6.Size.Height);

                sw.WriteLine("7symbolPosX=" + form_main.window_main.pictureBox_symbol7.Location.X);
                sw.WriteLine("7symbolPosY=" + form_main.window_main.pictureBox_symbol7.Location.Y);
                sw.WriteLine("7symbolSizeX=" + form_main.window_main.pictureBox_symbol7.Size.Width);
                sw.WriteLine("7symbolSizeY=" + form_main.window_main.pictureBox_symbol7.Size.Height);

                sw.WriteLine("8symbolPosX=" + form_main.window_main.pictureBox_symbol8.Location.X);
                sw.WriteLine("8symbolPosY=" + form_main.window_main.pictureBox_symbol8.Location.Y);
                sw.WriteLine("8symbolSizeX=" + form_main.window_main.pictureBox_symbol8.Size.Width);
                sw.WriteLine("8symbolSizeY=" + form_main.window_main.pictureBox_symbol8.Size.Height);

                sw.WriteLine("9symbolPosX=" + form_main.window_main.pictureBox_symbol9.Location.X);
                sw.WriteLine("9symbolPosY=" + form_main.window_main.pictureBox_symbol9.Location.Y);
                sw.WriteLine("9symbolSizeX=" + form_main.window_main.pictureBox_symbol9.Size.Width);
                sw.WriteLine("9symbolSizeY=" + form_main.window_main.pictureBox_symbol9.Size.Height);

                sw.WriteLine("10symbolPosX=" + form_main.window_main.pictureBox_symbol10.Location.X);
                sw.WriteLine("10symbolPosY=" + form_main.window_main.pictureBox_symbol10.Location.Y);
                sw.WriteLine("10symbolSizeX=" + form_main.window_main.pictureBox_symbol10.Size.Width);
                sw.WriteLine("10symbolSizeY=" + form_main.window_main.pictureBox_symbol10.Size.Height);

                sw.WriteLine("dotsPosX=" + form_main.window_main.pictureBox_dots.Location.X);
                sw.WriteLine("dotsPosY=" + form_main.window_main.pictureBox_dots.Location.Y);
                sw.WriteLine("dotsSizeX=" + form_main.window_main.pictureBox_dots.Size.Width);
                sw.WriteLine("dotsSizeY=" + form_main.window_main.pictureBox_dots.Size.Height);

                sw.WriteLine("disc1PosX=" + form_main.window_main.pictureBox_disc1.Location.X);
                sw.WriteLine("disc1PosY=" + form_main.window_main.pictureBox_disc1.Location.Y);
                sw.WriteLine("disc1SizeX=" + form_main.window_main.pictureBox_disc1.Size.Width);
                sw.WriteLine("disc1SizeY=" + form_main.window_main.pictureBox_disc1.Size.Height);

                sw.WriteLine("disc2PosX=" + form_main.window_main.pictureBox_disc2.Location.X);
                sw.WriteLine("disc2PosY=" + form_main.window_main.pictureBox_disc2.Location.Y);
                sw.WriteLine("disc2SizeX=" + form_main.window_main.pictureBox_disc2.Size.Width);
                sw.WriteLine("disc2SizeY=" + form_main.window_main.pictureBox_disc2.Size.Height);

                sw.WriteLine("disc3PosX=" + form_main.window_main.pictureBox_disc3.Location.X);
                sw.WriteLine("disc3PosY=" + form_main.window_main.pictureBox_disc3.Location.Y);
                sw.WriteLine("disc3SizeX=" + form_main.window_main.pictureBox_disc3.Size.Width);
                sw.WriteLine("disc3SizeY=" + form_main.window_main.pictureBox_disc3.Size.Height);

                sw.WriteLine("playPausePosX=" + form_main.window_main.pictureBox_playPause.Location.X);
                sw.WriteLine("playPausePosY=" + form_main.window_main.pictureBox_playPause.Location.Y);
                sw.WriteLine("playPauseSizeX=" + form_main.window_main.pictureBox_playPause.Size.Width);
                sw.WriteLine("playPauseSizeY=" + form_main.window_main.pictureBox_playPause.Size.Height);

                sw.WriteLine("cassettePosX=" + form_main.window_main.pictureBox_cassette.Location.X);
                sw.WriteLine("cassettePosY=" + form_main.window_main.pictureBox_cassette.Location.Y);
                sw.WriteLine("cassetteSizeX=" + form_main.window_main.pictureBox_cassette.Size.Width);
                sw.WriteLine("cassetteSizeY=" + form_main.window_main.pictureBox_cassette.Size.Height);

                sw.WriteLine("spectrumBorder1PosX=" + form_main.window_main.pictureBox_spectrumBorder1.Location.X);
                sw.WriteLine("spectrumBorder1PosY=" + form_main.window_main.pictureBox_spectrumBorder1.Location.Y);
                sw.WriteLine("spectrumBorder1SizeX=" + form_main.window_main.pictureBox_spectrumBorder1.Size.Width);
                sw.WriteLine("spectrumBorder1SizeY=" + form_main.window_main.pictureBox_spectrumBorder1.Size.Height);

                sw.WriteLine("spectrumBorder2PosX=" + form_main.window_main.pictureBox_spectrumBorder2.Location.X);
                sw.WriteLine("spectrumBorder2PosY=" + form_main.window_main.pictureBox_spectrumBorder2.Location.Y);
                sw.WriteLine("spectrumBorder2SizeX=" + form_main.window_main.pictureBox_spectrumBorder2.Size.Width);
                sw.WriteLine("spectrumBorder2SizeY=" + form_main.window_main.pictureBox_spectrumBorder2.Size.Height);

                sw.WriteLine("runningIndicatorPosX=" + form_main.window_main.pictureBox_runningIndicator.Location.X);
                sw.WriteLine("runningIndicatorPosY=" + form_main.window_main.pictureBox_runningIndicator.Location.Y);
                sw.WriteLine("runningIndicatorSizeX=" + form_main.window_main.pictureBox_runningIndicator.Size.Width);
                sw.WriteLine("runningIndicatorSizeY=" + form_main.window_main.pictureBox_runningIndicator.Size.Height);

                sw.WriteLine("track1PosX=" + form_main.window_main.pictureBox_track1.Location.X);
                sw.WriteLine("track1PosY=" + form_main.window_main.pictureBox_track1.Location.Y);
                sw.WriteLine("track1SizeX=" + form_main.window_main.pictureBox_track1.Size.Width);
                sw.WriteLine("track1SizeY=" + form_main.window_main.pictureBox_track1.Size.Height);

                sw.WriteLine("track2PosX=" + form_main.window_main.pictureBox_track2.Location.X);
                sw.WriteLine("track2PosY=" + form_main.window_main.pictureBox_track2.Location.Y);
                sw.WriteLine("track2SizeX=" + form_main.window_main.pictureBox_track2.Size.Width);
                sw.WriteLine("track2SizeY=" + form_main.window_main.pictureBox_track2.Size.Height);

                sw.WriteLine("track3PosX=" + form_main.window_main.pictureBox_track3.Location.X);
                sw.WriteLine("track3PosY=" + form_main.window_main.pictureBox_track3.Location.Y);
                sw.WriteLine("track3SizeX=" + form_main.window_main.pictureBox_track3.Size.Width);
                sw.WriteLine("track3SizeY=" + form_main.window_main.pictureBox_track3.Size.Height);

                sw.WriteLine("track4PosX=" + form_main.window_main.pictureBox_track4.Location.X);
                sw.WriteLine("track4PosY=" + form_main.window_main.pictureBox_track4.Location.Y);
                sw.WriteLine("track4SizeX=" + form_main.window_main.pictureBox_track4.Size.Width);
                sw.WriteLine("track4SizeY=" + form_main.window_main.pictureBox_track4.Size.Height);

                sw.WriteLine("track5PosX=" + form_main.window_main.pictureBox_track5.Location.X);
                sw.WriteLine("track5PosY=" + form_main.window_main.pictureBox_track5.Location.Y);
                sw.WriteLine("track5SizeX=" + form_main.window_main.pictureBox_track5.Size.Width);
                sw.WriteLine("track5SizeY=" + form_main.window_main.pictureBox_track5.Size.Height);

                sw.WriteLine("track6PosX=" + form_main.window_main.pictureBox_track6.Location.X);
                sw.WriteLine("track6PosY=" + form_main.window_main.pictureBox_track6.Location.Y);
                sw.WriteLine("track6SizeX=" + form_main.window_main.pictureBox_track6.Size.Width);
                sw.WriteLine("track6SizeY=" + form_main.window_main.pictureBox_track6.Size.Height);

                sw.WriteLine("track7PosX=" + form_main.window_main.pictureBox_track7.Location.X);
                sw.WriteLine("track7PosY=" + form_main.window_main.pictureBox_track7.Location.Y);
                sw.WriteLine("track7SizeX=" + form_main.window_main.pictureBox_track7.Size.Width);
                sw.WriteLine("track7SizeY=" + form_main.window_main.pictureBox_track7.Size.Height);

                sw.WriteLine("track8PosX=" + form_main.window_main.pictureBox_track8.Location.X);
                sw.WriteLine("track8PosY=" + form_main.window_main.pictureBox_track8.Location.Y);
                sw.WriteLine("track8SizeX=" + form_main.window_main.pictureBox_track8.Size.Width);
                sw.WriteLine("track8SizeY=" + form_main.window_main.pictureBox_track8.Size.Height);

                sw.WriteLine("track9PosX=" + form_main.window_main.pictureBox_track9.Location.X);
                sw.WriteLine("track9PosY=" + form_main.window_main.pictureBox_track9.Location.Y);
                sw.WriteLine("track9SizeX=" + form_main.window_main.pictureBox_track9.Size.Width);
                sw.WriteLine("track9SizeY=" + form_main.window_main.pictureBox_track9.Size.Height);

                sw.WriteLine("10trackPosX=" + form_main.window_main.pictureBox_track10.Location.X);
                sw.WriteLine("10trackPosY=" + form_main.window_main.pictureBox_track10.Location.Y);
                sw.WriteLine("10trackSizeX=" + form_main.window_main.pictureBox_track10.Size.Width);
                sw.WriteLine("10trackSizeY=" + form_main.window_main.pictureBox_track10.Size.Height);

                sw.WriteLine("11trackPosX=" + form_main.window_main.pictureBox_track11.Location.X);
                sw.WriteLine("11trackPosY=" + form_main.window_main.pictureBox_track11.Location.Y);
                sw.WriteLine("11trackSizeX=" + form_main.window_main.pictureBox_track11.Size.Width);
                sw.WriteLine("11trackSizeY=" + form_main.window_main.pictureBox_track11.Size.Height);

                sw.WriteLine("12trackPosX=" + form_main.window_main.pictureBox_track12.Location.X);
                sw.WriteLine("12trackPosY=" + form_main.window_main.pictureBox_track12.Location.Y);
                sw.WriteLine("12trackSizeX=" + form_main.window_main.pictureBox_track12.Size.Width);
                sw.WriteLine("12trackSizeY=" + form_main.window_main.pictureBox_track12.Size.Height);

                sw.WriteLine("13trackPosX=" + form_main.window_main.pictureBox_track13.Location.X);
                sw.WriteLine("13trackPosY=" + form_main.window_main.pictureBox_track13.Location.Y);
                sw.WriteLine("13trackSizeX=" + form_main.window_main.pictureBox_track13.Size.Width);
                sw.WriteLine("13trackSizeY=" + form_main.window_main.pictureBox_track13.Size.Height);

                sw.WriteLine("14trackPosX=" + form_main.window_main.pictureBox_track14.Location.X);
                sw.WriteLine("14trackPosY=" + form_main.window_main.pictureBox_track14.Location.Y);
                sw.WriteLine("14trackSizeX=" + form_main.window_main.pictureBox_track14.Size.Width);
                sw.WriteLine("14trackSizeY=" + form_main.window_main.pictureBox_track14.Size.Height);

                sw.WriteLine("15trackPosX=" + form_main.window_main.pictureBox_track15.Location.X);
                sw.WriteLine("15trackPosY=" + form_main.window_main.pictureBox_track15.Location.Y);
                sw.WriteLine("15trackSizeX=" + form_main.window_main.pictureBox_track15.Size.Width);
                sw.WriteLine("15trackSizeY=" + form_main.window_main.pictureBox_track15.Size.Height);

                sw.WriteLine("16trackPosX=" + form_main.window_main.pictureBox_track16.Location.X);
                sw.WriteLine("16trackPosY=" + form_main.window_main.pictureBox_track16.Location.Y);
                sw.WriteLine("16trackSizeX=" + form_main.window_main.pictureBox_track16.Size.Width);
                sw.WriteLine("16trackSizeY=" + form_main.window_main.pictureBox_track16.Size.Height);

                sw.Close();
                fs.Close();
            }
        }

        public void Load()
        {
            string dir = "";
            var skins = GetSkins();
            if (currentSkinName == "") dir = "mpsPlayerSkins\\Default";
            else dir = currentSkinName;
            string[] CD = new string[] { "CD\\disc1Detected.png", "CD\\disc1Empty.png", "CD\\disc1Selected.png", "CD\\disc2Detected.png", "CD\\disc2Empty.png", "CD\\disc2Selected.png", "CD\\disc3Detected.png", "CD\\disc3Empty.png", "CD\\disc3Selected.png", "CD\\play.png", "CD\\pause.png" };
            string[] runningIndicator = new string[] { "Running Indicator\\STOP.png", "Running Indicator\\CD\\1.png", "Running Indicator\\CD\\2.png", "Running Indicator\\CD\\3.png", "Running Indicator\\CD\\4.png", "Running Indicator\\CD\\5.png", "Running Indicator\\CD\\6.png", "Running Indicator\\CD\\7.png", "Running Indicator\\CD\\8.png", "Running Indicator\\CD\\9.png", "Running Indicator\\CD\\10.png", "Running Indicator\\CD\\11.png", "Running Indicator\\CD\\12.png", "Running Indicator\\TAPE\\FWD\\0.png", "Running Indicator\\TAPE\\FWD\\1.png", "Running Indicator\\TAPE\\FWD\\2.png", "Running Indicator\\TAPE\\FWD\\3.png", "Running Indicator\\TAPE\\FWD\\4.png", "Running Indicator\\TAPE\\FWD\\5.png", "Running Indicator\\TAPE\\FWD\\6.png", "Running Indicator\\TAPE\\FWD\\7.png", "Running Indicator\\TAPE\\FWD\\8.png", "Running Indicator\\TAPE\\FWD\\9.png", "Running Indicator\\TAPE\\FWD\\10.png", "Running Indicator\\TAPE\\FWD\\11.png", "Running Indicator\\TAPE\\FWD\\12.png", "Running Indicator\\TAPE\\RVS\\0.png", "Running Indicator\\TAPE\\RVS\\1.png", "Running Indicator\\TAPE\\RVS\\2.png", "Running Indicator\\TAPE\\RVS\\3.png", "Running Indicator\\TAPE\\RVS\\4.png", "Running Indicator\\TAPE\\RVS\\5.png", "Running Indicator\\TAPE\\RVS\\6.png", "Running Indicator\\TAPE\\RVS\\7.png", "Running Indicator\\TAPE\\RVS\\8.png", "Running Indicator\\TAPE\\RVS\\9.png", "Running Indicator\\TAPE\\RVS\\10.png", "Running Indicator\\TAPE\\RVS\\11.png", "Running Indicator\\TAPE\\RVS\\12.png" };
            symbols = new string[] { "Symbols\\0symbol.png", "Symbols\\1symbol.png", "Symbols\\2symbol.png", "Symbols\\3symbol.png", "Symbols\\4symbol.png", "Symbols\\5symbol.png", "Symbols\\6symbol.png", "Symbols\\7symbol.png", "Symbols\\8symbol.png", "Symbols\\9symbol.png", "Symbols\\Csymbol.png", "Symbols\\Dsymbol.png", "Symbols\\Msymbol.png", "Symbols\\Psymbol.png", "Symbols\\Tsymbol.png", "Symbols\\Asymbol.png", "Symbols\\Esymbol.png", "Symbols\\DASHsymbol.png", "Symbols\\DOTS.png", "Symbols\\Bsymbol.png", "Symbols\\Fsymbol.png", "Symbols\\Gsymbol.png", "Symbols\\Hsymbol.png", "Symbols\\Isymbol.png", "Symbols\\Jsymbol.png", "Symbols\\Ksymbol.png", "Symbols\\Lsymbol.png", "Symbols\\Nsymbol.png", "Symbols\\Osymbol.png", "Symbols\\Qsymbol.png", "Symbols\\Rsymbol.png", "Symbols\\Ssymbol.png", "Symbols\\Usymbol.png", "Symbols\\Vsymbol.png", "Symbols\\Wsymbol.png", "Symbols\\Xsymbol.png", "Symbols\\Ysymbol.png", "Symbols\\Zsymbol.png", "Symbols\\ASTERISKsymbol.png", "Symbols\\SLASHsymbol.png", "Symbols\\OPENPARENTHESISsymbol.png", "Symbols\\CLOSEPARENTHESISsymbol.png", "Symbols\\COMMAsymbol.png", "Symbols\\APOSTROPHEsymbol.png", "Symbols\\PIPEsymbol.png", "Symbols\\PLUSsymbol.png" };
            string[] tape = new string[] { "TAPE\\cassette.png" };
            string[] trackCalendar = new string[] { "Track Calendar\\1.png", "Track Calendar\\2.png", "Track Calendar\\3.png", "Track Calendar\\4.png", "Track Calendar\\5.png", "Track Calendar\\6.png", "Track Calendar\\7.png", "Track Calendar\\8.png", "Track Calendar\\9.png", "Track Calendar\\10.png", "Track Calendar\\11.png", "Track Calendar\\12.png", "Track Calendar\\13.png", "Track Calendar\\14.png", "Track Calendar\\15.png", "Track Calendar\\16.png" };
            string[] misc = new string[] { "border.png" };
            image_CD = new Image[] { CD_disc1Detected, CD_disc1Empty, CD_disc1Selected, CD_disc2Detected, CD_disc2Empty, CD_disc2Selected, CD_disc3Detected, CD_disc3Empty, CD_disc3Selected, CD_play, CD_pause };
            image_runningIndicator = new Image[] { runningIndicator_stop, runningIndicator_CD_1, runningIndicator_CD_2, runningIndicator_CD_3, runningIndicator_CD_4, runningIndicator_CD_5, runningIndicator_CD_6, runningIndicator_CD_7, runningIndicator_CD_8, runningIndicator_CD_9, runningIndicator_CD_10, runningIndicator_CD_11, runningIndicator_CD_12, runningIndicator_tape_FWD_0, runningIndicator_tape_FWD_1, runningIndicator_tape_FWD_2, runningIndicator_tape_FWD_3, runningIndicator_tape_FWD_4, runningIndicator_tape_FWD_5, runningIndicator_tape_FWD_6, runningIndicator_tape_FWD_7, runningIndicator_tape_FWD_8, runningIndicator_tape_FWD_9, runningIndicator_tape_FWD_10, runningIndicator_tape_FWD_11, runningIndicator_tape_FWD_12, runningIndicator_tape_RVS_0, runningIndicator_tape_RVS_1, runningIndicator_tape_RVS_2, runningIndicator_tape_RVS_3, runningIndicator_tape_RVS_4, runningIndicator_tape_RVS_5, runningIndicator_tape_RVS_6, runningIndicator_tape_RVS_7, runningIndicator_tape_RVS_8, runningIndicator_tape_RVS_9, runningIndicator_tape_RVS_10, runningIndicator_tape_RVS_11, runningIndicator_tape_RVS_12 };
            image_symbols = new Image[] { symbol_0, symbol_1, symbol_2, symbol_3, symbol_4, symbol_5, symbol_6, symbol_7, symbol_8, symbol_9, symbol_C, symbol_D, symbol_M, symbol_P, symbol_T, symbol_A, symbol_E, symbol_DASH, dots, symbol_B, symbol_F, symbol_G, symbol_H, symbol_I, symbol_J, symbol_K, symbol_L, symbol_N, symbol_O, symbol_Q, symbol_R, symbol_S, symbol_U, symbol_V, symbol_W, symbol_X, symbol_Y, symbol_Z, symbol_ASTERISK, symbol_SLASH, symbol_OPENPARENTHESIS, symbol_CLOSEPARENTHESIS, symbol_COMMA, symbol_APOSTROPHE, symbol_PIPE, symbol_PLUS };
            image_tape = new Image[] { tape_cassette };
            image_trackCalendar = new Image[] { trackCalendar_1, trackCalendar_2, trackCalendar_3, trackCalendar_4, trackCalendar_5, trackCalendar_6, trackCalendar_7, trackCalendar_8, trackCalendar_9, trackCalendar_10, trackCalendar_11, trackCalendar_12, trackCalendar_13, trackCalendar_14, trackCalendar_15, trackCalendar_16 };
            image_misc = new Image[] { spectrum_border };
            for (int i = 0; i < CD.Length; i++) if (File.Exists(dir + "\\" + CD[i])) { image_CD[i] = Image.FromFile(dir + "\\" + CD[i]); } else image_CD[i] = Properties.Resources.delete;
            for (int i = 0; i < runningIndicator.Length; i++) if (File.Exists(dir + "\\" + runningIndicator[i])) { image_runningIndicator[i] = Image.FromFile(dir + "\\" + runningIndicator[i]); } else image_runningIndicator[i] = Properties.Resources.delete;
            for (int i = 0; i < symbols.Length; i++) if (File.Exists(dir + "\\" + symbols[i])) { image_symbols[i] = Image.FromFile(dir + "\\" + symbols[i]); } else image_symbols[i] = Properties.Resources.delete;
            for (int i = 0; i < tape.Length; i++) if (File.Exists(dir + "\\" + tape[i])) { image_tape[i] = Image.FromFile(dir + "\\" + tape[i]); } else image_tape[i] = Properties.Resources.delete;
            for (int i = 0; i < trackCalendar.Length; i++) if (File.Exists(dir + "\\" + trackCalendar[i])) { image_trackCalendar[i] = Image.FromFile(dir + "\\" + trackCalendar[i]); } else image_trackCalendar[i] = Properties.Resources.delete;
            for (int i = 0; i < misc.Length; i++) if (File.Exists(dir + "\\" + misc[i])) { image_misc[i] = Image.FromFile(dir + "\\" + misc[i]); } else image_misc[i] = Properties.Resources.delete;
            if (File.Exists(dir + "\\window.txt"))
            {
                FileStream fs = new FileStream(dir + "\\window.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                form_main.window_main.MinimumSize = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.Size = form_main.window_main.MinimumSize;
                sr.Close();
                fs.Close();
            }
            if (File.Exists(dir + "\\layout.txt"))
            {
                FileStream fs = new FileStream(dir + "\\layout.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                form_main.window_main.pictureBox_symbol1.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol1.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol2.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol2.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol3.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol3.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol4.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol4.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol5.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol5.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol6.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol6.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol7.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol7.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol8.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol8.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol9.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol9.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol10.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_symbol10.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_dots.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_dots.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_disc1.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_disc1.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_disc2.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_disc2.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_disc3.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_disc3.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_playPause.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_playPause.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_cassette.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_cassette.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_spectrumBorder1.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_spectrumBorder1.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_spectrumBorder2.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_spectrumBorder2.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_runningIndicator.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_runningIndicator.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track1.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track1.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track2.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track2.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track3.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track3.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track4.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track4.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track5.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track5.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track6.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track6.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track7.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track7.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track8.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track8.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track9.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track9.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track10.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track10.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track11.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track11.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track12.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track12.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track13.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track13.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track14.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track14.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track15.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track15.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track16.Location = new Point(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                form_main.window_main.pictureBox_track16.Size = new Size(Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())), Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine())));
                sr.Close();
                fs.Close();
            }
            if (File.Exists(dir + "\\colors.txt"))
            {
                FileStream fs = new FileStream(dir + "\\colors.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                color_spectrumBaseColor = ColorTranslator.FromHtml(TextHandler.GetLineValue(sr.ReadLine()));
                color_spectrumHighColor = ColorTranslator.FromHtml(TextHandler.GetLineValue(sr.ReadLine()));
                color_spectrumPeakBaseColor = ColorTranslator.FromHtml(TextHandler.GetLineValue(sr.ReadLine()));
                color_spectrumPeakHighColor = ColorTranslator.FromHtml(TextHandler.GetLineValue(sr.ReadLine()));
                color_playerBackColor = ColorTranslator.FromHtml(TextHandler.GetLineValue(sr.ReadLine()));
                sr.Close();
                fs.Close();
            }
            if (File.Exists(dir + "\\spectrum.txt"))
            {
                FileStream fs = new FileStream(dir + "\\spectrum.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                form_main.spectrumBarX0P = Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.spectrumBarY0P = Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.spectrumBarWidth = Convert.ToDouble(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.spectrumBarHeight = Convert.ToDouble(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.spectrumBarSegmentWidthCount = Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.spectrumBarSegmentHeightCount = Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.spectrumBarSegmentDeltaCount = Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.window_main.timer_mpsPlayerSpectrumHandler.Interval = Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine()));
                form_main.window_main.timer_mpsPlayerSpectrumUpdater.Interval = form_main.window_main.timer_mpsPlayerSpectrumHandler.Interval;
                form_main.mpsPlayer_spectrumMode = TextHandler.GetLineValue(sr.ReadLine());
                form_main.mpsPlayer_peakHoldTimeDelay = Convert.ToInt16(TextHandler.GetLineValue(sr.ReadLine()));
                sr.Close();
                fs.Close();
            }
            GC.Collect();
        }
    }
}
