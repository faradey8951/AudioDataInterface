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
        public static string[] GetDoubleValue(string line)
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

        /// <summary>
        /// Возвращает список значений, отделяемых точкой с запятой, в строке str
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStringValues(string str)
        {
            List<string> values = new List<string>();
            int index = 0;
            string value = "";
            index = str.IndexOf(";");
            while (index != -1)
            {
                value = "";
                for (int i = 0; i < index; i++) value += str[i];
                str = str.Remove(0, index + 1);
                values.Add(value);
                index = str.IndexOf(";");
            }
            values.Add(str);
            return values.ToArray();
        }
    }
}
