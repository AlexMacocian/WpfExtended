using System.Collections.Generic;

namespace System.Extensions.Templates
{
    internal sealed class MethodTemplate : AbstractTemplate
    {
        public List<Modifier> Modifiers { get; } = new List<Modifier>();
        public List<ArgumentTemplate> Arguments { get; } = new List<ArgumentTemplate>();
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public string Body { get; set; }

        public MethodTemplate WithModifiers(params Modifier[] modifiers)
        {
            this.Modifiers.Clear();
            this.Modifiers.AddRange(modifiers);
            return this;
        }
        public MethodTemplate WithArguments(params ArgumentTemplate[] argumentTemplates)
        {
            this.Arguments.Clear();
            this.Arguments.AddRange(argumentTemplates);
            return this;
        }
        public MethodTemplate WithName(string name)
        {
            this.Name = name;
            return this;
        }
        public MethodTemplate WithReturnType(string returnType)
        {
            this.ReturnType = returnType;
            return this;
        }
        public MethodTemplate WithBody(string body)
        {
            this.Body = body;
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
                .Append(this.ReturnType)
                .Append(' ')
                .Append(this.Name)
                .Append('(');
            for(int i = 0; i < this.Arguments.Count; i++)
            {
                var argument = this.Arguments[i];
                codeWriter.Append(argument);
                if (i < this.Arguments.Count - 1)
                {
                    codeWriter.Append(", ");
                }
            }

            codeWriter
                .Append(')')
                .BeginCodeBlock()
                .Append(this.Body)
                .EndCodeBlock();
        }
    }
}