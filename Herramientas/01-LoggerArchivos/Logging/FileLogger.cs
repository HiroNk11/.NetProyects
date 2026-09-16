using System;
using System.IO;

namespace LoggerArchivos.Logging
{
    internal class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }

        public void Log(LogLevel level, string message)
        {
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllLines(_filePath, new[] { line });
        }
    }
}
