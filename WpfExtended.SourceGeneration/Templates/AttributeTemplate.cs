namespace System.Extensions.Templates
{
    internal sealed class AttributeTemplate : AbstractTemplate
    {
        public string Attribute { get; set; }

        public AttributeTemplate WithAttribute(string attribute)
        {
            this.Attribute = attribute;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            codeWriter
                .Append('[')
                .Append(this.Attribute)
                .Append(']');
        }
    }
}
