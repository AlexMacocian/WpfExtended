using System.Diagnostics;
using WpfExtended.Models;

namespace WpfExtended.Logging
{
    public sealed class DebugLogsWriter : ILogsWriter
    {
        public void WriteLog(Log log)
        {
            Debug.WriteLine($"{log.LogTime} - {log.Category} - {log.EventId} - {log.CorrelationVector} - {log.LogLevel} - {log.Message}");
        }
    }
}
