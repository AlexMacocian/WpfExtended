using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Windows.Extensions;

namespace WpfExtended.Tests.Configuration
{
    [TestClass]
    public class LiveUpdateableOptionsResolverTests
    {
        private readonly LiveUpdateableOptionsResolver liveUpdateableOptionsResolver = new();
        private readonly Mock<Slim.IServiceProvider> serviceProviderMock = new();
        private readonly Mock<IOptionsManager> optionsManagerMock = new();

        [TestMethod]
        public void CanResolve_ILiveUpdateableOptions_ReturnsTrue()
        {
            var type = typeof(ILiveUpdateableOptions<string>);

            var canResolve = this.liveUpdateableOptionsResolver.CanResolve(type);

            canResolve.Should().BeTrue();
        }

        [TestMethod]
        public void CanResolve_AnythingElse_ReturnsFalse()
        {
            var types = new Type[] { typeof(object), typeof(string), typeof(LiveOptionsResolverTests), typeof(int) };

            foreach (var type in types)
            {
                var canResolve = this.liveUpdateableOptionsResolver.CanResolve(type);

                canResolve.Should().BeFalse();
            }
        }

        [TestMethod]
        public void Resolve_ILiveUpdateableOptions_ReturnsILiveUpdateableOptions()
        {
            this.SetupServiceProvider();

            var liveOptions = this.liveUpdateableOptionsResolver.Resolve(this.serviceProviderMock.Object, typeof(ILiveUpdateableOptions<string>));

            liveOptions.Should().BeAssignableTo<ILiveUpdateableOptions<string>>();
        }

        [TestMethod]
        public void Resolve_AnythingElse_Throws()
        {
            this.SetupServiceProvider();

            Action action = new(() =>
            {
                this.liveUpdateableOptionsResolver.Resolve(this.serviceProviderMock.Object, typeof(string));
            });

            action.Should().Throw<Exception>();
        }

        private void SetupServiceProvider()
        {
            this.serviceProviderMock
                .Setup(u => u.GetService<IOptionsManager>())
                .Returns(this.optionsManagerMock.Object);
        }
    }
}
