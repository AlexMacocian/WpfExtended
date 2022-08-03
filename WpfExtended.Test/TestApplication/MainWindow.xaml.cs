using Microsoft.Extensions.Logging;
using System;
using System.Extensions;
using System.IO;
using System.Net.Http;
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

        private readonly ILogger<MainWindow> scopedLogger;
        private readonly ILogger logger;
        private readonly IHttpClient<MainWindow> httpClient;

        public MainWindow(
            ILogger<MainWindow> scopedLogger,
            ILogger logger,
            IHttpClient<MainWindow> httpClient)
        {
            this.InitializeComponent();
            this.scopedLogger = scopedLogger.ThrowIfNull(nameof(scopedLogger));
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.httpClient = httpClient.ThrowIfNull(nameof(httpClient));
            this.ImageSource = new BitmapImage(new Uri(Path.GetFullPath("Images/Test.jpg")));
            this.logger.LogCritical(new Exception("Some exception message"), "Some exception is logged");
            this.BuildEffectsView();
        }

        private async void BuildEffectsView()
        {
            await this.httpClient.GetAsync("https://google.com");

            foreach(var effectType in TypeCrawler.GetTypes<ShaderEffect>())
            {
                var effect = Activator.CreateInstance(effectType).As<ShaderEffect>();
                this.scopedLogger.LogInformation($"Creating effect {effectType.Name}");
                this.logger.LogInformation($"Creating effect {effectType.Name}");
                var image = new CaptionedImage()
                {
                    Width = 800,
                    Height = 800,
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
