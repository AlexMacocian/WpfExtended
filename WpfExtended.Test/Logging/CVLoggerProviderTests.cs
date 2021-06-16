using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WpfExtended.Logging;
using WpfExtended.Models;

namespace WpfExtended.Tests.Logging
{
    [TestClass]
    public class CVLoggerProviderTests
    {
        private readonly Mock<ILogsWriter> logsWriterMock = new();
        private CVLoggerProvider cVLoggerProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            this.cVLoggerProvider = new CVLoggerProvider(this.logsWriterMock.Object);
        }

        [TestMethod]
        public void CreateLogger_CreatesNewLogger()
        {
            var logger = this.cVLoggerProvider.CreateLogger(string.Empty);

            logger.Should().NotBeNull();
        }

        [TestMethod]
        public void LogEntry_CallsLogWriter()
        {
            this.cVLoggerProvider.LogEntry(new Log());

            this.logsWriterMock.Verify();
        }

        private void SetupLogsWriter()
        {
            this.logsWriterMock
                .Setup(u => u.WriteLog(It.IsAny<Log>()))
                .Verifiable();
        }
    }
}
