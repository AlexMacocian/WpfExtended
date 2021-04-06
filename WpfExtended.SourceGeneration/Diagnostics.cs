﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace System.Extensions
{
    internal static class Diagnostics
    {
        public const string DependencyPropertyGenerator_NoAttributeFound = "DGD 0001";
        public const string DependencyPropertyGenerator_ClassMustBeTopLevel = "DGD 1001";
        public const string DependencyPropertyGenerator_Success = "DGD 3001";

        public static Diagnostic MissingAttributeDiagnostic(string attributeName, string attributeNamespace) => Diagnostic.Create(
                    new DiagnosticDescriptor(
                        DependencyPropertyGenerator_NoAttributeFound,
                        $"{attributeNamespace}.{attributeName} not found",
                        "Could not find attribute with name {0} in namespace {1}.",
                        "WpfExtended.SourceGeneration",
                        DiagnosticSeverity.Error,
                        true,
                        $"This error occurs when the attribute generated by the {nameof(DependencyPropertyGenerator)} is not found",
                        null),
                    Location.None,
                    attributeName, attributeNamespace);

        public static Diagnostic ClassNotTopLevelDiagnostic(string className, string containingSymbol, SyntaxTree syntaxTree, TextSpan textSpan) => Diagnostic.Create(
                    new DiagnosticDescriptor(
                        DependencyPropertyGenerator_ClassMustBeTopLevel,
                        $"{className} must be top level",
                        "Class {0} must be top level. It is currently declared under {1}.",
                        "WpfExtended.SourceGeneration",
                        DiagnosticSeverity.Error,
                        true,
                        null,
                        null),
                    Location.Create(syntaxTree, textSpan),
                    className, containingSymbol);

        public static Diagnostic GeneratedSymbolsForClassDiagnostic(string className) => Diagnostic.Create(
            new DiagnosticDescriptor(
                DependencyPropertyGenerator_Success,
                $"{className} created dependency properties",
                "DependencyPropertyGenerator successfully created implementations for properties in {0}",
                "WpfExtended.SourceGeneration",
                DiagnosticSeverity.Info,
                true,
                null,
                null),
            Location.None,
            className);
    }
}
