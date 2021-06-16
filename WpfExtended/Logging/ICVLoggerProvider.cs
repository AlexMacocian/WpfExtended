using Microsoft.Extensions.Logging;
using WpfExtended.Models;

namespace WpfExtended.Logging
{
    public interface ICVLoggerProvider : ILoggerProvider
    {
        void LogEntry(Log log);
    }
}
