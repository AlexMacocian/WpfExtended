using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sybil;
using System.Linq;
using System.Collections.Immutable;

namespace System.Extensions;
#nullable enable
[Generator(LanguageNames.CSharp)]
public class NotifyPropertyChangedGenerator : IIncrementalGenerator
{
    private const string AttributeNamespace = "System.Windows.Extensions";
    private const string AttributeName = "GenerateNotifyPropertyChangedAttribute";
    private const string AttributeShortName = "GenerateNotifyPropertyChanged";
    private const string PropertyChangedEventHandler = "PropertyChangedEventHandler";
    private const string NullablePropertyChangedEventHandler = "PropertyChangedEventHandler?";
    private const string PropertyChanged = "PropertyChanged";
    private const string Partial = "partial";
    private const string Public = "public";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context =>
        {
            var attributeNamespaceBuilder = SyntaxBuilder.CreateNamespace(AttributeNamespace)
                .WithUsing("System.ComponentModel")
                    .WithClass(SyntaxBuilder.CreateClass(AttributeName)
                        .WithModifier(Public)
                    .WithConstructor(SyntaxBuilder.CreateConstructor(AttributeName)
                        .WithModifier(Public))
                    .WithAttribute(SyntaxBuilder.CreateAttribute("AttributeUsage")
                        .WithArgument(AttributeTargets.Field)
                        .WithArgument("Inherited", false)
                        .WithArgument("AllowMultiple", false))
                    .WithBaseClass(nameof(Attribute)));
            var attributeSyntax = attributeNamespaceBuilder.Build();
            var source = attributeSyntax.ToFullString();
            context.AddSource($"{AttributeName}.g", source);
        });

        var classDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (s, _) => s is FieldDeclarationSyntax,
            transform: static (ctx, _) => GetFilteredFieldDeclarationSyntax(ctx)).Where(static c => c is not null);

        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());
        context.RegisterSourceOutput(compilationAndClasses, (sourceProductionContext, tuple) => Execute(tuple.Left, tuple.Right, sourceProductionContext));
    }

    private static ClassDeclarationSyntax? GetFilteredFieldDeclarationSyntax(GeneratorSyntaxContext context)
    {
        var fieldDeclarationSyntax = (FieldDeclarationSyntax)context.Node;
        if (fieldDeclarationSyntax.AttributeLists
            .SelectMany(l => l.Attributes)
            .OfType<AttributeSyntax>()
            .Any(s => s.Name.ToString() == AttributeName || s.Name.ToString() == AttributeShortName))
        {
            return fieldDeclarationSyntax.Parent as ClassDeclarationSyntax;
        }

        return default;
    }

    private static void Execute(Compilation _, ImmutableArray<ClassDeclarationSyntax?> classes, SourceProductionContext sourceProductionContext)
    {
        if (classes.IsDefaultOrEmpty)
        {
            return;
        }

        var distinctClasses = classes.Distinct();
        foreach (var c in distinctClasses.OfType<ClassDeclarationSyntax>())
        {
            var className = c.Identifier.ToFullString();
            if (!c.Modifiers.Any(m => m.ToString() == Partial))
            {
                Diagnostics.ClassNotPartial(className);
                continue;
            }

            var originalNamespaceSyntax = GetParentOfType<BaseNamespaceDeclarationSyntax>(c);
            if (originalNamespaceSyntax is null)
            {
                Diagnostics.ClassNotInNamespace(className);
                continue;
            }

            if (!c.Members
                .OfType<EventFieldDeclarationSyntax>()
                .Any(eventField =>
                    (eventField.Declaration.Variables.Any(variable => variable.Identifier.Text == PropertyChanged) &&
                    eventField.Modifiers.Any(modifier => modifier.Text == Public) &&
                    eventField.Declaration.Type.ToString() == PropertyChangedEventHandler) ||
                    eventField.Declaration.Type.ToString() == NullablePropertyChangedEventHandler))
            {
                Diagnostics.ClassDoesNotImplementINotifyPropertyChanged(className);
                continue;
            }

            var generatedClassBuilder = SyntaxBuilder.CreateClass(className)
                .WithModifiers(string.Join(" ", c.Modifiers.Select(m => m.ToFullString())));
            var generatedNamespaceBuilder = SyntaxBuilder.CreateNamespace(originalNamespaceSyntax.Name.ToFullString())
                .WithClass(generatedClassBuilder);
            if (originalNamespaceSyntax.Usings.Count != 0)
            {
                foreach (var u in originalNamespaceSyntax.Usings.OfType<UsingDirectiveSyntax>())
                {
                    generatedNamespaceBuilder.WithUsing(u.Name?.ToString());
                }
            }
            else
            {
                if (originalNamespaceSyntax.Parent is CompilationUnitSyntax compilationUnit)
                {
                    foreach(var u in compilationUnit.Usings.OfType<UsingDirectiveSyntax>())
                    {
                        generatedNamespaceBuilder.WithUsing(u.Name?.ToString());
                    }
                }
            }
            
            foreach (var field in c.DescendantNodes().OfType<FieldDeclarationSyntax>())
            {
                if (field.AttributeLists
                    .SelectMany(l => l.Attributes)
                    .OfType<AttributeSyntax>()
                    .Any(s => s.Name.ToString() == AttributeName ||
                        s.Name.ToString() == AttributeShortName) is false)
                {
                    continue;
                }

                var fieldName = field.Declaration.Variables.First().Identifier.ToFullString();
                var propertyName = fieldName.TrimStart('_');
                propertyName = $"{char.ToUpper(propertyName[0])}{propertyName.Substring(1)}";
                var propertyBuilder = SyntaxBuilder.CreateProperty(field.Declaration.Type.ToFullString(), propertyName)
                    .WithModifier(Public)
                    .WithAccessor(SyntaxBuilder.CreateGetter()
                        .WithBody($"return this.{fieldName};"))
                    .WithAccessor(SyntaxBuilder.CreateSetter()
                        .WithBody($"this.{fieldName} = value;\r\nthis.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({propertyName})));"));
                generatedClassBuilder.WithProperty(propertyBuilder);
            }

            var fileSource = generatedNamespaceBuilder.Build().ToFullString();
            sourceProductionContext.AddSource($"{className}.g", fileSource);
        }
    }

    private static T? GetParentOfType<T>(SyntaxNode syntaxNode)
    {
        if (syntaxNode.Parent is null)
        {
            return default;
        }

        if (syntaxNode.Parent is T parentNode)
        {
            return parentNode;
        }

        return GetParentOfType<T>(syntaxNode.Parent);
    }
}
#nullable disable
