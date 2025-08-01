using System;
using System.Windows;

namespace WpfExtended.Blazor.Launch;


/// <summary>
/// Interaction logic for BlazorHostWindow.xaml
/// </summary>
public partial class BlazorHostWindow : Window
{
    public static readonly DependencyProperty ServicesProperty = DependencyProperty.Register(
        nameof(Services), typeof(IServiceProvider), typeof(BlazorHostWindow));

    public static readonly DependencyProperty ShowTitleBarProperty = DependencyProperty.Register(
        nameof(ShowTitleBar), typeof(bool), typeof(BlazorHostWindow));

    public static readonly DependencyProperty HostPageProperty = DependencyProperty.Register(
        nameof(HostPage), typeof(string), typeof(BlazorHostWindow));

    public static readonly DependencyProperty AppTypeProperty = DependencyProperty.Register(
        nameof(AppType), typeof(Type), typeof(BlazorHostWindow));

    public IServiceProvider Services
    {
        get => (IServiceProvider)this.GetValue(ServicesProperty);
        set => this.SetValue(ServicesProperty, value);
    }

    public bool ShowTitleBar
    {
        get => (bool)this.GetValue(ShowTitleBarProperty);
        set
        {
            this.SetValue(ShowTitleBarProperty, value);
            this.WindowStyle = value ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            this.ResizeMode = value ? ResizeMode.CanResize : ResizeMode.NoResize;
        }
    }

    public string HostPage
    {
        get => (string)this.GetValue(HostPageProperty);
        set => this.SetValue(HostPageProperty, value);
    }

    public Type AppType
    {
        get => (Type)this.GetValue(AppTypeProperty);
        set
        {
            this.SetValue(AppTypeProperty, value);
            this.RootComponent.ComponentType = value;
        }
    }

    public BlazorHostWindow(
        BlazorLaunchProperties blazorLaunchProperties,
        IServiceProvider serviceProvider)
    {
        this.InitializeComponent();
        this.Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.ShowTitleBar = blazorLaunchProperties.ShowTitleBar;
        this.HostPage = blazorLaunchProperties.HostPage;
        this.AppType = blazorLaunchProperties.AppType;
    }
}
