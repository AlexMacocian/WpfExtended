using Microsoft.CorrelationVector;
using Microsoft.Extensions.Logging;
using System.Extensions;
using WpfExtended.Models;

namespace WpfExtended.Logging
{
    public sealed class CVLoggerProvider : ILoggerProvider
    {
        private readonly ILogsWriter logsManager;
        private CorrelationVector correlationVector;

        public CVLoggerProvider(ILogsWriter logsWriter)
        {
            this.logsManager = logsWriter.ThrowIfNull(nameof(logsWriter));
            this.correlationVector = new CorrelationVector();
        }

        public void LogEntry(Log log)
        {
            if (this.correlationVector is not null)
            {
                log.CorrelationVector = this.correlationVector.Value.ToString();
                this.correlationVector.Increment();
            }

            this.logsManager.WriteLog(log);
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (this.correlationVector is not null)
            {
                this.correlationVector = CorrelationVector.Extend(this.correlationVector.ToString());
            }

            return new CVLogger(categoryName, this);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
