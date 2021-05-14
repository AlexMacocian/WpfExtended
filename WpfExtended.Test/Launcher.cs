using Microsoft.Extensions.Logging;
using Slim;
using System;
using System.Extensions;
using System.Http;
using System.Net.Http;
using System.Windows.Extensions;
using System.Windows.Extensions.Http;
using WpfExtended.Test;
using WpfExtended.Tests.Http;

namespace WpfExtended.Tests
{
    public class Launcher : ExtendedApplication<MainWindow>
    {
        private static Launcher Instance { get; } = new Launcher();

        [STAThread]
        public static int Main()
        {
            return Instance.Run();
        }

        protected override void SetupServiceManager(IServiceManager serviceManager)
        {
            serviceManager.RegisterResolver(
                new HttpClientResolver()
                .WithHttpMessageHandlerFactory((sp, category) =>
                {
                    var loggerType = typeof(ILogger<>).MakeGenericType(category);
                    var logger = sp.GetService(loggerType).As<ILogger>();
                    return new HttpMessageLogger(logger, new HttpClientHandler());
                }));
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
        }
    }
}
