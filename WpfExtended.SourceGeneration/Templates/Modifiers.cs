namespace System.Extensions.Templates
{
    internal sealed class Modifier : AbstractTemplate
    {
        public static Modifier Readonly { get; } = new Modifier { Value = "readonly" };
        public static Modifier Static { get; } = new Modifier { Value = "static" };
        public static Modifier Public { get; } = new Modifier { Value = "public" };
        public static Modifier Internal { get; } = new Modifier { Value = "internal" };
        public static Modifier Private { get; } = new Modifier { Value = "private" };
        public static Modifier Abstract { get; } = new Modifier { Value = "abstract" };
        public static Modifier Virtual { get; } = new Modifier { Value = "virtual" };
        public static Modifier Partial { get; } = new Modifier { Value = "partial" };
        public static Modifier Sealed { get; } = new Modifier { Value = "sealed" };

        public string Value { get; private set; }

        private Modifier()
        {
        }

        public override void Generate(CodeWriter codeWriter)
        {
            codeWriter.Append(this.Value);
        }
    }
}
