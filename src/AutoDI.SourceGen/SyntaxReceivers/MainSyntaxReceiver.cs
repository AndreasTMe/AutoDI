using Microsoft.CodeAnalysis;

namespace AutoDI.SourceGen.SyntaxReceivers;

internal sealed class MainSyntaxReceiver : ISyntaxReceiver
{
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        // TODO: Capture attribute data
    }
}