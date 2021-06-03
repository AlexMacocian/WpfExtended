using Microsoft.Extensions.Logging;
using Slim;
using System.Extensions;
using System.Net.Http;
using System.Windows.Extensions.Http;
using WpfExtended.Logging;

namespace System.Windows.Extensions
{
    public static class ServiceManagerExtensions
    {
        /// <summary>
        /// Registers a <see cref="ILogsWriter"/> with the default <see cref="CVLoggerProvider"/>.
        /// </summary>
        /// <typeparam name="TLogsWriter">Implementation of <see cref="ILogsWriter"/>.</typeparam>
        /// <param name="serviceManager"><see cref="ServiceManager"/>.</param>
        /// <returns>Provided <see cref="ServiceManager"/>.</returns>
        public static IServiceManager RegisterLogWriter<TLogsWriter>(this IServiceManager serviceManager)
            where TLogsWriter : ILogsWriter
        {
            serviceManager.RegisterSingleton<ILogsWriter, TLogsWriter>();
            serviceManager.RegisterScoped<ILoggerFactory, LoggerFactory>(sp =>
            {
                var factory = new LoggerFactory();
                factory.AddProvider(new CVLoggerProvider(sp.GetService<ILogsWriter>()));
                return factory;
            });

            return serviceManager;
        }

        /// <summary>
        /// Registers a <see cref="ILogsWriter"/> with the default <see cref="CVLoggerProvider"/>.
        /// </summary>
        /// <typeparam name="TILogsWriter">Interface of <see cref="ILogsWriter"/>.</typeparam>
        /// <typeparam name="TLogsWriter">Implementation of <see cref="ILogsWriter"/>.</typeparam>
        /// <param name="serviceManager"><see cref="ServiceManager"/>.</param>
        /// <returns>Provided <see cref="ServiceManager"/>.</returns>
        public static IServiceManager RegisterLogWriter<TILogsWriter, TLogsWriter>(this IServiceManager serviceManager)
            where TLogsWriter : TILogsWriter
            where TILogsWriter : class, ILogsWriter
        {
            serviceManager.RegisterSingleton<TILogsWriter, TLogsWriter>();
            serviceManager.RegisterScoped<ILoggerFactory, LoggerFactory>(sp =>
            {
                var factory = new LoggerFactory();
                factory.AddProvider(new CVLoggerProvider(sp.GetService<TILogsWriter>()));
                return factory;
            });

            return serviceManager;
        }

        public static IServiceManager RegisterLoggerFactory(this IServiceManager serviceManager, Func<Slim.IServiceProvider, ILoggerFactory> loggerFactory)
        {
            serviceManager.RegisterSingleton<ILoggerFactory, ILoggerFactory>(loggerFactory);
            return serviceManager;
        }

        public static IServiceManager RegisterDebugLoggerFactory(this IServiceManager serviceManager)
        {
            serviceManager.RegisterLogWriter<ILogsWriter, DebugLogsWriter>();
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
