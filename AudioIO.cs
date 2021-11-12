using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;

namespace AudioDataInterface
{
    public class AudioIO
    {
        //NAudio
        //Префикс "naudio_"
        //////////////////////////////////////////////////////////////////////////////////////
        static WaveIn naudio_waveGraphWaveIn = null;                                            //Поток аудиозахвата для сигналограммы
        static WaveIn naudio_signalWaveIn = null;                                               //Поток аудиозахвата сигнала
        static WasapiOut naudio_WasapiOut = new WasapiOut();                                    //Вывод звука для mp3
        static AudioFileReader naudio_audioFileReader = null;                                   //Читалка аудио файлов для встроенного плеера
        //////////////////////////////////////////////////////////////////////////////////////

        //Буферы
        //Префикс "buff_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static List<short> buff_waveGraphSamples = new List<short>();                                  //Аудиосэмплы для сигналограммы
        //////////////////////////////////////////////////////////////////////////////////////

        //Общие настройки
        //Префикс "audio_"
        //////////////////////////////////////////////////////////////////////////////////////
        public static int audio_recDeviceId = 0;                                                             //ID текущего устройства записи
        public static int audio_playDeviceId = 0;                                                            //ID текущего устройства воспроизведения
        public static int audio_signalHeight = 0;                                                            //Высота сигнала
        public static bool audio_invertSignal = false;                                                       //Инверсия сигнала
        //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Запрашивает список устройств записи
        /// </summary>
        /// <returns></returns>
        public static string[] GetRecDevices()
        {
            string[] recDevices = new string[WaveIn.DeviceCount];
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                recDevices[i] = deviceInfo.ProductName;
            }

            return recDevices;
        }

        /// <summary>
        /// Инициализация захвата звука для сигналограммы
        /// </summary>
        public static void WaveGraphCaptureInit()
        {
            try
            {
                if (naudio_waveGraphWaveIn != null)
                    naudio_waveGraphWaveIn.Dispose();
                naudio_waveGraphWaveIn = new WaveIn();
                naudio_waveGraphWaveIn.DeviceNumber = audio_recDeviceId;
                naudio_waveGraphWaveIn.WaveFormat = new NAudio.Wave.WaveFormat(96000, 1);
                naudio_waveGraphWaveIn.DataAvailable += new EventHandler<WaveInEventArgs>(WaveGraph_DataAvailable);
                naudio_waveGraphWaveIn.StartRecording();
            }
            catch { }
        }

        /// <summary>
        /// Закрытие захвата звука для сигналограммы
        /// </summary>
        public static void WaveGraphCaptureClose()
        {
            try
            {
                if (naudio_waveGraphWaveIn != null)
                    naudio_waveGraphWaveIn.Dispose();
                buff_waveGraphSamples.Clear();
            }
            catch { }
        }

        static void WaveGraph_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            for (int i = 0; i < e.Buffer.Length / 2; i += 2)
            {
                if (!audio_invertSignal)
                    buff_waveGraphSamples.Add((short)(BitConverter.ToInt16(new byte[2] { e.Buffer[i], e.Buffer[i + 1] }, 0) + (short)audio_signalHeight));
                else
                    buff_waveGraphSamples.Add((short)(-BitConverter.ToInt16(new byte[2] { e.Buffer[i], e.Buffer[i + 1] }, 0) + (short)audio_signalHeight));
            }
            MainWindow.class_captureWindow.DrawWaveGraphFrame();
        }
    }
}
