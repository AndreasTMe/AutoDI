using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutoDI.SourceGen.SyntaxReceivers;

internal sealed class AttributeSyntaxReceiver
{
    private const string AttributeName = "RegisterService";

    public List<Capture> Captures { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attributeSyntax
            || !attributeSyntax.Name.ToString().StartsWith(AttributeName))
            return;

        var classDeclaration = attributeSyntax.FirstAncestorOrSelf<ClassDeclarationSyntax>();

        var serviceName = attributeSyntax.Name.ToString().Split('<', '>')[1];

        var lifetime = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault()?.ToString();
        Debug.Assert(lifetime is not null, "Lifetime argument is required.");

        var key = attributeSyntax.ArgumentList?.Arguments.Skip(1).FirstOrDefault()?.ToString();

        Captures.Add(new Capture(serviceName, classDeclaration!.Identifier.Text, lifetime!, key));
    }

    public readonly record struct Capture(string Service, string Implementation, string Lifetime, string? Key);
}