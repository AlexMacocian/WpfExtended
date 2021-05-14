using Microsoft.Extensions.Logging;
using Slim.Resolvers;
using System.Extensions;
using System.Http;
using System.Linq;

namespace System.Windows.Extensions.Http
{
    public sealed class HttpClientResolver : IDependencyResolver
    {
        private static readonly Type clientType = typeof(HttpClient<>);
        private static readonly Type loggerType = typeof(ILogger<>);

        public bool LogEvents { get; set; }

        public HttpClientResolver WithLogEvents(bool logEvents)
        {
            this.LogEvents = logEvents;
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
            var httpClient = Activator.CreateInstance(typedClientType);
            if (this.LogEvents is false)
            {
                return httpClient;
            }

            var typedLoggerType = loggerType.MakeGenericType(type.GetGenericArguments());
            var logger = serviceProvider.GetService(typedLoggerType).As<ILogger>();
            var eventInfo = httpClient.GetType().GetEvents().Where(ev => ev.Name == nameof(HttpClient<object>.EventEmitted)).First();
            var handler = new EventHandler<HttpClientEventMessage>((sender, message) => LogHttpMessage(sender, message, logger));
            eventInfo.AddMethod.Invoke(httpClient, new object[] { handler });
            return httpClient;
        }

        private static void LogHttpMessage(object sender, HttpClientEventMessage httpClientEventMessage, ILogger logger)
        {
            logger.LogInformation($"{httpClientEventMessage.Method} - {httpClientEventMessage.Url}");
        }
    }
}
