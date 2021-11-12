using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AudioDataInterface
{
    public class FileHandler
    {
        /// <summary>
        /// Проверка статуса доступа файла по пути filePath. createDir создает директорию filePath, если она не существует
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="createDir"></param>
        /// <returns></returns>
        public static string CheckStatus(string filePath, bool createDir)
        {
            if (!Directory.Exists(Path.GetDirectoryName(Path.GetFullPath(filePath))))
            {
                if (createDir == true)
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath)));
                return "!pathExists";
            }
            else
            {
                if (!File.Exists(filePath))
                    return "!fileExists";
                else
                {
                    try
                    {
                        File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    }
                    catch
                    {
                        return "!available";
                    }
                }
            }
            return "available";
        }
    }
}
