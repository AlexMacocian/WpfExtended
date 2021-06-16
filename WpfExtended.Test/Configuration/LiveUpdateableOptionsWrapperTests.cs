using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Extensions;

namespace WpfExtended.Tests.Configuration
{
    [TestClass]
    public class LiveUpdateableOptionsWrapperTests
    {
        private LiveUpdateableOptionsWrapper<string> optionsWrapper;
        private readonly Mock<IOptionsManager> optionsManagerMock = new();

        [TestInitialize]
        public void TestInitialize()
        {
            this.optionsWrapper = new LiveUpdateableOptionsWrapper<string>(optionsManagerMock.Object);
        }

        [TestMethod]
        public void GetValue_ReturnsValue()
        {
            this.optionsManagerMock
                .Setup(u => u.GetOptions<string>())
                .Returns("hello");

            var value = this.optionsWrapper.Value;

            value.Should().Be("hello");
        }

        [TestMethod]
        public void UpdateOption_CallsOptionsManager()
        {
            this.optionsManagerMock
                .Setup(u => u.UpdateOptions<string>(It.IsAny<string>()))
                .Verifiable();

            this.optionsWrapper.UpdateOption();

            this.optionsManagerMock.Verify();
        }
    }
}
