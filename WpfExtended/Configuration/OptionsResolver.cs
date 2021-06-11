using Microsoft.Extensions.Options;
using Slim.Resolvers;

namespace System.Windows.Extensions
{
    public sealed class OptionsResolver : IDependencyResolver
    {
        private static readonly Type optionsType = typeof(OptionsWrapper<>);
        private static readonly Type configurationType = typeof(IOptionsManager);

        public bool CanResolve(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IOptions<>))
            {
                return true;
            }

            return false;
        }

        public object Resolve(Slim.IServiceProvider serviceProvider, Type type)
        {
            var typedOptionsType = optionsType.MakeGenericType(type.GetGenericArguments());
            var configurationManager = serviceProvider.GetService<IOptionsManager>();
            var optionsValue = configurationType
                .GetMethod(nameof(IOptionsManager.GetOptions))
                .MakeGenericMethod(type.GetGenericArguments())
                .Invoke(configurationManager, Array.Empty<object>());

            return Activator.CreateInstance(typedOptionsType, optionsValue);
        }
    }
}
