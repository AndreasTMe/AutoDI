using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(ISingletonService), ServiceLifetime.Singleton)]
public sealed class SingletonService : ISingletonService
{
    public Guid GetSingletonGuid { get; } = Guid.NewGuid();
}