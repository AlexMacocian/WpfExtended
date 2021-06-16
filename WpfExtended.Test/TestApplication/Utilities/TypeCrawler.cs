using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfExtended.Tests.Utilities
{
    internal static class TypeCrawler
    {
        public static IEnumerable<Type> GetTypes<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(T)))
                    .Where(t => t.IsAbstract is false));
        }
    }
}
