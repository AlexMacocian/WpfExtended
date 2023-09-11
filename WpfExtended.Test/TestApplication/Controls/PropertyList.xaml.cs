using System.Collections.ObjectModel;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using WpfExtended.Tests.TestApplication.Models;

namespace WpfExtended.Tests.TestApplication.Controls
{
    /// <summary>
    /// Interaction logic for PropertyList.xaml
    /// </summary>
    public partial class PropertyList : UserControl
    {
        public ObservableCollection<EffectProperty> EffectProperties { get; } = new ObservableCollection<EffectProperty>();

        public PropertyList()
        {
            this.InitializeComponent();
        }

        private void PropertyList_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext is not ShaderEffect shaderEffect)
            {
                return;
            }

            this.EffectProperties.ClearAnd().AddRange(shaderEffect.GetType().GetProperties()
                .Where(pInfo => pInfo.PropertyType == typeof(double))
                .Select(pInfo => new EffectProperty
                {
                    Name = pInfo.Name,
                    Property = pInfo,
                    Value = (double)pInfo.GetValue(shaderEffect),
                    Source = shaderEffect
                }));
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is not TextBox textBox)
            {
                return;
            }

            if (textBox.DataContext is not EffectProperty effectProperty)
            {
                return;
            }

            if (e.Key is not System.Windows.Input.Key.Enter)
            {
                return;
            }

            if (!double.TryParse(textBox.Text, out var value))
            {
                return;
            }

            effectProperty.Value = value;
            effectProperty.UpdateProperty();
        }
    }
}
