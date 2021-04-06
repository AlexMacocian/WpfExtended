namespace System.Extensions.Templates
{
    internal sealed class ArgumentTemplate : AbstractTemplate
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public ArgumentTemplate WithName(string name)
        {
            this.Name = name;
            return this;
        }

        public ArgumentTemplate WithType(string type)
        {
            this.Type = type;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            codeWriter
                .Append(this.Type)
                .Append(' ')
                .Append(this.Name);
        }
    }
}
