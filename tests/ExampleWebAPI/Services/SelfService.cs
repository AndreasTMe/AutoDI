using AutoDI.Attributes;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(SelfService), DependencyLifetime.Singleton)]
public sealed class SelfService
{
    public Guid GetSelfGuid { get; } = Guid.NewGuid();
}