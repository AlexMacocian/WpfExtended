using Slim.Resolvers;
using System.Http;

namespace System.Windows.Extensions.Http
{
    public sealed class HttpClientResolver : IDependencyResolver
    {
        private readonly Type clientType = typeof(HttpClient<>);

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
            return Activator.CreateInstance(typedClientType);
        }
    }
}
