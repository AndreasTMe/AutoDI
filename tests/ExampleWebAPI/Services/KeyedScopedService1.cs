using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedScopedService), DependencyLifetime.Scoped, "1")]
public sealed class KeyedScopedService1 : IKeyedScopedService
{
    public string GetScopedGuid { get; } = $"{Guid.NewGuid()} from 1";
}