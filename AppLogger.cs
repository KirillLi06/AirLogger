using System;
using System.IO;
using System.Text;
using Microsoft.Data.SqlClient;

namespace WinFormsApp1
{
    public static class AppLogger
    {
        public static event Action<string>? LogAdded;
        public static bool RealTimeDbExceptionLogging { get; set; } = true;

        private static readonly object Sync = new();
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_log.txt");

        public static string LogFilePath => logFilePath;

        public static void Log(string message)
        {
            Write($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {message}");
        }

        public static void LogFromProvider(string providerMessage)
        {
            Write($"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {providerMessage}");
        }

        public static void LogException(Exception ex, string source, int indexForm = 1)
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

            if (RealTimeDbExceptionLogging)
            {
                SaveExceptionToDatabase(ex, source, indexForm);
            }
        }

        private static void SaveExceptionToDatabase(Exception ex, string source, int indexForm)
        {
            try
            {
                using AirLoggerDbContext db = new AirLoggerDbContext();
                UserException entity = new UserException
                {
                    Message = ex.Message,
                    TargetSite = ex.TargetSite?.ToString() ?? source,
                    dateTimeExc = DateTime.Now,
                    IndexForm = indexForm
                };
                db.UserExceptions.Add(entity);
                db.SaveChanges();
            }
            catch
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(AirLoggerDbContext.ConnectionString);
                    conn.Open();
                    string sql = @"
                        INSERT INTO UserException (Id, Message, TargetSite, dateTimeExc, IndexForm)
                        VALUES ((SELECT ISNULL(MAX(Id), 0) + 1 FROM UserException), @msg, @target, @date, @form)";
                    using SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@msg", ex.Message);
                    cmd.Parameters.AddWithValue("@target", ex.TargetSite?.ToString() ?? source);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@form", indexForm);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
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
            }
        }
    }
}