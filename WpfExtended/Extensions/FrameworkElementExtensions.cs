using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace System.Windows.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static BitmapSource RenderBitmap(this FrameworkElement frameworkElement)
        {
            frameworkElement = frameworkElement ?? throw new ArgumentNullException(nameof(frameworkElement));

            var source = PresentationSource.FromVisual(frameworkElement);
            var dpix = 96 * source.CompositionTarget.TransformToDevice.M11;
            var dpiy = 96 * source.CompositionTarget.TransformToDevice.M22;
            var renderTargetBitmap = new RenderTargetBitmap((int)frameworkElement.ActualWidth, (int)frameworkElement.ActualHeight, dpix, dpiy, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(frameworkElement);
            return renderTargetBitmap;
        }

        public static void RemoveParent(this FrameworkElement frameworkElement)
        {
            frameworkElement = frameworkElement ?? throw new ArgumentNullException(nameof(frameworkElement));

            if (frameworkElement.Parent is null) return;

            switch (frameworkElement.Parent)
            {
                case Panel panel:
                    panel.Children.Remove(frameworkElement);
                    break;
                case Decorator decorator:
                    decorator.Child = null;
                    break;
                case ContentPresenter presenter:
                    presenter.Content = null;
                    break;
                case ContentControl contentControl:
                    contentControl.Content = null;
                    break;
                default:
                    throw new InvalidOperationException($"Could not determine how to remove parent for parent type: {frameworkElement.Parent.GetType().Name}");
            }
        }
    }
}
