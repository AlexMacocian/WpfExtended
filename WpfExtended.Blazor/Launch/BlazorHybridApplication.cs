using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Extensions;

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
}
