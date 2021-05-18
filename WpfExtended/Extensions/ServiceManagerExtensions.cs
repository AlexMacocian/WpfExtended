using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Slim;
using System.Extensions;
using System.Net.Http;
using System.Windows.Extensions.Http;

namespace System.Windows.Extensions
{
    public static class ServiceManagerExtensions
    {
        public static IServiceManager RegisterLoggerFactory(this IServiceManager serviceManager, Func<Slim.IServiceProvider, ILoggerFactory> loggerFactory)
        {
            serviceManager.RegisterSingleton<ILoggerFactory, ILoggerFactory>(loggerFactory);
            return serviceManager;
        }

        public static IServiceManager RegisterLoggerFactory(this IServiceManager serviceManager)
        {
            var factory = new LoggerFactory();
            factory.AddProvider(new DebugLoggerProvider());
            serviceManager.RegisterSingleton<ILoggerFactory, ILoggerFactory>(_ => factory);
            return serviceManager;
        }

        public static IServiceManager RegisterHttpFactory(this IServiceManager serviceManager)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterResolver(new HttpClientResolver());
            return serviceManager;
        }

        public static IServiceManager RegisterHttpFactory(this IServiceManager serviceManager, Func<Slim.IServiceProvider, Type, HttpMessageHandler> handlerFactory)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterResolver(
                new HttpClientResolver()
                .WithHttpMessageHandlerFactory(handlerFactory));
            return serviceManager;
        }
    }
}
