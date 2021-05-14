using Microsoft.Extensions.Logging;
using Slim.Resolvers;
using System.Extensions;
using System.Http;
using System.Linq;
using System.Net.Http;

namespace System.Windows.Extensions.Http
{
    public sealed class HttpClientResolver : IDependencyResolver
    {
        private static readonly Type clientType = typeof(HttpClient<>);
        private static readonly Type loggerType = typeof(ILogger<>);

        /// <summary>
        /// Factory method. <see cref="Type"/> parameter of the factory is the scope of <see cref="IHttpClient{TScope}"/>.
        /// </summary>
        public Func<Slim.IServiceProvider, Type, HttpMessageHandler> HttpMessageHandlerFactory { get; set; }

        /// <summary>
        /// Sets the <see cref="HttpMessageHandlerFactory"/>.
        /// </summary>
        /// <param name="factory">Factory method. <see cref="Type"/> parameter of the factory is the scope of <see cref="IHttpClient{TScope}"/>.</param>
        /// <returns></returns>
        public HttpClientResolver WithHttpMessageHandlerFactory(Func<Slim.IServiceProvider, Type, HttpMessageHandler> factory)
        {
            this.HttpMessageHandlerFactory = factory;
            return this;
        }

        public bool CanResolve(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IHttpClient<>))
            {
                return true;
            }

            return false;
        }

        public object Resolve(Slim.IServiceProvider serviceProvider, Type type)
        {
            var typedClientType = clientType.MakeGenericType(type.GetGenericArguments());
            var typedLoggerType = loggerType.MakeGenericType(type.GetGenericArguments());
            var logger = serviceProvider.GetService(typedLoggerType).As<ILogger>();
            var handler = this.HttpMessageHandlerFactory?.Invoke(serviceProvider, type.GetGenericArguments().First());
            var httpClient = handler is null ?
                Activator.CreateInstance(typedClientType) :
                Activator.CreateInstance(typedClientType, new object[] { handler });
            return httpClient;
        }
    }
}
