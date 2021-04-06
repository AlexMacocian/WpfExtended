using System.Collections.Generic;

namespace System.Extensions.Templates
{
    internal sealed class ConstructorTemplate : AbstractTemplate
    {
        public List<Modifier> Modifiers { get; } = new List<Modifier>();
        public List<ArgumentTemplate> Arguments { get; } = new List<ArgumentTemplate>();
        public string Type { get; set; }
        public string Body { get; set; }
        public string Base { get; set; }

        public ConstructorTemplate WithType(string type)
        {
            this.Type = type;
            return this;
        }
        public ConstructorTemplate WithBody(string body)
        {
            this.Body = body;
            return this;
        }
        public ConstructorTemplate WithBase(string b)
        {
            this.Base = b;
            return this;
        }
        public ConstructorTemplate WithModifiers(params Modifier[] modifiers)
        {
            this.Modifiers.Clear();
            this.Modifiers.AddRange(modifiers);
            return this;
        }
        public ConstructorTemplate WithArguments(params ArgumentTemplate[] argumentTemplates)
        {
            this.Arguments.Clear();
            this.Arguments.AddRange(argumentTemplates);
            return this;
        }
        public ConstructorTemplate WithArgument(ArgumentTemplate argumentTemplate)
        {
            this.Arguments.Add(argumentTemplate);
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
                .Append('(');
            for (int i = 0; i < this.Arguments.Count; i++)
            {
                var argument = this.Arguments[i];
                codeWriter.Append(argument);
                if (i < this.Arguments.Count - 1)
                {
                    codeWriter.Append(", ");
                }
            }

            codeWriter.Append(')');
            if (!string.IsNullOrEmpty(this.Base))
            {
                codeWriter
                    .Append(" : base(")
                    .Append(this.Base)
                    .Append(')');
            }

            codeWriter.BeginCodeBlock()
                .Append(this.Body)
                .EndCodeBlock();
        }
    }
}
