using System;
using System.IO;

namespace DBComparer.Helpers
{
    public static class FileHelper
    {
        public static string GetBinDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? "");
        }

        public static string CreateFile(string name)
        {
            string logDirectory = GetBinDirectory() + @"\Arquivo";

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string path = Path.Combine(logDirectory, $"{name}-{DateTime.Now.ToString("dd-MM-yyyy-hh-mm")}.xlsx");
            
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            return path;
        }
    }
}
