using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace AutoDI.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class InjectDependencySyntaxAnalyzer : DiagnosticAnalyzer
{
    private const string InjectDependency = "InjectDependency";

    private const string DiagnosticId = "AutoDI0001";

    private static readonly LocalizableString Title = new LocalizableResourceString(
        nameof(Resources.AutoDI0001Title),
        Resources.ResourceManager,
        typeof(Resources));

    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(
            nameof(Resources.AutoDI0001MessageFormat),
            Resources.ResourceManager,
            typeof(Resources));

    private const string Category = "AutoDI";

    private static readonly LocalizableString Description =
        new LocalizableResourceString(
            nameof(Resources.AutoDI0001Description),
            Resources.ResourceManager,
            typeof(Resources));

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.Attribute);
    }

    private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AttributeSyntax attributeSyntax)
            return;

        if (!attributeSyntax.Name.ToString().Equals(InjectDependency))
            return;

        Debug.Assert(attributeSyntax.ArgumentList is not null, "Attribute must have arguments.");
        Debug.Assert(attributeSyntax.ArgumentList!.Arguments.Count >= 2, "Attribute must have at least two arguments.");

        var typeArgument = attributeSyntax.ArgumentList!.Arguments
            .FirstOrDefault(argument => argument.Expression.ToString().Contains("typeof"))
            ?.Expression.ToString()
            .Replace("typeof(", string.Empty)
            .Replace(")", string.Empty);

        Debug.Assert(typeArgument is not null, "Attribute must have a type argument.");

        var classDeclarationSyntax = attributeSyntax.FirstAncestorOrSelf<ClassDeclarationSyntax>();
        Debug.Assert(classDeclarationSyntax is not null, "Attribute must be applied to a class.");

        if (ImplementsOrIsService(classDeclarationSyntax!, typeArgument!))
            return;

        var diagnostic = Diagnostic.Create(
            Rule,
            attributeSyntax.GetLocation(),
            classDeclarationSyntax!.Identifier.Text,
            typeArgument);

        context.ReportDiagnostic(diagnostic);
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