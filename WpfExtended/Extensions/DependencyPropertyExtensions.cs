using System.Windows;

namespace System.Windows.Extensions
{
    public static class DependencyPropertyExtensions
    {
        public static DependencyProperty Register<TOwnerType, TPropertyType>(string name, PropertyMetadata propertyMetadata = null)
        {
            return DependencyProperty.Register(name, typeof(TPropertyType), typeof(TOwnerType), propertyMetadata);
        }
    }
}
