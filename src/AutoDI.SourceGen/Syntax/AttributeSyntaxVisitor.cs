using AutoDI.SourceGen.Internal;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoDI.SourceGen.Syntax;

internal sealed class AttributeSyntaxVisitor : CSharpSyntaxVisitor
{
    private enum AttributeArgument
    {
        Service = 0,
        Lifetime = 1,
        Key = 2
    }

    private const string IsDependency = "IsDependency";
    private const string InjectDependency = "InjectDependency";

    public AttributeDataCapture Capture { get; private set; }

    public string Namespace { get; private set; } = string.Empty;

    public override void VisitAttribute(AttributeSyntax node)
    {
        base.VisitAttribute(node);

        if (!node.Name.ToString().Equals(InjectDependency)
            && !node.Name.ToString().Equals(IsDependency))
            return;

        switch (node.Name.ToString())
        {
            case IsDependency:
                var isDepDeclaration = node.FirstAncestorOrSelf<InterfaceDeclarationSyntax>()
                    ?? (TypeDeclarationSyntax?)node.FirstAncestorOrSelf<ClassDeclarationSyntax>();
                Debug.Assert(
                    isDepDeclaration is not null,
                    "Attribute must be applied to either an interface or a class.");

                Namespace = GetFullNamespace(isDepDeclaration);

                break;
            case InjectDependency:
                var injectClassDeclaration = node.FirstAncestorOrSelf<ClassDeclarationSyntax>();
                Debug.Assert(injectClassDeclaration is not null, "Attribute must be applied to a class.");

                var arguments = node.ArgumentList?.Arguments;
                Debug.Assert(arguments is not null, "Attribute must have arguments.");

                var argumentsValues = GetAttributeArgumentsValues(arguments!.Value);

                if (!ImplementsOrIsService(injectClassDeclaration!, argumentsValues[AttributeArgument.Service]))
                    ThrowHelpers.ThrowAutoDIException(SR.ClassMustEitherImplementOrBeTheService);

                Capture = new AttributeDataCapture(
                    argumentsValues[AttributeArgument.Service],
                    injectClassDeclaration!.Identifier.ValueText,
                    argumentsValues[AttributeArgument.Lifetime],
                    argumentsValues[AttributeArgument.Key]);

                Namespace = GetFullNamespace(injectClassDeclaration);

                break;
            default:
                return;
        }
    }

    private static string GetFullNamespace(SyntaxNode? node, string currentNamespace = "")
    {
        while (true)
        {
            switch (node)
            {
                case null:
                    return currentNamespace;
                case NamespaceDeclarationSyntax namespaceDeclaration:
                    currentNamespace = currentNamespace.Length > 0
                        ? $"{namespaceDeclaration.Name}.{currentNamespace}"
                        : namespaceDeclaration.Name.ToString();

                    break;
                case FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclaration:
                    currentNamespace = currentNamespace.Length > 0
                        ? $"{fileScopedNamespaceDeclaration.Name}.{currentNamespace}"
                        : fileScopedNamespaceDeclaration.Name.ToString();

                    break;
            }

            node = node.Parent;
        }
    }

    private static Dictionary<AttributeArgument, string> GetAttributeArgumentsValues(
        SeparatedSyntaxList<AttributeArgumentSyntax> arguments)
    {
        var regularArguments = new List<string>();
        var namedArguments = Enum.GetNames(typeof(AttributeArgument))
            .ToDictionary(
                name => (AttributeArgument)Enum.Parse(typeof(AttributeArgument), name, true),
                _ => string.Empty);

        foreach (var argument in arguments)
        {
            if (argument.NameColon is not null)
            {
                var key = (AttributeArgument)Enum.Parse(
                    typeof(AttributeArgument),
                    argument.NameColon.Name.ToString(),
                    true);
                namedArguments[key] = argument.Expression.ToString();

                continue;
            }

            regularArguments.Add(argument.Expression.ToString());
        }

        for (var i = 0; i < regularArguments.Count; i++)
            namedArguments[(AttributeArgument)i] = regularArguments[i];

        namedArguments[AttributeArgument.Service] = namedArguments[AttributeArgument.Service].Split('(', ')')[1];

        return namedArguments;
    }

    private static bool ImplementsOrIsService(BaseTypeDeclarationSyntax typeDeclaration, string service) =>
        (typeDeclaration
                .BaseList is not null
            && typeDeclaration
                .BaseList
                .Types
                .Any(t => t.ToString() == service))
        || service == typeDeclaration.Identifier.Text;
}