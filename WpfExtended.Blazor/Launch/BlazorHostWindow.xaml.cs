using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Web.WebView2.Core;
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

    public event EventHandler<BlazorWebViewInitializedEventArgs>? BlazorWebViewInitialized;

    public event EventHandler<BlazorWebViewInitializingEventArgs>? BlazorWebViewInitializing;

    public event EventHandler<CoreWebView2InitializationCompletedEventArgs>? CoreWebView2InitializationCompleted;

    public event EventHandler<CoreWebView2>? CoreWebView2Initialized;

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
            this.WindowChrome.CaptionHeight = value ? SystemParameters.CaptionHeight : 0;
            this.BlazorWebView.Margin = value ? new Thickness(0, SystemParameters.CaptionHeight, 0, 0) : new Thickness(0);
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
        this.BlazorWebView.BlazorWebViewInitialized += this.BlazorWebView_BlazorWebViewInitialized;
        this.BlazorWebView.BlazorWebViewInitializing += this.BlazorWebView_BlazorWebViewInitializing;
    }

    protected override void OnStateChanged(EventArgs e)
    {
        base.OnStateChanged(e);
        var cornerRadius = Math.Max(
            Math.Max(SystemParameters.WindowCornerRadius.TopLeft, SystemParameters.WindowCornerRadius.TopRight),
            Math.Max(SystemParameters.WindowCornerRadius.BottomLeft, SystemParameters.WindowCornerRadius.BottomRight));
        this.WindowChrome.ResizeBorderThickness = this.WindowState is WindowState.Maximized ? new Thickness(0) : new Thickness(cornerRadius);
        this.WindowChrome.CornerRadius = this.WindowState is WindowState.Maximized ? new CornerRadius(0) : SystemParameters.WindowCornerRadius;
        this.BlazorWebView.Margin = this.WindowState is WindowState.Maximized
            ? this.ShowTitleBar 
                ? new Thickness(cornerRadius, SystemParameters.CaptionHeight + cornerRadius, cornerRadius, cornerRadius)
                : new Thickness(cornerRadius)
            : new Thickness(0, this.ShowTitleBar ? SystemParameters.CaptionHeight : 0, 0, 0);
    }

    private void BlazorWebView_BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs args)
    {
        this.BlazorWebView.WebView.CoreWebView2InitializationCompleted += this.CoreWebView2_InitializationCompleted;
        this.BlazorWebViewInitialized?.Invoke(this, args);
    }

    private void BlazorWebView_BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs args)
    {
        this.BlazorWebViewInitializing?.Invoke(this, args);
    }

    private void CoreWebView2_InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        this.CoreWebView2InitializationCompleted?.Invoke(this, e);

        if (!e.IsSuccess)
        {
            return;
        }

        this.CoreWebView2Initialized?.Invoke(this, this.BlazorWebView.WebView.CoreWebView2);
    }
}
