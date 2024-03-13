using AutoDI.SourceGen.Syntax;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDI.SourceGen.Generators;

[Generator]
internal class ServiceCollectionExtensionsGenerator : ISourceGenerator
{
    private const string ExtensionsFileName = "AutoDIServiceCollectionExtensions.g.cs";

    private const string SingletonTemplate = "services.AddSingleton<{0}>();";
    private const string ScopedTemplate = "services.AddScoped<{0}>();";
    private const string TransientTemplate = "services.AddTransient<{0}>();";

    private const string KeyedSingletonTemplate = "services.AddKeyedSingleton<{0}>({1});";
    private const string KeyedScopedTemplate = "services.AddKeyedScoped<{0}>({1});";
    private const string KeyedTransientTemplate = "services.AddKeyedTransient<{0}>({1});";

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not AttributeSyntaxReceiver receiver)
            return;

        var assemblyName = context.Compilation.AssemblyName;

        if (string.IsNullOrEmpty(assemblyName))
            return;

        var usings = new HashSet<string>
        {
            "Microsoft.Extensions.DependencyInjection"
        };

        foreach (var @namespace in receiver.Namespaces.Where(n => !string.IsNullOrEmpty(n)))
            usings.Add(@namespace);

        var builder = new StringBuilder();

        foreach (var @using in usings.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x))
            builder.AppendLine($"using {@using};");

        builder.AppendLine();
        builder.AppendLine($"namespace {assemblyName}");
        builder.AppendLine("{");
        builder.AppendLine("\tpublic static class AutoDI_ServiceCollectionExtensions");
        builder.AppendLine("\t{");
        builder.AppendLine("\t\tpublic static void AddAutoDI(this IServiceCollection services)");
        builder.AppendLine("\t\t{");

        foreach (var (service, implementation, lifetime, key) in receiver.Captures)
        {
#pragma warning disable CS8509
            var template = string.IsNullOrEmpty(key)
                ? lifetime switch
                {
                    "DependencyLifetime.Singleton" => SingletonTemplate,
                    "DependencyLifetime.Transient" => TransientTemplate,
                    "DependencyLifetime.Scoped"    => ScopedTemplate,
                }
                : lifetime switch
                {
                    "DependencyLifetime.Singleton" => KeyedSingletonTemplate,
                    "DependencyLifetime.Transient" => KeyedTransientTemplate,
                    "DependencyLifetime.Scoped"    => KeyedScopedTemplate,
                };
#pragma warning restore CS8509

            var genericArguments = service == implementation
                ? service
                : $"{service}, {implementation}";
            var line = string.IsNullOrEmpty(key)
                ?string.Format(template, genericArguments)
                :string.Format(template, genericArguments, key);

            builder.AppendLine($"\t\t\t{line}");
        }

        builder.AppendLine("\t\t}");
        builder.AppendLine("\t}");
        builder.AppendLine("}");

        context.AddSource(ExtensionsFileName, builder.ToString());
    }
}