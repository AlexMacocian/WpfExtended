using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Extensions;

namespace WpfExtended.Tests.Configuration
{
    [TestClass]
    public class DefaultOptionsManagerTests
    {
        private readonly DefaultOptionsManager optionsManager = new();

        [TestMethod]
        public void GetOptions_ReturnsDefault()
        {
            var options = this.optionsManager.GetOptions<string>();

            options.Should().BeNull();
        }

        [TestMethod]
        public void UpdateOptions_Succeeds()
        {
            this.optionsManager.UpdateOptions(string.Empty);
        }
    }
}
