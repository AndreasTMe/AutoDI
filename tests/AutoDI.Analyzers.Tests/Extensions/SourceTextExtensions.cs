// Many thanks to Denis Ekart and their awesome blog post:
// https://denace.dev/testing-roslyn-analyzers-and-code-fixes

using AutoDI.Attributes;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace AutoDI.Analyzers.Tests.Extensions;

internal static class SourceTextExtensions
{
    private const string DefaultProjectName = "TestProject.csproj";
    private const string DefaultDocumentName = "TestDocument.cs";

    private static readonly MetadataReference[] CommonReferences =
    [
        MetadataReference.CreateFromFile(typeof(InjectDependencyAttribute).Assembly.Location)
    ];

    public static async Task<ImmutableArray<Diagnostic>> GetDiagnostics<TAnalyzer>(this SourceText source)
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        var project = source.CreateWorkspace()
            .CurrentSolution
            .Projects
            .First(x => x.Name == DefaultProjectName);

        var compilation = await project.GetCompilationAsync();

        if (compilation is null)
            throw new InvalidOperationException("Could not get compilation.");

        DiagnosticAnalyzer analyzer = new TAnalyzer();

        var allDiagnostics = await compilation.WithAnalyzers(ImmutableArray.Create(analyzer)).GetAllDiagnosticsAsync();
        var document = project.Documents.First(x => x.Name == DefaultDocumentName);

        return allDiagnostics
            .Where(
                x => x.Id == analyzer.SupportedDiagnostics.Single().Id
                    && x.Location.SourceTree?.FilePath == document.Name)
            .ToImmutableArray();
    }

    private static AdhocWorkspace CreateWorkspace(this SourceText source)
    {
        var projectId = ProjectId.CreateNewId();
        var documentId = DocumentId.CreateNewId(projectId, DefaultDocumentName);

        var sourceTextLoader = TextLoader.From(TextAndVersion.Create(source, VersionStamp.Create()));
        var document = DocumentInfo
            .Create(documentId, DefaultDocumentName)
            .WithTextLoader(sourceTextLoader);

        var project = ProjectInfo.Create(
                id: projectId,
                version: VersionStamp.Create(),
                name: DefaultProjectName,
                assemblyName: DefaultProjectName,
                language: LanguageNames.CSharp)
            .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var workspace = new AdhocWorkspace();
        var updatedSolution = workspace
            .CurrentSolution
            .AddProject(project)
            .AddMetadataReferences(projectId, CommonReferences)
            .AddDocument(document);

        workspace.TryApplyChanges(updatedSolution);

        return workspace;
    }
}