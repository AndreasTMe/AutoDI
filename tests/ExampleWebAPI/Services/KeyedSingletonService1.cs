using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedSingletonService), ServiceLifetime.Singleton, "1")]
public sealed class KeyedSingletonService1 : IKeyedSingletonService
{
    public string GetSingletonGuid { get; } = $"{Guid.NewGuid()} from 1";
}