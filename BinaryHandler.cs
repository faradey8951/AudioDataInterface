using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace AudioDataInterface
{
    public class BinaryHandler
    {
        /// <summary>
        /// Возвращает хеш-сумму MD5 для строчки input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Кодирует строчку bin контрольными битами по коду Хэмминга и возвращает бит-массив
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static string[] HammingEncode(string bin)
        {
            List<string> binary = new List<string>();
            foreach (var s in bin)
                binary.Add(s.ToString());
            string[] block = new string[bin.Length + 1 + (int)Math.Ceiling(Math.Log(bin.Length, 2))];
            List<int> controlBitPos = new List<int>(); //Список индексов контрольных бит          
            for (int i = 1; i <= bin.Length; i += i) //Расставляем нули на местах контрольных битов
            {
                block[i - 1] = "0";
                controlBitPos.Add(i);
            }           
            for (int i = 0; binary.Count > 0; i++) //В остальные позиции переписываем полезные биты
            {
                if (block[i] != "0")
                {
                    block[i] = binary[0];
                    binary.RemoveAt(0);
                }
            }           
            while (controlBitPos.Count > 0) //Вычисляем контрольные биты
            {
                List<string> temp = new List<string>(); //Формируемая последовательность
                int chunkSize = controlBitPos[0]; //Размер элемента последовательности
                for (int i = controlBitPos[0], k = 0; k <= chunkSize;)
                {
                    if (i > block.Length)
                        break;
                    if (k >= chunkSize)
                    {
                        i = i + controlBitPos[0];
                        k = 0;
                    }
                    else
                    {
                        temp.Add(block[i - 1]);
                        k++;
                        i++;
                    }
                }
                //Суммируем единицы
                int c = 0;
                foreach (string bit in temp)
                    if (bit == "1") c++;
                if (((double)c / 2) - Math.Truncate((double)c / 2) == 0 && c != 0) //Проверяем четность и записываем бит четности на место контрольного бита
                    block[controlBitPos[0] - 1] = "0";
                else
                    block[controlBitPos[0] - 1] = "1";
                if (c == 0)
                    block[controlBitPos[0] - 1] = "0";
                controlBitPos.RemoveAt(0);
            }
            return block;
        }

        /// <summary>
        /// Проверка блока данных на ошибки.
        /// Возвращает <see langword="null"/>, если ошибки не обнаружены или неисправимы. 
        /// Возвращает исправленный блок данных, если возможно
        /// </summary>
        public static string[] HammingDecode(string bin)
        {
            string unfixableErrorFound = "";
            bool returnZero = false;
            string[] block = new string[bin.Length];
            string decodedData = "";
            List<int> controlBitPos = new List<int>(); //Список индексов контрольных бит 
            List<int> controlBitPos1 = new List<int>(); //Список индексов контрольных бит           
            for (int i = 1; i <= bin.Length; i += i) //Заменяем нулями все полученные контрольные биты
            {
                block[i - 1] = "0";
                controlBitPos.Add(i);
                controlBitPos1.Add(i);
            }            
            for (int i = 0; i < block.Length; i++) //В остальные позиции переписываем полезные биты
            {
                if (block[i] != "0")
                    block[i] = bin[i].ToString();
            }        
            while (controlBitPos.Count > 0) //Вычисляем контрольные биты по полученным данным
            {
                List<string> temp = new List<string>(); //Формируемая последовательность
                int chunkSize = controlBitPos[0]; //Размер элемента последовательности
                for (int i = controlBitPos[0], k = 0; k <= chunkSize;)
                {
                    if (i > block.Length) break;
                    if (k >= chunkSize)
                    {
                        i = i + controlBitPos[0];
                        k = 0;
                    }
                    else
                    {
                        temp.Add(block[i - 1]);
                        k++;
                        i++;
                    }
                }
                //Суммируем единицы
                int c = 0;
                foreach (string bit in temp)
                    if (bit == "1") c++;             
                if (((double)c / 2) - Math.Truncate((double)c / 2) == 0 && c != 0) //Проверяем четность и записываем бит четности на место контрольного бита
                    block[controlBitPos[0] - 1] = "0";
                else
                    block[controlBitPos[0] - 1] = "1";
                if (c == 0)
                    block[controlBitPos[0] - 1] = "0";
                controlBitPos.RemoveAt(0);
            }
            //Собираем индексы неверных контрольных битов
            List<int> badList = new List<int>();
            foreach (int index in controlBitPos1.ToArray())
            {
                if (block[index - 1] != bin[index - 1].ToString())
                    badList.Add(index);
            }
            //Суммируем индексы неверных контрольных битов. Получаем индекс ошибки
            int summ = 0;
            for (int i = 0; i < badList.Count; i++)
                summ += badList[i];
            if (badList.Count == 0) //Если ошибки не обнаружены
            {              
                block = null;
            }
            else
            {
                if (summ <= bin.Length) //Если исправление возможно
                {
                    unfixableErrorFound = "fixed";
                    if (bin[summ - 1] == '0')
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (i != summ - 1)
                                block[i] = bin[i].ToString();
                            else
                                block[i] = "1";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (i != summ - 1)
                                block[i] = bin[i].ToString();
                            else
                                block[i] = "0";
                        }
                    }
                }
                else //Если исправление невозможно
                {
                    //При наличии неисправимых ошибок в блоке - вернуть нули
                    unfixableErrorFound = "error";
                    returnZero = true;
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    decodedData += "0";
                    
                    block = null;
                }
            }

            if (returnZero == false)
            {
                if (block != null) //Возвращаем исправленный вариант
                {
                    decodedData += block[2];
                    decodedData += block[4];
                    decodedData += block[5];
                    decodedData += block[6];
                    decodedData += block[8];
                    decodedData += block[9];
                    decodedData += block[10];
                    decodedData += block[11];
                    decodedData += block[12];
                    decodedData += block[13];
                    decodedData += block[14];
                    decodedData += block[16];
                    decodedData += block[17];
                    decodedData += block[18];
                    decodedData += block[19];
                    decodedData += block[20];
                    decodedData += block[21];
                    decodedData += block[22];
                    decodedData += block[23];
                    decodedData += block[24];
                    decodedData += block[25];
                    decodedData += block[26];
                    decodedData += block[27];
                    decodedData += block[28];
                    decodedData += block[29];
                    decodedData += block[30];
                    decodedData += block[32];
                    decodedData += block[33];
                    decodedData += block[34];
                    decodedData += block[35];
                    decodedData += block[36];
                    decodedData += block[37];
                }
                else //Возвращаем исходный вариант
                {
                    decodedData += bin[2];
                    decodedData += bin[4];
                    decodedData += bin[5];
                    decodedData += bin[6];
                    decodedData += bin[8];
                    decodedData += bin[9];
                    decodedData += bin[10];
                    decodedData += bin[11];
                    decodedData += bin[12];
                    decodedData += bin[13];
                    decodedData += bin[14];
                    decodedData += bin[16];
                    decodedData += bin[17];
                    decodedData += bin[18];
                    decodedData += bin[19];
                    decodedData += bin[20];
                    decodedData += bin[21];
                    decodedData += bin[22];
                    decodedData += bin[23];
                    decodedData += bin[24];
                    decodedData += bin[25];
                    decodedData += bin[26];
                    decodedData += bin[27];
                    decodedData += bin[28];
                    decodedData += bin[29];
                    decodedData += bin[30];
                    decodedData += bin[32];
                    decodedData += bin[33];
                    decodedData += bin[34];
                    decodedData += bin[35];
                    decodedData += bin[36];
                    decodedData += bin[37];
                }
            }
            return new string[] { decodedData, unfixableErrorFound };
        }
    }
}
