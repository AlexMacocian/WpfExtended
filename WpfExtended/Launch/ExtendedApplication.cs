using Microsoft.Extensions.Logging;
using Slim;
using System.Extensions;
using System.Threading.Tasks;
using System.Windows.Extensions.Logging;
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
        protected IServiceManager ServiceManager { get; } = new ServiceManager();
        protected ILoggerFactory LoggerFactory { get; private set; }

        public ExtendedApplication()
        {
            this.RegisterInternals();
        }

        /// <summary>
        /// Do some work on the <see cref="IServiceManager"/> before calling <see cref="RegisterServices(IServiceProducer)"/>.
        /// </summary>
        protected virtual void SetupServiceManager(IServiceManager serviceManager)
        {
            serviceManager.RegisterDebugLoggerFactory();
            serviceManager.RegisterHttpFactory();
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
        /// Register services into the <see cref="IServiceProducer"/>.
        /// </summary>
        /// <param name="serviceProducer"></param>
        protected abstract void RegisterServices(IServiceProducer serviceProducer);
        /// <summary>
        /// Handle a caught exception.
        /// </summary>
        /// <param name="e">Exception to be handled.</param>
        /// <returns>True if exception is handled. Otherwise, return false.</returns>
        protected abstract bool HandleException(Exception e);

        protected sealed override void OnStartup(StartupEventArgs e)
        {
            this.SetupExceptionHandling();
            this.SetupServiceManager(this.ServiceManager);
            this.RegisterServices(this.ServiceManager);
            this.SetupApplicationLifetime();
            this.LaunchWindow();
        }
        protected sealed override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            foreach (var service in this.ServiceManager.GetServicesOfType<IApplicationLifetimeService>())
            {
                service.OnClosing();
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
            var window = this.ServiceManager.GetService<T>();
            this.ApplicationStarting();
            foreach(var service in this.ServiceManager.GetServicesOfType<IApplicationLifetimeService>())
            {
                service.OnStartup();
            }

            window.Show();
        }
        private void RegisterInternals()
        {
            this.ServiceManager.RegisterServiceManager();
            this.ServiceManager.RegisterSingleton<T, T>();
            this.ServiceManager.RegisterResolver(new LoggerResolver());
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
