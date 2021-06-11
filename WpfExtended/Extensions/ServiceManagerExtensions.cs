using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        /// Registers a <see cref="IOptionsManager"/> with the default <see cref="DefaultOptionsManager"/>.
        /// This call also registers the resolver that resolves <see cref="IUpdateableOptions{T}"/> and <see cref="IOptions{T}"/>.
        /// </summary>
        /// <param name="serviceManager"><see cref="IServiceManager"/>.</param>
        /// <returns>Provided <see cref="IServiceManager"/>.</returns>
        public static IServiceManager RegisterOptionsManager(this IServiceManager serviceManager)
        {
            serviceManager.RegisterSingleton<IOptionsManager, DefaultOptionsManager>();
            serviceManager.RegisterOptionsResolver();

            return serviceManager;
        }

        /// <summary>
        /// Registers a <see cref="IOptionsManager"/>.
        /// This call also registers the resolver that resolves <see cref="IUpdateableOptions{T}"/> and <see cref="IOptions{T}"/>.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="IOptionsManager"/>.</typeparam>
        /// <param name="serviceManager"><see cref="IServiceManager"/>.</param>
        /// <returns>Provided <see cref="IServiceManager"/>.</returns>
        public static IServiceManager RegisterOptionsManager<T>(this IServiceManager serviceManager)
            where T : IOptionsManager
        {
            serviceManager.RegisterSingleton<IOptionsManager, T>();
            serviceManager.RegisterOptionsResolver();

            return serviceManager;
        }

        /// <summary>
        /// Registers resolvers for <see cref="IOptions{TOptions}"/> and <see cref="IUpdateableOptions{T}"/>.
        /// Depends on a <see cref="IOptionsManager"/> in order to properly resolve options.
        /// </summary>
        /// <param name="serviceManager"><see cref="IServiceManager"/>.</param>
        /// <returns><see cref="IServiceManager"/>.</returns>
        public static IServiceManager RegisterOptionsResolver(this IServiceManager serviceManager)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterResolver(new OptionsResolver());
            serviceManager.RegisterResolver(new UpdateableOptionsResolver());
            serviceManager.RegisterResolver(new LiveOptionsResolver());
            serviceManager.RegisterResolver(new LiveUpdateableOptionsResolver());

            return serviceManager;
        }

        /// <summary>
        /// Registers a <see cref="ILogsWriter"/> with the default <see cref="CVLoggerProvider"/>.
        /// </summary>
        /// <typeparam name="TLogsWriter">Implementation of <see cref="ILogsWriter"/>.</typeparam>
        /// <param name="serviceManager"><see cref="IServiceProducer"/>.</param>
        /// <returns>Provided <see cref="IServiceProducer"/>.</returns>
        public static IServiceProducer RegisterLogWriter<TLogsWriter>(this IServiceProducer serviceManager)
            where TLogsWriter : ILogsWriter
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

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
        /// <param name="serviceManager"><see cref="IServiceProducer"/>.</param>
        /// <returns>Provided <see cref="IServiceProducer"/>.</returns>
        public static IServiceProducer RegisterLogWriter<TILogsWriter, TLogsWriter>(this IServiceProducer serviceManager)
            where TLogsWriter : TILogsWriter
            where TILogsWriter : class, ILogsWriter
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

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
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterSingleton<ILoggerFactory, ILoggerFactory>(loggerFactory);
            return serviceManager;
        }

        public static IServiceManager RegisterDebugLoggerFactory(this IServiceManager serviceManager)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

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
