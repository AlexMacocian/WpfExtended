using Slim;
using System;
using System.Windows.Extensions;
using WpfExtended.Test;

namespace WpfExtended.Tests
{
    public class Launcher : ExtendedApplication<MainWindow>
    {
        private static Launcher Instance { get; } = new Launcher();

        [STAThread]
        public static int Main()
        {            
            return Instance.Run();
        }

        protected override bool HandleException(Exception e)
        {
            return false;
        }

        protected override void RegisterServices(IServiceProducer serviceProducer)
        {
        }
    }
}
