using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace WpfExtended.Tests.Controls
{
    /// <summary>
    /// Interaction logic for CaptionedImage.xaml
    /// </summary>
    public partial class CaptionedImage : UserControl
    {
        public readonly static DependencyProperty ImageEffectProperty =
            DependencyPropertyExtensions.Register<CaptionedImage, Effect>(nameof(ImageEffect));

        public readonly static DependencyProperty ImageSourceProperty =
            DependencyPropertyExtensions.Register<CaptionedImage, ImageSource>(nameof(ImageSource));

        public readonly static DependencyProperty CaptionProperty =
            DependencyPropertyExtensions.Register<CaptionedImage, string>(nameof(Caption));

        public Effect ImageEffect
        {
            get => this.GetTypedValue<Effect>(ImageEffectProperty);
            set => this.SetTypedValue(ImageEffectProperty, value);
        }

        public ImageSource ImageSource
        {
            get => this.GetTypedValue<ImageSource>(ImageSourceProperty);
            set => this.SetTypedValue(ImageSourceProperty, value);
        }

        public string Caption
        {
            get => this.GetTypedValue<string>(CaptionProperty);
            set => this.SetTypedValue(CaptionProperty, value);
        }

        public CaptionedImage()
        {
            InitializeComponent();
        }
    }
}
