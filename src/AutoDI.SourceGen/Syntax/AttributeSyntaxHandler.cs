using AutoDI.SourceGen.Internal;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoDI.SourceGen.Syntax;

internal readonly record struct AttributeDataCapture(
    (string Name, string Namespace) Service,
    (string Name, string Namespace) Implementation,
    string Lifetime,
    string? Key);

internal sealed class AttributeSyntaxHandler
{
    public List<AttributeDataCapture> CaptureAttributeData(
        List<AttributeSyntax> attributeSyntaxList,
        Compilation compilation)
    {
        List<AttributeDataCapture> captures = new();

        foreach (var attributeSyntax in attributeSyntaxList)
        {
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
            
            if (!ImplementsOrIsService(classDeclaration, service!))
                ThrowHelpers.ThrowAutoDIException(SR.ClassMustEitherImplementOrBeTheService);

            var lifetime = arguments?.Skip(1).FirstOrDefault()?.ToString();
            Debug.Assert(lifetime is not null, "Lifetime argument is required.");

            var key = arguments?.Skip(2).FirstOrDefault()?.ToString();

            captures.Add(
                new AttributeDataCapture(
                    (Name: service!, Namespace: ""), // TODO: Capture this namespace
                    (Name: classDeclaration.Identifier.ValueText, Namespace: classNamespace!.Name.ToString()),
                    lifetime!,
                    key));
        }

        return captures;
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