using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace WinFormsApp1
{
    public class AirLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose() { }

        private class MyLogger : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                string message = formatter(state, exception);
                if (string.IsNullOrWhiteSpace(message))
                    return;

                File.AppendAllText("log.txt", message + Environment.NewLine);
                Console.WriteLine(message);

                AppLogger.LogFromProvider($"[EF Core {logLevel}] {message}");
                SaveLogToDatabase(logLevel.ToString(), message);
            }

            private void SaveLogToDatabase(string level, string logMessage)
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(AirLoggerDbContext.ConnectionString);
                    conn.Open();

                    string shortMsg = logMessage.Length > 250 ? logMessage.Substring(0, 250) : logMessage;
                    string target = $"AirLoggerProvider.{level}";
                    string sql = @"
    DECLARE @newExcId INT;

    IF COLUMNPROPERTY(OBJECT_ID('UserException'), 'Id', 'IsIdentity') = 1
    BEGIN
        INSERT INTO UserException (Message, TargetSite, dateTimeExc, IndexForm)
        VALUES (@msg, @target, @date, @form);
        SET @newExcId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        SELECT @newExcId = ISNULL(MAX(Id), 0) + 1 FROM UserException;
        INSERT INTO UserException (Id, Message, TargetSite, dateTimeExc, IndexForm)
        VALUES (@newExcId, @msg, @target, @date, @form);
    END

    IF EXISTS (SELECT 1 FROM Professor) AND EXISTS (SELECT 1 FROM Task)
       AND EXISTS (SELECT 1 FROM Student) AND EXISTS (SELECT 1 FROM Project)
       AND EXISTS (SELECT 1 FROM Incident)
    BEGIN
        INSERT INTO MetaData (Id_Professor, Id_Task, Id_Student, Id_Project, Id, Id_Incident, hasTextLog)
        VALUES (
            (SELECT TOP 1 Id_Professor FROM Professor),
            (SELECT TOP 1 Id_Task FROM Task),
            (SELECT TOP 1 Id_Student FROM Student),
            (SELECT TOP 1 Id_Project FROM Project),
            @newExcId,
            (SELECT TOP 1 Id_Incident FROM Incident),
            1
        );
    END";

                    using SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@msg", shortMsg);
                    cmd.Parameters.AddWithValue("@target", target);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@form", 1);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
        }
    }
}