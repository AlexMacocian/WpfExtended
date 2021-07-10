using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Slim;
using System;
using System.Extensions;
using System.Http;
using System.Net.Http;
using System.Windows.Extensions;
using WpfExtended.Test;
using WpfExtended.Tests.Services;

namespace WpfExtended.Tests
{
    public class Launcher : ExtendedApplication<MainWindow>
    {
        private static Launcher Instance { get; } = new Launcher();

        protected override void SetupServiceManager(IServiceManager serviceManager)
        {
            serviceManager.RegisterDebugLoggerFactory();
            serviceManager.RegisterResolver(
                new HttpClientResolver()
                .WithHttpMessageHandlerFactory((serviceProvider, categoryType) =>
                {
                    var loggerType = typeof(ILogger<>).MakeGenericType(categoryType);
                    var logger = serviceProvider.GetService(loggerType).As<ILogger>();
                    var handler = new LoggingHttpMessageHandler(logger) { InnerHandler = new HttpClientHandler() };
                    return handler;
                }));
            serviceManager.RegisterOptionsManager();
        }

        protected override void ApplicationClosing()
        {
        }

        protected override void ApplicationStarting()
        {
        }

        protected override bool HandleException(Exception e)
        {
            return false;
        }

        protected override void RegisterServices(IServiceProducer serviceProducer)
        {
            serviceProducer.RegisterSingleton<IDummyService, DummyService>();
        }
    }
}
