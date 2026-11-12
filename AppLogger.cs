using System;
using System.IO;
using System.Text;

namespace WinFormsApp1
{
    public static class AppLogger
    {
        public static event Action<string>? LogAdded;

        private static readonly object Sync = new();
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_log.txt");

        public static string LogFilePath => logFilePath;

        public static void Log(string message)
        {
            Write($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {message}");
        }

        public static void LogException(Exception ex, string source)
        {
            StringBuilder sb = new();
            sb.AppendLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] ИСКЛЮЧЕНИЕ в {source}");
            sb.AppendLine($"Сообщение: {ex.Message}");
            sb.AppendLine($"Тип: {ex.GetType().FullName}");
            sb.AppendLine($"Source: {ex.Source}");
            sb.AppendLine($"TargetSite: {ex.TargetSite}");
            if (ex.InnerException != null)
                sb.AppendLine($"InnerException: {ex.InnerException.GetType().FullName}: {ex.InnerException.Message}");
            sb.AppendLine("StackTrace:");
            sb.AppendLine(ex.StackTrace ?? "StackTrace отсутствует");
            sb.AppendLine(new string('-', 70));
            Write(sb.ToString().TrimEnd());
        }

        private static void Write(string fullMessage)
        {
            LogAdded?.Invoke(fullMessage);
            try
            {
                lock (Sync)
                {
                    File.AppendAllText(logFilePath, fullMessage + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // Логгер не должен ломать приложение.
            }
        }
    }
}
