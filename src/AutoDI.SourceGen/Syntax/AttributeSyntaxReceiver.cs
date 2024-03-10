using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

namespace AutoDI.SourceGen.Syntax;

internal sealed class AttributeSyntaxReceiver : ISyntaxReceiver
{
    private readonly AttributeSyntaxVisitor _attributeSyntaxVisitor = new();

    public HashSet<AttributeDataCapture> Captures { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attributeSyntax)
            return;

        attributeSyntax.Accept(_attributeSyntaxVisitor);

        if (_attributeSyntaxVisitor.Capture.IsValid())
            Captures.Add(_attributeSyntaxVisitor.Capture);
    }
}