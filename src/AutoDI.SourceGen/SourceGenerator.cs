using AutoDI.SourceGen.Syntax;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDI.SourceGen;

[Generator]
internal class SourceGenerator : ISourceGenerator
{
    private const string _singletonTemplate = "services.AddSingleton<{0}>({1});";
    private const string _scopedTemplate = "services.AddScoped<{0}>({1});";
    private const string _transientTemplate = "services.AddTransient<{0}>({1});";

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
        builder.AppendLine("    public static class AutoDI_ServiceCollectionExtensions");
        builder.AppendLine("    {");
        builder.AppendLine("        public static void AddAutoDI(this IServiceCollection services)");
        builder.AppendLine("        {");

        foreach (var (service, implementation, lifetime, key) in receiver.Captures)
        {
            var template = lifetime switch
            {
                "ServiceLifetime.Singleton" => _singletonTemplate,
                "ServiceLifetime.Transient" => _transientTemplate,
                "ServiceLifetime.Scoped"    => _scopedTemplate,
                _                           => throw new ArgumentException("Invalid lifetime.")
            };

            var genericArguments = service.Name == implementation.Name
                ? service.Name
                : $"{service.Name}, {implementation.Name}";
            var line = string.Format(template, genericArguments, key ?? "");

            builder.AppendLine($"            {line}");
        }

        builder.AppendLine("        }");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource("AutoDIServiceCollectionExtensions.g.cs", builder.ToString());
    }
}