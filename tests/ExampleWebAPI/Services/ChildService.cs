using AutoDI.Attributes;

using ExampleWebAPI.Services.Abstractions;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(AbstractService), DependencyLifetime.Singleton)]
public sealed class ChildService : AbstractService
{
    public override Guid GetAbstractGuid { get; } = Guid.NewGuid();
}