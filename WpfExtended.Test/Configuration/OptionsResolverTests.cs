using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Windows.Extensions;

namespace WpfExtended.Tests.Configuration
{
    [TestClass]
    public class OptionsResolverTests
    {
        private readonly OptionsResolver optionsResolver = new();
        private readonly Mock<Slim.IServiceProvider> serviceProviderMock = new();
        private readonly Mock<IOptionsManager> optionsManagerMock = new();

        public Mock<IOptionsManager> OptionsManagerMock => optionsManagerMock;

        [TestMethod]
        public void CanResolve_ILiveOptions_ReturnsTrue()
        {
            var type = typeof(IOptions<string>);

            var canResolve = this.optionsResolver.CanResolve(type);

            canResolve.Should().BeTrue();
        }

        [TestMethod]
        public void CanResolve_AnythingElse_ReturnsFalse()
        {
            var types = new Type[] { typeof(object), typeof(string), typeof(OptionsResolverTests), typeof(int) };

            foreach (var type in types)
            {
                var canResolve = this.optionsResolver.CanResolve(type);

                canResolve.Should().BeFalse();
            }
        }

        [TestMethod]
        public void Resolve_IOptions_ReturnsIOptions()
        {
            this.SetupServiceProvider();

            var liveOptions = this.optionsResolver.Resolve(this.serviceProviderMock.Object, typeof(IOptions<string>));

            liveOptions.Should().BeAssignableTo<IOptions<string>>();
        }

        [TestMethod]
        public void Resolve_AnythingElse_Throws()
        {
            this.SetupServiceProvider();

            Action action = new(() =>
            {
                this.optionsResolver.Resolve(this.serviceProviderMock.Object, typeof(string));
            });

            action.Should().Throw<Exception>();
        }

        private void SetupServiceProvider()
        {
            this.serviceProviderMock
                .Setup(u => u.GetService<IOptionsManager>())
                .Returns(this.OptionsManagerMock.Object);
        }
    }
}
