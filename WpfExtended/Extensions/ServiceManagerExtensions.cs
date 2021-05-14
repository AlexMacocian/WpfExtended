using Slim;
using System.Extensions;
using System.Windows.Extensions.Http;

namespace System.Windows.Extensions
{
    public static class ServiceManagerExtensions
    {
        public static IServiceProducer RegisterHttpFactory(this IServiceManager serviceManager)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterResolver(
                new HttpClientResolver()
                    .WithLogEvents(true));
            return serviceManager;
        }
    }
}
