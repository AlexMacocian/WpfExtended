using System.Collections.Generic;

namespace System.Extensions.Templates
{
    internal sealed class GetterTemplate : AbstractTemplate
    {
        public List<Modifier> Modifiers { get; } = new List<Modifier>();
        public CodeTemplate Code { get; set; }

        public GetterTemplate WithModifiers(params Modifier[] modifiers)
        {
            this.Modifiers.Clear();
            this.Modifiers.AddRange(modifiers);
            return this;
        }
        public GetterTemplate WithCode(CodeTemplate codeTemplate)
        {
            this.Code = codeTemplate;
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

            codeWriter.Append("get");
            if (this.Code is SimpleCodeTemplate simpleGetter)
            {
                codeWriter
                    .Append(simpleGetter)
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
