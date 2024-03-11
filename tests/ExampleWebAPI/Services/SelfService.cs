using AutoDI.Attributes;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(SelfService), ServiceLifetime.Singleton)]
public sealed class SelfService
{
    public Guid GetSelfGuid { get; } = Guid.NewGuid();
}