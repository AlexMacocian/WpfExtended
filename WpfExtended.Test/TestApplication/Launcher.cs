using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Slim;
using Slim.Integration.ServiceCollection;
using System;
using System.Extensions;
using System.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Extensions;
using WpfExtended.Test;
using WpfExtended.Tests.Services;

namespace WpfExtended.Tests
{
    public class Launcher : ExtendedApplication<MainWindow>
    {
        private static Launcher Instance { get; } = new Launcher();

        [STAThread]
        public static void Main()
        {
            Instance.Run();
        }

        protected override System.IServiceProvider SetupServiceProvider(IServiceCollection services)
        {
            var serviceManager = new ServiceManager();
            serviceManager.RegisterResolver(new LoggerResolver());
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

            var provider = services.BuildSlimServiceProvider(serviceManager);
            return provider;
        }

        protected override void ApplicationClosing()
        {
        }

        protected override ValueTask ApplicationStarting() => ValueTask.CompletedTask;

        protected override bool HandleException(Exception e)
        {
            return false;
        }

        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IDummyService, DummyService>();
        }
    }
}
