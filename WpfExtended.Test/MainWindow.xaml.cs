using System.Windows;
using System.Windows.Media;

namespace WpfExtended.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(MainWindow));
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
