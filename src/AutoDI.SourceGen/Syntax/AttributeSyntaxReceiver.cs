using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

namespace AutoDI.SourceGen.Syntax;

internal readonly record struct AttributeDataCapture(
    (string Name, string Namespace) Service,
    (string Name, string Namespace) Implementation,
    string Lifetime,
    string? Key);

internal sealed class AttributeSyntaxReceiver : ISyntaxReceiver
{
    private readonly AttributeSyntaxHandler _attributeSyntaxHandler = new();

    public List<AttributeDataCapture> Captures { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attributeSyntax)
            return;

        if (!_attributeSyntaxHandler.TryCaptureAttributeData(attributeSyntax, out var capture))
            return;

        Captures.Add(capture);
    }
}