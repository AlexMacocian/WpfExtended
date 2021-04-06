using System.Collections.Generic;

namespace System.Extensions.Templates
{
    internal sealed class CodeBlockTemplate : CodeTemplate
    {
        public List<string> Lines { get; set; } = new List<string>();

        public CodeBlockTemplate WithLines(params string[] lines)
        {
            this.Lines.Clear();
            this.Lines.AddRange(lines);
            return this;
        }

        public CodeBlockTemplate WithLine(string line)
        {
            this.Lines.Add(line);
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            foreach(var line in this.Lines)
            {
                codeWriter.AppendLine(line);
            }
        }
    }
}
