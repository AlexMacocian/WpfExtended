using Slim;
using System.Extensions;
using System.Net.Http;
using System.Windows.Extensions.Http;

namespace System.Windows.Extensions
{
    public static class ServiceManagerExtensions
    {
        public static IServiceProducer RegisterHttpFactory(this IServiceManager serviceManager)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterResolver(new HttpClientResolver());
            return serviceManager;
        }

        public static IServiceProducer RegisterHttpFactory(this IServiceManager serviceManager, Func<Slim.IServiceProvider, Type, HttpMessageHandler> handlerFactory)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterResolver(
                new HttpClientResolver()
                .WithHttpMessageHandlerFactory(handlerFactory));
            return serviceManager;
        }
    }
}
