using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedSingletonService), DependencyLifetime.Singleton, "2")]
public sealed class KeyedSingletonService2 : IKeyedSingletonService
{
    public string GetSingletonGuid { get; } = $"{Guid.NewGuid()} from 2";
}