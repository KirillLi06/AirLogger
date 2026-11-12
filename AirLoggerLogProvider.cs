using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;
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
        public IDisposable BeginScope<Tstate> (Tstate state)
        {
            return null;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
        public void Log<Tstate> (LogLevel logLevel, EventId eventId, Tstate state, Exception exception, Func<Tstate, Exception, string> formatter) 
        {
            File.AppendAllText("log.txt", formatter(state, exception));
                Console.WriteLine(formatter(state, exception));
        }
        }
    }
}
