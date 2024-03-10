using AutoDI.Attributes;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;

namespace AutoDI.SourceGen.Syntax;

internal sealed class AttributeSyntaxReceiver : ISyntaxReceiver
{
    private readonly string _attributeName = nameof(RegisterServiceAttribute).Replace(nameof(Attribute), string.Empty);

    public List<AttributeSyntax> Attributes { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attributeSyntax
            || !attributeSyntax.Name.ToString().StartsWith(_attributeName))
            return;

        Attributes.Add(attributeSyntax);
    }
}