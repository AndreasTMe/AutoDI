using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedSingletonService), DependencyLifetime.Singleton, "1")]
public sealed class KeyedSingletonService1 : IKeyedSingletonService
{
    public string GetSingletonGuid { get; } = $"{Guid.NewGuid()} from 1";
}