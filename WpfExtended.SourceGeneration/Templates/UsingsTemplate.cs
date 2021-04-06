namespace System.Extensions.Templates
{
    internal sealed class UsingsTemplate : AbstractTemplate
    {
        public string Namespace { get; set; }
        public UsingsTemplate WithNamespace(string n)
        {
            this.Namespace = n;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            codeWriter
                .Append("using ")
                .Append(this.Namespace)
                .Append(';');
        }
    }
}
