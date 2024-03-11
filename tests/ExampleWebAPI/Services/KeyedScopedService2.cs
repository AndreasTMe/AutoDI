using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedScopedService), ServiceLifetime.Scoped, "2")]
public sealed class KeyedScopedService2 : IKeyedScopedService
{
    public string GetScopedGuid { get; } = $"{Guid.NewGuid()} from 2";
}