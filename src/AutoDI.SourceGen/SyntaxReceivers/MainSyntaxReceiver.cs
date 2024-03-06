using Microsoft.CodeAnalysis;

namespace AutoDI.SourceGen.SyntaxReceivers;

internal sealed class MainSyntaxReceiver : ISyntaxReceiver
{
    private readonly AttributeSyntaxReceiver _attributeSyntaxReceiver = new();
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        _attributeSyntaxReceiver.OnVisitSyntaxNode(syntaxNode);
        
        var captures = _attributeSyntaxReceiver.Captures;
    }
}