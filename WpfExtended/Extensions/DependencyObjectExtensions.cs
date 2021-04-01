using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Extensions
{
    public static class DependencyObjectExtensions
    {
        public static T GetTypedValue<T>(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            dependencyObject = dependencyObject ?? throw new ArgumentNullException(nameof(dependencyObject));

            return (T)dependencyObject.GetValue(dependencyProperty);
        }
        public static void SetTypedValue<T>(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, T value)
        {
            dependencyObject = dependencyObject ?? throw new ArgumentNullException(nameof(dependencyObject));

            dependencyObject.SetValue(dependencyProperty, value);
        }
        public static List<T> FindVisualChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            obj = obj ?? throw new ArgumentNullException(nameof(obj));

            List<T> children = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var o = VisualTreeHelper.GetChild(obj, i);
                if (o != null)
                {
                    if (o is T t)
                        children.Add(t);

                    children.AddRange(FindVisualChildren<T>(o)); // recursive
                }
            }
            return children;
        }
        public static T FindUpVisualTree<T>(this DependencyObject initial) where T : DependencyObject
        {
            initial = initial ?? throw new ArgumentNullException(nameof(initial));

            DependencyObject current = initial;

            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }
    }
}
