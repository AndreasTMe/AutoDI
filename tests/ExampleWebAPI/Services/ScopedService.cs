using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IScopedService), ServiceLifetime.Scoped)]
public sealed class ScopedService : IScopedService
{
    public Guid GetScopedGuid { get; } = Guid.NewGuid();
}