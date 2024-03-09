using AutoDI.SourceGen.Syntax;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDI.SourceGen;

[Generator]
internal class SourceGenerator : ISourceGenerator
{
    private const string SingletonTemplate = "services.AddSingleton<{0}>({1});";
    private const string ScopedTemplate = "services.AddScoped<{0}>({1});";
    private const string TransientTemplate = "services.AddTransient<{0}>({1});";

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not AttributeSyntaxReceiver receiver)
            return;

        var usings = new HashSet<string>
        {
            "Microsoft.Extensions.DependencyInjection"
        };

        foreach (var (service, implementation, _, _) in receiver.Captures)
        {
            if (!string.IsNullOrEmpty(service.Namespace))
                usings.Add(service.Namespace);

            if (!string.IsNullOrEmpty(implementation.Namespace))
                usings.Add(implementation.Namespace);
        }

        var builder = new StringBuilder();

        foreach (var @using in usings.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x))
            builder.AppendLine($"using {@using};");

        builder.AppendLine();
        builder.AppendLine("namespace AutoDI.Extensions");
        builder.AppendLine("{");
        builder.AppendLine("\tpublic static class AutoDI_ServiceCollectionExtensions");
        builder.AppendLine("\t{");
        builder.AppendLine("\t\tpublic static void AddAutoDI(this IServiceCollection services)");
        builder.AppendLine("\t\t{");

        foreach (var (service, implementation, lifetime, key) in receiver.Captures)
        {
#pragma warning disable CS8509
            var template = lifetime switch
            {
                "ServiceLifetime.Singleton" => SingletonTemplate,
                "ServiceLifetime.Transient" => TransientTemplate,
                "ServiceLifetime.Scoped"    => ScopedTemplate,
            };
#pragma warning restore CS8509

            var genericArguments = service.Name == implementation.Name
                ? service.Name
                : $"{service.Name}, {implementation.Name}";
            var line = string.Format(template, genericArguments, key ?? "");

            builder.AppendLine($"\t\t\t{line}");
        }

        builder.AppendLine("\t\t}");
        builder.AppendLine("\t}");
        builder.AppendLine("}");

        context.AddSource("AutoDIServiceCollectionExtensions.g.cs", builder.ToString());
    }
}