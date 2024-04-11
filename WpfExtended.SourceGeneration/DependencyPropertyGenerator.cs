using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sybil;
using System.Collections.Immutable;
using System.Linq;

namespace System.Extensions;
#nullable enable
[Generator(LanguageNames.CSharp)]
public class DependencyPropertyGenerator : IIncrementalGenerator
{
    private const string DependencyPropertyNamespace = "System.Windows";
    private const string AttributeNamespace = "System.Windows.Extensions";
    private const string AttributeName = "GenerateDependencyPropertyAttribute";
    private const string AttributeShortName = "GenerateDependencyProperty";
    private const string DependencyPropertySuffix = "Property";
    private const string DependencyProperty = "DependencyProperty";
    private const string Register = "Register";
    private const string Partial = "partial";
    private const string Public = "public";
    private const string Static = "static";
    private const string Readonly = "readonly";
    private const string InitialValue = "InitialValue";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context =>
        {
            var compilationUnitBuilder = 
            SyntaxBuilder.CreateCompilationUnit()
            .WithUsing("System.ComponentModel")
            .WithNamespace(
                SyntaxBuilder.CreateNamespace(AttributeNamespace)
                    .WithClass(SyntaxBuilder.CreateClass(AttributeName)
                        .WithModifier(Public)
                    .WithConstructor(SyntaxBuilder.CreateConstructor(AttributeName)
                        .WithModifier(Public))
                    .WithAttribute(SyntaxBuilder.CreateAttribute("AttributeUsage")
                        .WithArgument(AttributeTargets.Field)
                        .WithArgument("Inherited", false)
                        .WithArgument("AllowMultiple", false))
                    .WithBaseClass(nameof(Attribute))
                    .WithProperty(SyntaxBuilder.CreateProperty("object", InitialValue)
                        .WithModifier(Public)
                        .WithAccessor(SyntaxBuilder.CreateGetter())
                        .WithAccessor(SyntaxBuilder.CreateSetter()))));
            var compilationUnitSyntax = compilationUnitBuilder.Build();
            var source = compilationUnitSyntax.ToFullString();
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

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax?> classes, SourceProductionContext sourceProductionContext)
    {
        if (classes.IsDefaultOrEmpty)
        {
            return;
        }

        var maybeLanguageVersion = (compilation.SyntaxTrees.FirstOrDefault()?.Options as CSharpParseOptions)?.LanguageVersion;
        if (!maybeLanguageVersion.HasValue)
        {
            return;
        }

        var languageVersion = maybeLanguageVersion.Value;
        var distinctClasses = classes.Distinct();
        foreach (var c in distinctClasses.OfType<ClassDeclarationSyntax>())
        {
            var className = c.Identifier.ToFullString().Trim();
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

            var generatedClassBuilder = SyntaxBuilder.CreateClass(className)
                .WithModifiers(string.Join(" ", c.Modifiers.Select(m => m.ToFullString())));
            var generatedCompilationUnit = SyntaxBuilder.CreateCompilationUnit()
                .WithNamespace(
                    (languageVersion >= LanguageVersion.CSharp10 ?
                    SyntaxBuilder.CreateFileScopedNamespace(originalNamespaceSyntax.Name.ToFullString()) :
                    SyntaxBuilder.CreateNamespace(originalNamespaceSyntax.Name.ToFullString()))
                    .WithClass(generatedClassBuilder));

            if (originalNamespaceSyntax.Usings.OfType<UsingDirectiveSyntax>().Any(u => u.Name?.ToString() == DependencyPropertyNamespace) is false &&
                (originalNamespaceSyntax.Parent as CompilationUnitSyntax)?.Usings.Any(u => u.Name?.ToString() == DependencyPropertyNamespace) is false)
            {
                generatedCompilationUnit.WithUsing(DependencyPropertyNamespace);
            }

            foreach (var field in c.DescendantNodes().OfType<FieldDeclarationSyntax>())
            {
                var attributeSyntax = field.AttributeLists
                    .SelectMany(l => l.Attributes)
                    .OfType<AttributeSyntax>()
                    .FirstOrDefault(s => s.Name.ToString() == AttributeName ||
                        s.Name.ToString() == AttributeShortName);
                if (attributeSyntax is null)
                {
                    continue;
                }

                var initialValueArgument = attributeSyntax.ArgumentList?.Arguments
                    .FirstOrDefault(arg => arg.NameEquals != null && arg.NameEquals.Name.Identifier.ValueText == InitialValue);

                var fieldName = field.Declaration.Variables.First().Identifier.ToFullString().Trim();
                var fieldType = field.Declaration.Type.ToFullString().Trim();
                var propertyName = fieldName.TrimStart('_');
                propertyName = $"{char.ToUpper(propertyName[0])}{propertyName.Substring(1)}".Trim();
                var dependencyPropertyName = $"{propertyName.Trim()}{DependencyPropertySuffix}".Trim();
                var propertyBuilder = SyntaxBuilder.CreateProperty(fieldType, propertyName)
                    .WithModifier(Public)
                    .WithAccessor(SyntaxBuilder.CreateGetter()
                        .WithBody($"this.{fieldName} = ({fieldType})this.GetValue({dependencyPropertyName});\r\nreturn this.{fieldName};"))
                    .WithAccessor(SyntaxBuilder.CreateSetter()
                        .WithBody($"this.{fieldName} = value;\r\nthis.SetValue({dependencyPropertyName}, value);"));
                var dependencyPropertyFieldBuilder = SyntaxBuilder.CreateField(DependencyProperty, dependencyPropertyName)
                    .WithModifiers($"{Public} {Static} {Readonly}")
                    .WithInitializer($"{DependencyProperty}.Register(nameof({propertyName}), typeof({fieldType}), typeof({className}), new PropertyMetadata({initialValueArgument?.Expression.ToString()}))");

                generatedClassBuilder
                    .WithProperty(propertyBuilder)
                    .WithField(dependencyPropertyFieldBuilder);
            }

            var generatedSyntax = generatedCompilationUnit.Build();
            if (originalNamespaceSyntax.Usings.Count != 0)
            {
                generatedSyntax = generatedSyntax.AddUsings([.. originalNamespaceSyntax.Usings]);
            }
            else
            {
                if (originalNamespaceSyntax.Parent is CompilationUnitSyntax compilationUnit)
                {
                    generatedSyntax = generatedSyntax.AddUsings([.. compilationUnit.Usings]);
                }
            }

            var fileSource = generatedSyntax.ToFullString();
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