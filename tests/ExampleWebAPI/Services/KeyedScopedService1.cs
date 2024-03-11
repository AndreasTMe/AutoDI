using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedScopedService), ServiceLifetime.Scoped, "1")]
public sealed class KeyedScopedService1 : IKeyedScopedService
{
    public string GetScopedGuid { get; } = $"{Guid.NewGuid()} from 1";
}