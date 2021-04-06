using System.Collections.Generic;

namespace System.Extensions.Templates
{
    internal sealed class SetterTemplate : AbstractTemplate
    {
        public List<Modifier> Modifiers { get; } = new List<Modifier>();
        public CodeTemplate Code { get; set; }

        public SetterTemplate WithModifiers(params Modifier[] modifiers)
        {
            this.Modifiers.Clear();
            this.Modifiers.AddRange(modifiers);
            return this;
        }
        public SetterTemplate WithCode(CodeTemplate codeTemplate)
        {
            this.Code = codeTemplate;
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

            codeWriter.Append("set");
            if (this.Code is SimpleCodeTemplate simpleSetter)
            {
                codeWriter
                    .Append(simpleSetter)
                    .AppendLine();
            }
            else
            {
                codeWriter
                    .BeginCodeBlock()
                    .Append(this.Code)
                    .EndCodeBlock();
            }

        }
    }
}
