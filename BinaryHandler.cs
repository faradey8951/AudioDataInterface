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
        /// Дополняет строчку bin контрольными битами по коду Хэмминга и возвращает бит-массив
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static string[] HCMake(string bin)
        {
            List<string> binary = new List<string>();
            foreach (var s in bin)
                binary.Add(s.ToString());
            string[] block = new string[bin.Length + 1 + (int)Math.Ceiling(Math.Log(bin.Length, 2))];
            List<int> controlBitPos = new List<int>(); //Список индексов контрольных бит 
            //Расставляем нули на местах контрольных битов
            for (int i = 1; i <= bin.Length; i += i)
            {
                block[i - 1] = "0";
                controlBitPos.Add(i);
            }
            //В остальные позиции переписываем полезные биты
            for (int i = 0; binary.Count > 0; i++)
            {
                if (block[i] != "0")
                {
                    block[i] = binary[0];
                    binary.RemoveAt(0);
                }
            }
            //Вычисляем контрольные биты
            while (controlBitPos.Count > 0)
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
                    if (bit == "1")
                        c++;
                //Проверяем четность и записываем бит четности на место контрольного бита
                if (((double)c / 2) - Math.Truncate((double)c / 2) == 0 && c != 0)
                    block[controlBitPos[0] - 1] = "0";
                else
                    block[controlBitPos[0] - 1] = "1";
                if (c == 0)
                    block[controlBitPos[0] - 1] = "0";
                controlBitPos.RemoveAt(0);
            }
            return block;
        }
    }
}
