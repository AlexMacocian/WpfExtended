using System;
using System.Extensions;
using System.IO;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using WpfExtended.Tests.Controls;
using WpfExtended.Tests.Utilities;

namespace WpfExtended.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly static DependencyProperty ImageSourceProperty =
            DependencyPropertyExtensions.Register<MainWindow, ImageSource>(nameof(ImageSource));

        public ImageSource ImageSource
        {
            get => this.GetTypedValue<ImageSource>(ImageSourceProperty);
            set => this.SetTypedValue(ImageSourceProperty, value);
        }

        public MainWindow()
        {
            InitializeComponent();
            this.ImageSource = new BitmapImage(new Uri(Path.GetFullPath("Images/Test.jpg")));
            this.BuildEffectsView();
        }

        private void BuildEffectsView()
        {
            foreach(var effectType in TypeCrawler.GetTypes<ShaderEffect>())
            {
                var effect = Activator.CreateInstance(effectType).As<ShaderEffect>();
                var image = new CaptionedImage()
                {
                    Width = 300,
                    Height = 300,
                    ImageSource = this.ImageSource,
                    Caption = effect.GetType().Name,
                    ImageEffect = effect,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1)
                };
                this.WrapPanel.Children.Add(image);
            }
        }
    }
}
