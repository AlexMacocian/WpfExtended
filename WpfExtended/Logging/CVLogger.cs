using Microsoft.Extensions.Logging;
using System;
using System.Extensions;
using WpfExtended.Models;

namespace WpfExtended.Logging
{
    public sealed class CVLogger : ILogger
    {
        private readonly string category;
        private readonly CVLoggerProvider cvLoggerProvider;

        public CVLogger(string category, CVLoggerProvider debugLoggerProvider)
        {
            this.category = category;
            this.cvLoggerProvider = debugLoggerProvider.ThrowIfNull(nameof(debugLoggerProvider));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            var log = new Log
            {
                Exception = exception,
                LogLevel = logLevel,
                EventId = eventId.Name,
                Message = message,
                Category = category,
                LogTime = DateTime.Now
            };

            this.cvLoggerProvider.LogEntry(log);
        }
    }
}
