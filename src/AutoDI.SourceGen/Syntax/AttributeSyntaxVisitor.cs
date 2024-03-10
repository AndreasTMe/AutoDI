using AutoDI.Attributes;
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
        ServiceNamespace = 2,
        Key = 3
    }

    public AttributeDataCapture Capture { get; private set; }

    public override void VisitAttribute(AttributeSyntax node)
    {
        base.VisitAttribute(node);

        if (!IsCorrectAttribute(node))
            return;

        var classDeclaration = node.FirstAncestorOrSelf<ClassDeclarationSyntax>();
        Debug.Assert(classDeclaration is not null, "Attribute must be applied to a class.");

        var classNamespace = classDeclaration!.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
        Debug.Assert(classNamespace is not null, "Class must be in a namespace.");

        var arguments = node.ArgumentList?.Arguments;
        Debug.Assert(arguments is not null, "Attribute must have arguments.");

        var argumentsValues = GetAttributeArgumentsValues(arguments!.Value);

        if (!ImplementsOrIsService(classDeclaration, argumentsValues[AttributeArgument.Service]))
            ThrowHelpers.ThrowAutoDIException(SR.ClassMustEitherImplementOrBeTheService);

        Capture = new AttributeDataCapture(
            (Name: argumentsValues[AttributeArgument.Service],
                Namespace: argumentsValues[AttributeArgument.ServiceNamespace]),
            (Name: classDeclaration.Identifier.ValueText, Namespace: classNamespace!.Name.ToString()),
            argumentsValues[AttributeArgument.Lifetime],
            argumentsValues[AttributeArgument.Key]);
    }

    private static bool IsCorrectAttribute(AttributeSyntax node) =>
        string.Join("", node.Name.ToString().Concat(nameof(Attribute))) == nameof(InjectServiceAttribute);

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
        namedArguments[AttributeArgument.ServiceNamespace] =
            namedArguments[AttributeArgument.ServiceNamespace].Replace("\"", string.Empty);

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