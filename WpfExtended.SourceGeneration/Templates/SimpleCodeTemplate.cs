namespace System.Extensions.Templates
{
    internal sealed class SimpleCodeTemplate : CodeTemplate
    {
        public string Code { get; set; }

        public SimpleCodeTemplate WithCode(string code)
        {
            this.Code = code;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            codeWriter.Append(this.Code);
        }
    }
}
