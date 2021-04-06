using System.Collections.Generic;

namespace System.Extensions.Templates
{
    internal sealed class ClassTemplate : AbstractTemplate
    {
        public List<AttributeTemplate> Attributes { get; } = new List<AttributeTemplate>();
        public List<UsingsTemplate> Usings { get; } = new List<UsingsTemplate>();
        public List<Modifier> Modifiers { get; } = new List<Modifier>();
        public List<PropertyTemplate> Properties { get; } = new List<PropertyTemplate>();
        public List<FieldTemplate> Fields { get; } = new List<FieldTemplate>();
        public List<MethodTemplate> Methods { get; } = new List<MethodTemplate>();
        public List<ConstructorTemplate> Constructors { get; } = new List<ConstructorTemplate>();
        public NamespaceTemplate Namespace { get; set; }
        public string Name { get; set; }
        public string Base { get; set; }

        public ClassTemplate WithAttributes(params AttributeTemplate[] attributeTemplates)
        {
            this.Attributes.Clear();
            this.Attributes.AddRange(attributeTemplates);
            return this;
        }
        public ClassTemplate WithAttribute(AttributeTemplate attributeTemplate)
        {
            this.Attributes.Add(attributeTemplate);
            return this;
        }
        public ClassTemplate WithUsings(params UsingsTemplate[] usingsTemplates)
        {
            this.Usings.Clear();
            this.Usings.AddRange(usingsTemplates);
            return this;
        }
        public ClassTemplate WithUsing(UsingsTemplate usingsTemplate)
        {
            this.Usings.Add(usingsTemplate);
            return this;
        }
        public ClassTemplate WithModifiers(params Modifier[] modifiers)
        {
            this.Modifiers.Clear();
            this.Modifiers.AddRange(modifiers);
            return this;
        }
        public ClassTemplate WithModifier(Modifier modifier)
        {
            this.Modifiers.Add(modifier);
            return this;
        }
        public ClassTemplate WithMethods(params MethodTemplate[] methodTemplates)
        {
            this.Methods.Clear();
            this.Methods.AddRange(methodTemplates);
            return this;
        }
        public ClassTemplate WithMethod(MethodTemplate methodTemplate)
        {
            this.Methods.Add(methodTemplate);
            return this;
        }
        public ClassTemplate WithFields(params FieldTemplate[] fieldTemplates)
        {
            this.Fields.Clear();
            this.Fields.AddRange(fieldTemplates);
            return this;
        }
        public ClassTemplate WithField(FieldTemplate fieldTemplate)
        {
            this.Fields.Add(fieldTemplate);
            return this;
        }
        public ClassTemplate WithProperties(params PropertyTemplate[] propertyTemplates)
        {
            this.Properties.Clear();
            this.Properties.AddRange(propertyTemplates);
            return this;
        }
        public ClassTemplate WithProperty(PropertyTemplate propertyTemplate)
        {
            this.Properties.Add(propertyTemplate);
            return this;
        }
        public ClassTemplate WithConstructors(params ConstructorTemplate[] constructorTemplates)
        {
            this.Constructors.Clear();
            this.Constructors.AddRange(constructorTemplates);
            return this;
        }
        public ClassTemplate WithConstructor(ConstructorTemplate constructorTemplate)
        {
            this.Constructors.Add(constructorTemplate);
            return this;
        }
        public ClassTemplate WithNamespace(NamespaceTemplate namespaceTemplate)
        {
            this.Namespace = namespaceTemplate;
            return this;
        }
        public ClassTemplate WithName(string name)
        {
            this.Name = name;
            return this;
        }
        public ClassTemplate WithBase(string b)
        {
            this.Base = b;
            return this;
        }

        public override void Generate(CodeWriter codeWriter)
        {
            foreach(var u in this.Usings)
            {
                codeWriter
                    .Append(u)
                    .AppendLine();
            }

            codeWriter
                .AppendLine()
                .Append(this.Namespace)
                .BeginCodeBlock();
            foreach(var attribute in this.Attributes)
            {
                codeWriter
                    .Append(attribute)
                    .AppendLine();
            }

            foreach(var modifier in this.Modifiers)
            {
                codeWriter
                    .Append(modifier)
                    .Append(' ');
            }

            codeWriter
                .Append("class")
                .Append(' ')
                .Append(this.Name);
            if (!string.IsNullOrEmpty(this.Base))
            {
                codeWriter
                    .Append(' ')
                    .Append(':')
                    .Append(' ')
                    .Append(this.Base);
            }

            codeWriter.BeginCodeBlock();
            foreach(var field in this.Fields)
            {
                codeWriter
                    .Append(field)
                    .AppendLine();
            }

            foreach(var property in this.Properties)
            {
                codeWriter
                    .Append(property)
                    .AppendLine();
            }

            foreach(var constructor in this.Constructors)
            {
                codeWriter
                    .Append(constructor)
                    .AppendLine();
            }

            foreach(var method in this.Methods)
            {
                codeWriter
                    .Append(method)
                    .AppendLine();
            }

            codeWriter.EndCodeBlock().EndCodeBlock();
        }

        public string GenerateString()
        {
            var codeWriter = new CodeWriter();
            this.Generate(codeWriter);
            return codeWriter.ToString();
        }
    }
}
