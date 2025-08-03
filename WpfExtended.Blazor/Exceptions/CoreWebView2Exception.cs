using Microsoft.Web.WebView2.Core;
using System;

namespace WpfExtended.Blazor.Exceptions;

public sealed class CoreWebView2Exception(CoreWebView2ProcessFailedEventArgs args, string message, Exception? innerException = default) : Exception(message, innerException)
{
    public CoreWebView2ProcessFailedEventArgs Args { get; } = args ?? throw new ArgumentNullException(nameof(args));
}
