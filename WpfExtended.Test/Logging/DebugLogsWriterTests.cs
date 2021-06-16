using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfExtended.Logging;
using WpfExtended.Models;

namespace WpfExtended.Tests.Logging
{
    [TestClass]
    public class DebugLogsWriterTests
    {
        private readonly DebugLogsWriter logsWriter = new();

        [TestMethod]
        public void WriteLog_Succeeds()
        {
            this.logsWriter.WriteLog(new Log());
        }

        [TestMethod]
        public void WriteNullLog_Throws()
        {
            Action action = new(() =>
            {
                this.logsWriter.WriteLog(null);
            });

            action.Should().Throw<Exception>();
        }
    }
}
