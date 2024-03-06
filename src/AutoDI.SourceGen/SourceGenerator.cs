using AutoDI.SourceGen.SyntaxReceivers;

using Microsoft.CodeAnalysis;

namespace AutoDI.SourceGen;

[Generator]
internal class SourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxReceiver as MainSyntaxReceiver;

        // TODO: Add code generation logic here
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
    }
}