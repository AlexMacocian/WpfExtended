using System;

namespace WpfExtended.Blazor.Launch;

public sealed record BlazorLaunchProperties(Type AppType, string HostPage, bool ShowTitleBar)
{
}
