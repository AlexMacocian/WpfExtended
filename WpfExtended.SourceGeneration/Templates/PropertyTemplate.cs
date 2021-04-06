using System.Collections.Generic;

namespace System.Extensions.Templates
{
    internal sealed class PropertyTemplate : AbstractTemplate
    {
        public static GetterTemplate DefaultGetter = new GetterTemplate()
            .WithCode(
                new SimpleCodeTemplate()
                    .WithCode(";"));
        public static SetterTemplate DefaultSetter = new SetterTemplate()
            .WithCode(
                new SimpleCodeTemplate()
                    .WithCode(";"));
        public List<Modifier> Modifiers { get; private set; } = new List<Modifier>() { Modifier.Public };
        public GetterTemplate Getter { get; set; } = DefaultGetter;
        public SetterTemplate Setter { get; set; } = DefaultSetter;
        public string Type { get; set; }
        public string Name { get; set; }

        public PropertyTemplate WithGetter(GetterTemplate getter)
        {
            this.Getter = getter;
            return this;
        }
        public PropertyTemplate WithSetter(SetterTemplate setter)
        {
            this.Setter = setter;
            return this;
        }
        public PropertyTemplate WithModifiers(params Modifier[] modifiers)
        {
            this.Modifiers.Clear();
            this.Modifiers.AddRange(modifiers);
            return this;
        }
        public PropertyTemplate WithName(string name)
        {
            this.Name = name;
            return this;
        }
        public PropertyTemplate WithType(string type)
        {
            this.Type = type;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            foreach(var modifier in this.Modifiers)
            {
                codeWriter
                    .Append(modifier)
                    .Append(' ');
            }

            codeWriter
                .Append(this.Type)
                .Append(' ')
                .Append(this.Name)
                .BeginCodeBlock()
                .Append(this.Getter)
                .Append(this.Setter)
                .EndCodeBlock();
        }
    }
}
