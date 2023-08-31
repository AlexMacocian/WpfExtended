using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Extensions.Templates;

namespace System.Extensions
{
    [Generator(LanguageNames.CSharp)]
    public class DependencyPropertyGenerator : ISourceGenerator
    {
        private const string AttributeNamespace = "System.Windows.Extensions";
        private const string AttributeName = "GenerateDependencyPropertyAttribute";
        private const string DependencyPropertySuffix = "Property";
        private static readonly ClassTemplate GenerateDependencyPropertyAttribute =
            new ClassTemplate()
                .WithUsings(
                    new UsingsTemplate()
                        .WithNamespace("System"))
                .WithNamespace(
                    new NamespaceTemplate()
                        .WithNamespace(AttributeNamespace))
                .WithConstructor(
                    new ConstructorTemplate()
                        .WithType(AttributeName)
                        .WithModifiers(Modifier.Public))
                .WithAttributes(
                    new AttributeTemplate()
                        .WithAttribute("AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)"))
                .WithModifiers(Modifier.Public, Modifier.Sealed)
                .WithName(AttributeName)
                .WithBase("Attribute")
                .WithProperty(
                    new PropertyTemplate()
                        .WithModifiers(Modifier.Public)
                        .WithName("InitialValue")
                        .WithType("object"));

        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var attributeText = GenerateDependencyPropertyAttribute.GenerateString();
            context.AddSource("GenerateDependencyPropertyAttribute.g", attributeText);

            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                return;

            var options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            var compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));

            var attributeSymbol = compilation.GetTypeByMetadataName($"{AttributeNamespace}.{AttributeName}");
            if (attributeSymbol is null)
            {
                context.ReportDiagnostic(Diagnostics.MissingAttributeDiagnostic(AttributeName, AttributeNamespace));
                return;
            }

            var fieldSymbols = new List<IFieldSymbol>();
            foreach (var field in receiver.CandidateFields)
            {
                var model = compilation.GetSemanticModel(field.SyntaxTree);
                foreach (var variable in field.Declaration.Variables)
                {
                    var fieldSymbol = model.GetDeclaredSymbol(variable) as IFieldSymbol;
                    if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
                    {
                        fieldSymbols.Add(fieldSymbol);
                    }
                }
            }

            foreach (var group in fieldSymbols.GroupBy(f => f.ContainingType, SymbolEqualityComparer.IncludeNullability))
            {
                var classSource = ProcessClass((INamedTypeSymbol)group.Key, attributeSymbol, group.ToList(), context);
                context.AddSource($"{group.Key.Name}.DependencyPropertyGenerator.g.cs", classSource);
            }
        }

        private static string ProcessClass(INamedTypeSymbol classSymbol, INamedTypeSymbol attributeSymbol, List<IFieldSymbol> fields, GeneratorExecutionContext generatorExecutionContext)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                var syntaxTree = classSymbol.DeclaringSyntaxReferences.First().SyntaxTree;
                var textSpan = classSymbol.DeclaringSyntaxReferences.First().Span;
                generatorExecutionContext.ReportDiagnostic(
                    Diagnostics.ClassNotTopLevelDiagnostic(classSymbol.Name, classSymbol.ContainingSymbol.Name, syntaxTree, textSpan));
                return null;
            }

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            var source = new ClassTemplate()
                .WithUsings(
                    new UsingsTemplate()
                        .WithNamespace("System"),
                    new UsingsTemplate()
                        .WithNamespace("System.Windows"))
                .WithNamespace(
                    new NamespaceTemplate()
                        .WithNamespace(namespaceName))
                .WithName(classSymbol.Name);
            AssignModifiers(source, classSymbol);
            foreach (var fieldSymbol in fields)
            {
                ProcessField(source, classSymbol, attributeSymbol, fieldSymbol);
            }

            return source.GenerateString();
        }

        private static void AssignModifiers(ClassTemplate source, INamedTypeSymbol classSymbol)
        {
            //Find class declaration syntax and parse modifiers.
            if (classSymbol.DeclaringSyntaxReferences.Select(sr => sr.GetSyntax()).OfType<ClassDeclarationSyntax>().First() is ClassDeclarationSyntax classDeclarationSyntax)
            {
                source.WithModifiers(classDeclarationSyntax.Modifiers.Select(m => Modifier.Parse(m.ValueText)).ToArray());
            }
        }

        private static void ProcessField(ClassTemplate source, INamedTypeSymbol classSymbol, INamedTypeSymbol attributeSymbol, IFieldSymbol fieldSymbol)
        {
            var fieldName = fieldSymbol.Name;
            var fieldType = fieldSymbol.Type;

            string propertyName = GenerateName(fieldName);

            var attributeData = fieldSymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            var initialValueOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "InitialValue").Value;

            var dependencyPropertyValue = initialValueOpt.Value is null ?
                @$"DependencyProperty.Register(""{propertyName}"", typeof({fieldType}), typeof({classSymbol.Name}))" :
                @$"DependencyProperty.Register(""{propertyName}"", typeof({fieldType}), typeof({classSymbol.Name}), new PropertyMetadata({initialValueOpt.ToCSharpString()}))";

            source
                .WithField(
                    new FieldTemplate()
                    .WithModifiers(Modifier.Public, Modifier.Static, Modifier.Readonly)
                    .WithType("DependencyProperty")
                    .WithName(propertyName + DependencyPropertySuffix)
                    .WithValue(dependencyPropertyValue))
                .WithProperty(
                    new PropertyTemplate()
                    .WithType(fieldType.ToString())
                    .WithName(propertyName)
                    .WithGetter(
                        new GetterTemplate()
                            .WithCode(
                                new CodeBlockTemplate()
                                    .WithLine($"this.{fieldName} = ({fieldType})this.GetValue({propertyName + DependencyPropertySuffix});")
                                    .WithLine($"return ({fieldType})this.GetValue({propertyName + DependencyPropertySuffix});")))
                    .WithSetter(
                        new SetterTemplate()
                            .WithCode(
                                new CodeBlockTemplate()
                                    .WithLine($"this.{fieldName} = value;")
                                    .WithLine($"this.SetValue({propertyName + DependencyPropertySuffix}, value);"))));
        }

        private static string GenerateName(string fieldName)
        {
            fieldName = fieldName.TrimStart('_');
            if (char.IsUpper(fieldName.First()))
            {
                return char.ToLower(fieldName.First()) + fieldName.Substring(1);
            }
            else
            {
                return char.ToUpper(fieldName.First()) + fieldName.Substring(1);
            }
        }

        internal class SyntaxReceiver : ISyntaxReceiver
        {
            public List<FieldDeclarationSyntax> CandidateFields { get; } = new List<FieldDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is FieldDeclarationSyntax fieldDeclarationSyntax &&
                    fieldDeclarationSyntax.AttributeLists.Count > 0)
                {
                    this.CandidateFields.Add(fieldDeclarationSyntax);
                }
            }
        }
    }
}
