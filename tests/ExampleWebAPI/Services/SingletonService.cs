using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(ISingletonService), DependencyLifetime.Singleton)]
public sealed class SingletonService : ISingletonService
{
    public Guid GetSingletonGuid { get; } = Guid.NewGuid();
}