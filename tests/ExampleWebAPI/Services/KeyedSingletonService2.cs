using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedSingletonService), ServiceLifetime.Singleton, "2")]
public sealed class KeyedSingletonService2 : IKeyedSingletonService
{
    public string GetSingletonGuid { get; } = $"{Guid.NewGuid()} from 2";
}