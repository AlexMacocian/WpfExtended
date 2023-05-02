using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slim;
using Slim.Integration.ServiceCollection;
using System.Extensions;
using System.Logging;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace System.Windows.Extensions
{
    /// <summary>
    /// Extended application class.
    /// </summary>
    /// <typeparam name="T">Type of window to be launched after initialization.</typeparam>
    public abstract class ExtendedApplication<T> : Application
        where T : Window
    {
        private readonly ServiceCollection services = new();

        protected IServiceProvider ServiceProvider { get; private set; }

        public ExtendedApplication()
        {
        }

        /// <summary>
        /// Create a new <see cref="IServiceProvider"/>.
        /// </summary>
        protected virtual IServiceProvider SetupServiceProvider(IServiceCollection services)
        {
            var serviceProvider = services.BuildSlimServiceProvider();
            if (serviceProvider is IServiceManager serviceManager)
            {
                serviceManager.RegisterDebugLoggerFactory();
                serviceManager.RegisterHttpFactory();
                serviceManager.RegisterResolver(new LoggerResolver());
            }

            return serviceProvider;
        }

        /// <summary>
        /// Called right before the window is shown.
        /// </summary>
        protected abstract void ApplicationStarting();
        /// <summary>
        /// Called right before the application is closing.
        /// </summary>
        protected abstract void ApplicationClosing();
        /// <summary>
        /// Register services into the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection"></param>
        protected abstract void RegisterServices(IServiceCollection services);
        /// <summary>
        /// Handle a caught exception.
        /// </summary>
        /// <param name="e">Exception to be handled.</param>
        /// <returns>True if exception is handled. Otherwise, return false.</returns>
        protected abstract bool HandleException(Exception e);

        protected sealed override void OnStartup(StartupEventArgs e)
        {
            this.SetupExceptionHandling();
            this.RegisterInternals();
            this.RegisterServices(this.services);
            this.ServiceProvider = this.SetupServiceProvider(this.services);
            this.SetupApplicationLifetime();
            this.LaunchWindow();
        }
        protected sealed override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            foreach (var service in this.ServiceProvider.GetServices<IApplicationLifetimeService>())
            {
                service?.OnClosing();
            }

            this.ApplicationClosing();
        }
        protected sealed override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);
            this.ApplicationClosing();
        }

        private void LaunchWindow()
        {
            this.ApplicationStarting();
            foreach(var service in this.ServiceProvider.GetServices<IApplicationLifetimeService>())
            {
                service.OnStartup();
            }

            var window = this.ServiceProvider.GetRequiredService<T>();
            window.Show();
        }
        private void RegisterInternals()
        {
            this.services.AddSingleton<T, T>();
        }
        private void SetupExceptionHandling()
        {
            this.DispatcherUnhandledException += (_, e) =>
            {
                e.Handled = this.HandleException(e.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (this.HandleException(e.ExceptionObject.As<Exception>()) is false && e.IsTerminating is false)
                {
                    throw e.ExceptionObject.As<Exception>();
                }
            };

            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                if (this.HandleException(e.Exception))
                {
                    e.SetObserved();
                }
            };
        }
        private void SetupApplicationLifetime()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => this.ApplicationClosing();
        }
    }
}
