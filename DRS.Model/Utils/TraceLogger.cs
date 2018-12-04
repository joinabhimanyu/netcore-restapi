using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DRS.Model.Utils
{
    public class TraceLogger : ILogger
    {
        private readonly string categoryName;

        public TraceLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            Trace.WriteLine($"{DateTime.Now.ToString("o")} {logLevel} {eventId.Id} {this.categoryName}");
            Trace.WriteLine(formatter(state, exception));

            if (File.Exists(@"C:\temp\log.txt"))
            {
                File.Delete(@"C:\temp\log.txt");
            }

            File.AppendAllText(@"C:\temp\log.txt", formatter(state, exception));
            Console.WriteLine(formatter(state, exception));
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
