﻿using Slim;
using System.Extensions;
using System.Threading.Tasks;
using System.Windows.Navigation;

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

        public ExtendedApplication()
        {
            this.RegisterInternals();
        }

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
            this.RegisterServices(this.ServiceManager);
            this.LaunchWindow();
        }

        private void LaunchWindow()
        {
            var window = this.ServiceManager.GetService<T>();
            window.Show();
        }
        private void RegisterInternals()
        {
            this.ServiceManager.RegisterServiceManager();
            this.ServiceManager.RegisterSingleton<T, T>();
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
    }
}