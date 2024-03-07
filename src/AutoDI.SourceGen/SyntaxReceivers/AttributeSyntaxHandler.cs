using AutoDI.Attributes;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Diagnostics;
using System.Linq;

namespace AutoDI.SourceGen.SyntaxReceivers;

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

        var arguments = attributeSyntax.ArgumentList?.Arguments;
        Debug.Assert(arguments is not null, "Attribute must have arguments.");

        var service = arguments?.FirstOrDefault()
            ?.ToString()
            .Replace("typeof(", string.Empty)
            .Replace(")", string.Empty);
        Debug.Assert(service is not null, "Service argument is required.");

        var lifetime = arguments?.Skip(1).FirstOrDefault()?.ToString();
        Debug.Assert(lifetime is not null, "Lifetime argument is required.");

        var key = arguments?.Skip(2).FirstOrDefault()?.ToString();

        capture = new AttributeDataCapture(service!, classDeclaration!.Identifier.Text, lifetime!, key);

        return true;
    }
}