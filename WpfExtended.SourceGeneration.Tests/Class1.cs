using FluentAssertions;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media.Effects;

namespace WpfExtended.SourceGeneration.Tests;

internal partial class Class1 : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private bool calledEvent;

    [GenerateDependencyProperty]
    public int someF;
    [GenerateDependencyProperty(InitialValue = "This has a value")]
    public string someValue;
    [GenerateDependencyProperty]
    public Effect effect;
    [GenerateNotifyPropertyChanged]
    public string someNotifyingValue;


    public void TestValues()
    {
        this.PropertyChanged += Class1_PropertyChanged;
        this.SomeF.Should().Be(0);
        this.SomeValue.Should().Be("This has a value");
        this.Effect.Should().BeNull();

        this.SomeNotifyingValue = "NewValue";
        this.calledEvent.Should().BeTrue();
        this.someNotifyingValue.Should().Be("NewValue");
    }

    private void Class1_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        this.calledEvent = true;
    }
}
