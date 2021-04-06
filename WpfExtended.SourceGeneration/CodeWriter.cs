using System.Extensions.Templates;
using System.Linq;
using System.Text;

namespace System.Extensions
{
    internal class CodeWriter
    {
        private StringBuilder StringBuilder { get; } = new StringBuilder();
        private CodeBlock CurrentBlock { get; set; } = new CodeBlock();

        public CodeWriter Append(string value)
        {
            this.CurrentBlock.Append(this.StringBuilder, value);
            return this;
        }

        public CodeWriter AppendLine(string value)
        {
            this.CurrentBlock.AppendLine(this.StringBuilder, value);
            return this;
        }

        public CodeWriter AppendLine()
        {
            this.CurrentBlock.AppendLine(this.StringBuilder);
            return this;
        }

        public CodeWriter Append(char c)
        {
            this.CurrentBlock.Append(this.StringBuilder, c);
            return this;
        }

        public CodeWriter AppendLine(char c)
        {
            this.AppendLine(c);
            return this;
        }

        public CodeWriter Append(AbstractTemplate abstractTemplate)
        {
            abstractTemplate.Generate(this);
            return this;
        }

        public CodeWriter BeginCodeBlock()
        {
            this.CurrentBlock.AppendLine(this.StringBuilder).AppendLine(this.StringBuilder, '{');
            this.CurrentBlock = CodeBlock.GenerateCodeBlock(this.CurrentBlock);
            return this;
        }

        public CodeWriter EndCodeBlock()
        {
            this.CurrentBlock = this.CurrentBlock.End(this.StringBuilder).Parent;
            this.CurrentBlock.AppendLine(this.StringBuilder, '}');
            return this;
        }

        public override string ToString()
        {
            return this.StringBuilder.ToString();
        }

        private class CodeBlock
        {
            private const char PrefixSymbol = '\t';
            private bool NewLine { get; set; } = true;
            private string Prefix { get; }
            public CodeBlock Parent { get; }
            public int Level { get; }
            public CodeBlock()
            {
                this.Level = 0;
                this.Prefix = string.Empty;
            }
            private CodeBlock(int level, CodeBlock parent)
            {
                this.Level = level;
                this.Prefix = new string(Enumerable.Repeat(PrefixSymbol, this.Level).ToArray());
                this.Parent = parent;
            }

            public CodeBlock AppendLine(StringBuilder stringBuilder)
            {
                stringBuilder.AppendLine();
                this.NewLine = true;
                return this;
            }

            public CodeBlock AppendLine(StringBuilder stringBuilder, string value)
            {
                value = this.NewLine is true ? Prefix + value : value;
                stringBuilder.AppendLine(value);
                this.NewLine = true;
                return this;
            }

            public CodeBlock Append(StringBuilder stringBuilder, string value)
            {
                value = this.NewLine is true ? Prefix + value : value;
                stringBuilder.Append(value);
                this.NewLine = false;
                return this;
            }

            public CodeBlock Append(StringBuilder stringBuilder, char c)
            {
                var value = this.NewLine is true ? Prefix + c : c.ToString();
                stringBuilder.Append(value);
                this.NewLine = false;
                return this;
            }

            public CodeBlock AppendLine(StringBuilder stringBuilder, char c)
            {
                var value = this.NewLine is true ? Prefix + c : c.ToString();
                stringBuilder.AppendLine(value);
                this.NewLine = true;
                return this;
            }

            public CodeBlock End(StringBuilder stringBuilder)
            {
                if (this.NewLine is false)
                {
                    stringBuilder.AppendLine();
                    this.NewLine = true;
                }

                return this;
            }

            public static CodeBlock GenerateCodeBlock(CodeBlock codeBlock)
            {
                var child = new CodeBlock(codeBlock.Level + 1, codeBlock);
                return child;
            }
        }
    }
}
