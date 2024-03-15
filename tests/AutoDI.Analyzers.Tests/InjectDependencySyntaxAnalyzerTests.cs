using AutoDI.Analyzers.Tests.Extensions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using System.Threading.Tasks;

using Xunit;

namespace AutoDI.Analyzers.Tests;

public class InjectDependencySyntaxAnalyzerTests
{
    [Fact]
    public async Task InjectDependency_WhenServiceImplementsInterface_ReturnsNoDiagnostics()
    {
        var sourceText = SourceText.From(
            """
            using AutoDI.Attributes;

            namespace MyNamespace;

            [IsDependency]
            public interface IMyService
            {
                void DoSomething();
            }

            [InjectDependency(typeof(IMyService), DependencyLifetime.Singleton)]
            public class MyService : IMyService
            {
                public void DoSomething()
                {
                    // Do something
                }
            }
            """);

        var actual = await sourceText.GetDiagnostics<InjectDependencySyntaxAnalyzer>();

        Assert.Empty(actual);
    }

    [Fact]
    public async Task InjectDependency_WhenServiceDoesNotImplementInterface_ReturnsOneDiagnostic()
    {
        var sourceText = SourceText.From(
            """
            using AutoDI.Attributes;

            namespace MyNamespace;

            [IsDependency]
            public interface IMyService
            {
                void DoSomething();
            }

            [InjectDependency(typeof(IMyService), DependencyLifetime.Singleton)]
            public class MyService
            {
                public void DoSomething()
                {
                    // Do something
                }
            }
            """);

        var actual = await sourceText.GetDiagnostics<InjectDependencySyntaxAnalyzer>();

        Assert.Single(actual);
        Assert.Equal(InjectDependencySyntaxAnalyzer.DiagnosticId, actual[0].Id);
        Assert.Equal(DiagnosticSeverity.Error, actual[0].Severity);
    }
}