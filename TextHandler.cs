using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioDataInterface
{
    public class TextHandler
    {
        /// <summary>
        /// Возвращает значение value строки line формата "var=value"
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string GetLineValue(string line)
        {
            int index = line.IndexOf("=");
            string temp = line;

            if (index != -1)
            {
                for (int i = 0; i <= index; i++)
                    temp = temp.Remove(0, 1);
                return temp;
            }
            else
                return null;
        }

        /// <summary>
        /// Возвращает массив value[] {value1, value2} строки line формата "value1:value2"
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string[] GetDoubleValue(string line)
        {
            string value1 = "";
            string temp = line;
            int index = line.IndexOf(":");

            if (index != -1)
            {
                for (int i = 0; i <= index; i++)
                {
                    value1 += line[i];
                    temp = temp.Remove(0, 1);
                }
                value1 = value1.Remove(value1.Count() - 1, 1);
                return new string[2] { value1, temp };
            }
            else
                return null;
        }
    }
}
