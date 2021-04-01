using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Effects;

namespace WpfExtended.Tests.Utilities
{
    internal static class TypeCrawler
    {
        public static IEnumerable<Type> GetEffectTypes<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(T))));
        }
    }
}
