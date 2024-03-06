using AutoDI.SourceGen.SyntaxReceivers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace AutoDI.SourceGen;

[Generator]
internal class SourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = context.SyntaxReceiver as MainSyntaxReceiver;

        // TODO: Add code generation logic here

        // Temporary
        var source = CompilationUnit()
            .WithUsings(
                SingletonList(
                    UsingDirective(
                        QualifiedName(
                            QualifiedName(
                                IdentifierName("Microsoft"),
                                IdentifierName("Extensions")),
                            IdentifierName("DependencyInjection")))))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    NamespaceDeclaration(
                            QualifiedName(
                                IdentifierName("AutoDI"),
                                IdentifierName("Extensions")))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                ClassDeclaration("AutoDI_ServiceCollectionExtensions")
                                    .WithModifiers(
                                        TokenList(
                                            new[]
                                            {
                                                Token(SyntaxKind.PublicKeyword),
                                                Token(SyntaxKind.StaticKeyword)
                                            }))
                                    .WithMembers(
                                        SingletonList<MemberDeclarationSyntax>(
                                            MethodDeclaration(
                                                    PredefinedType(Token(SyntaxKind.VoidKeyword)),
                                                    Identifier("AutoInject"))
                                                .WithModifiers(
                                                    TokenList(
                                                    [
                                                        Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)
                                                    ]))
                                                .WithParameterList(
                                                    ParameterList(
                                                        SingletonSeparatedList<ParameterSyntax>(
                                                            Parameter(Identifier("services"))
                                                                .WithModifiers(TokenList(Token(SyntaxKind.ThisKeyword)))
                                                                .WithType(IdentifierName("IServiceCollection")))))
                                                .WithBody(Block())))))))
            .NormalizeWhitespace();

        context.AddSource("AutoDIServiceCollectionExtensions.g.cs", source.NormalizeWhitespace().ToString());
    }
}