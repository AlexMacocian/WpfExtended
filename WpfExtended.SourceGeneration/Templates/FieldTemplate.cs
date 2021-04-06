using System;
using System.Collections.Generic;
using System.Text;

namespace System.Extensions.Templates
{
    internal sealed class FieldTemplate : AbstractTemplate
    {
        public List<Modifier> Modifiers { get; } = new List<Modifier> { Modifier.Public };
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public FieldTemplate WithName(string name)
        {
            this.Name = name;
            return this;
        }
        public FieldTemplate WithValue(string value)
        {
            this.Value = value;
            return this;
        }
        public FieldTemplate WithModifiers(params Modifier[] modifiers)
        {
            this.Modifiers.Clear();
            this.Modifiers.AddRange(modifiers);
            return this;
        }
        public FieldTemplate WithType(string type)
        {
            this.Type = type;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            foreach (var modifier in this.Modifiers)
            {
                codeWriter
                    .Append(modifier)
                    .Append(' ');
            }

            codeWriter.Append(this.Type)
                .Append(' ')
                .Append(this.Name)
                .Append(' ')
                .Append('=')
                .Append(' ')
                .Append(this.Value)
                .Append(';');
        }
    }
}
