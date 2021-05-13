using Microsoft.Extensions.Logging;
using Slim.Resolvers;
using System.Linq;

namespace System.Windows.Extensions.Logging
{
    public sealed class LoggerResolver : IDependencyResolver
    {
        public bool CanResolve(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ILogger<>) ||
                type == typeof(ILogger))
            {
                return true;
            }

            return false;
        }

        public object Resolve(Slim.IServiceProvider serviceProvider, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ILogger<>))
            {
                return ResolveScopedLogger(serviceProvider, type);
            }
            else
            {
                return ResolveLogger(serviceProvider, type);
            }
        }

        private static object ResolveScopedLogger(Slim.IServiceProvider serviceProvider, Type type)
        {
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var categoryTypes = type.GetGenericArguments();
            var createLoggerMethod = typeof(LoggerFactoryExtensions).GetMethods().Where(m => m.IsGenericMethodDefinition && m.Name == nameof(LoggerFactoryExtensions.CreateLogger)).First();
            return createLoggerMethod.MakeGenericMethod(categoryTypes.First()).Invoke(null, new object[] { loggerFactory });
        }

        private static object ResolveLogger(Slim.IServiceProvider serviceProvider, Type type)
        {
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            return loggerFactory.CreateLogger(string.Empty);
        }
    }
}
