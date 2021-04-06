using System.Windows.Controls;
using System.Windows.Extensions;

namespace WpfExtended.SourceGeneration.Tests
{
    public partial class Class1 : UserControl
    {
        [GenerateDependencyProperty]
        public int someF;
    }
}
