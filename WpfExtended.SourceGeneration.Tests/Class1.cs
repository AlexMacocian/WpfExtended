using FluentAssertions;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media.Effects;

namespace WpfExtended.SourceGeneration.Tests
{
    public partial class Class1 : UserControl
    {
        [GenerateDependencyProperty]
        public int someF;
        [GenerateDependencyProperty(InitialValue = "This has a value")]
        public string someValue;
        [GenerateDependencyProperty]
        public Effect effect;


        public void TestValues()
        {
            this.SomeF.Should().Be(0);
            this.SomeValue.Should().Be("This has a value");
            this.Effect.Should().BeNull();
        }
    }
}
