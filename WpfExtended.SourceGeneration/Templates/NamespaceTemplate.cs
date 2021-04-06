namespace System.Extensions.Templates
{
    internal sealed class NamespaceTemplate : AbstractTemplate
    {
        public string Namespace { get; set; }
        public NamespaceTemplate WithNamespace(string n)
        {
            this.Namespace = n;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            codeWriter
                .Append("namespace ")
                .Append(this.Namespace);
        }
    }
}
