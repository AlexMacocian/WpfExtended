using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Extensions;
using WpfExtended.Blazor.Exceptions;

namespace WpfExtended.Blazor.Launch;

public abstract class BlazorHybridApplication<TApp> : ExtendedApplication<BlazorHostWindow>
    where TApp : ComponentBase
{
    public abstract bool DevToolsEnabled { get; }
    public abstract string HostPage { get; }
    public abstract bool ShowTitleBar { get; }

    protected override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton(new BlazorLaunchProperties(typeof(TApp), this.HostPage, this.ShowTitleBar));
        services.AddWpfBlazorWebView();
        if (this.DevToolsEnabled)
        {
            services.AddBlazorWebViewDeveloperTools();
        }
    }

    protected override ValueTask ApplicationStarting()
    {
        var host = this.ServiceProvider.GetRequiredService<BlazorHostWindow>();
        host.BlazorWebViewInitializing += (_, e) => this.Host_BlazorWebViewInitializing(e);
        host.BlazorWebViewInitialized += (_, e) => this.Host_BlazorWebViewInitialized(e);
        host.CoreWebView2InitializationCompleted += (_, e) => this.Host_CoreWebView2InitializationCompleted(e);
        host.CoreWebView2Initialized += (_, e) => this.Host_CoreWebView2Initialized(e);

        return ValueTask.CompletedTask;
    }

    protected virtual void Host_CoreWebView2Initialized(CoreWebView2 e)
    {
        e.ProcessFailed += (_, ex) => this.CoreWebView2_ProcessFailed(ex);
    }

    protected virtual void Host_CoreWebView2InitializationCompleted(CoreWebView2InitializationCompletedEventArgs e)
    {
        if (e.InitializationException is not null)
        {
            this.HandleException(e.InitializationException);
            return;
        }

        if (e.IsSuccess is false)
        {
            this.HandleException(new InvalidOperationException("CoreWebView2 initialization failed."));
            return;
        }


    }

    protected virtual void Host_BlazorWebViewInitialized(BlazorWebViewInitializedEventArgs e)
    {
    }

    protected virtual void Host_BlazorWebViewInitializing(BlazorWebViewInitializingEventArgs e)
    {
    }

    private void CoreWebView2_ProcessFailed(CoreWebView2ProcessFailedEventArgs e)
    {
        var exception = new CoreWebView2Exception(e, "Encountered exception in CoreWebView2");
        if (!this.HandleException(exception))
        {
            throw exception;
        }
    }
}
