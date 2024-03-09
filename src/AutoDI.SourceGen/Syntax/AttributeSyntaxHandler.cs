using AutoDI.Attributes;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Diagnostics;
using System.Linq;

namespace AutoDI.SourceGen.Syntax;

internal sealed class AttributeSyntaxHandler
{
    private readonly string _attributeName;

    public AttributeSyntaxHandler() =>
        _attributeName = nameof(RegisterServiceAttribute).Replace(nameof(Attribute), string.Empty);

    public bool TryCaptureAttributeData(AttributeSyntax attributeSyntax, out AttributeDataCapture capture)
    {
        if (!attributeSyntax.Name.ToString().StartsWith(_attributeName))
        {
            capture = default;

            return false;
        }

        var classDeclaration = attributeSyntax.FirstAncestorOrSelf<ClassDeclarationSyntax>();
        Debug.Assert(classDeclaration is not null, "Attribute must be applied to a class.");

        var classNamespace = classDeclaration!.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
        Debug.Assert(classNamespace is not null, "Class must be in a namespace.");

        var arguments = attributeSyntax.ArgumentList?.Arguments;
        Debug.Assert(arguments is not null, "Attribute must have arguments.");

        var service = arguments?.FirstOrDefault()
            ?.ToString()
            .Replace("typeof(", string.Empty)
            .Replace(")", string.Empty);
        Debug.Assert(service is not null, "Service argument is required.");
        Debug.Assert(
            ImplementsOrIsService(classDeclaration, service!),
            "Class must either implement the service or be the service.");

        var lifetime = arguments?.Skip(1).FirstOrDefault()?.ToString();
        Debug.Assert(lifetime is not null, "Lifetime argument is required.");

        var key = arguments?.Skip(2).FirstOrDefault()?.ToString();

        capture = new AttributeDataCapture(
            (Name: service!, Namespace: ""), // TODO: Capture this namespace
            (Name: classDeclaration.Identifier.ValueText, Namespace: classNamespace!.Name.ToString()),
            lifetime!,
            key);

        return true;
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