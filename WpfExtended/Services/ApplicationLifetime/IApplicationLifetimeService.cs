namespace System.Windows.Extensions.Services
{
    public interface IApplicationLifetimeService
    {
        void OnStartup();
        void OnClosing();
    }
}
