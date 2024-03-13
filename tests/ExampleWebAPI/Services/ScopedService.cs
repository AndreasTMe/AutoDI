using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IScopedService), DependencyLifetime.Scoped)]
public sealed class ScopedService : IScopedService
{
    public Guid GetScopedGuid { get; } = Guid.NewGuid();
}