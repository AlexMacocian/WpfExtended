using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Extensions;

namespace WpfExtended.Tests.Configuration
{
    [TestClass]
    public class UpdateableOptionsResolverTests
    {
        private readonly UpdateableOptionsResolver updateableOptionsResolver = new();
        private readonly Mock<Slim.IServiceProvider> serviceProviderMock = new();
        private readonly Mock<IOptionsManager> optionsManagerMock = new();

        [TestMethod]
        public void CanResolve_ILiveOptions_ReturnsTrue()
        {
            var type = typeof(IUpdateableOptions<string>);

            var canResolve = this.updateableOptionsResolver.CanResolve(type);

            canResolve.Should().BeTrue();
        }

        [TestMethod]
        public void CanResolve_AnythingElse_ReturnsFalse()
        {
            var types = new Type[] { typeof(object), typeof(string), typeof(UpdateableOptionsResolverTests), typeof(int) };

            foreach (var type in types)
            {
                var canResolve = this.updateableOptionsResolver.CanResolve(type);

                canResolve.Should().BeFalse();
            }
        }

        [TestMethod]
        public void Resolve_IUpdateableOptions_ReturnsIUpdateableOptions()
        {
            this.SetupServiceProvider();

            var liveOptions = this.updateableOptionsResolver.Resolve(this.serviceProviderMock.Object, typeof(IUpdateableOptions<string>));

            liveOptions.Should().BeAssignableTo<IUpdateableOptions<string>>();
        }

        [TestMethod]
        public void Resolve_AnythingElse_Throws()
        {
            this.SetupServiceProvider();

            Action action = new(() =>
            {
                this.updateableOptionsResolver.Resolve(this.serviceProviderMock.Object, typeof(string));
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
