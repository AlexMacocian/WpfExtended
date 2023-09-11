using System.Reflection;

namespace WpfExtended.Tests.TestApplication.Models
{
    public sealed class EffectProperty
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public object Source { get; set; }
        public PropertyInfo Property { get; set; }

        public void UpdateProperty()
        {
            if (this.Source is null)
            {
                return;
            }

            if (this.Property is null)
            {
                return;
            }

            this.Property.SetValue(this.Source, this.Value);
        }
    }
}
