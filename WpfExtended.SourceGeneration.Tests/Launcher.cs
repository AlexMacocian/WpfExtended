using System;

namespace WpfExtended.SourceGeneration.Tests
{
    public class Launcher
    {
        [STAThread]
        public static int Main()
        {
            var cls = new Class1();
            cls.TestValues();
            return 0;
        }
    }
}
