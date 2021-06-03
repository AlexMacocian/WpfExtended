using WpfExtended.Models;

namespace WpfExtended.Logging
{
    public interface ILogsWriter
    {
        void WriteLog(Log log);
    }
}
