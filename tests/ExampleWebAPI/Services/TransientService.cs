using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(ITransientService), DependencyLifetime.Transient)]
public sealed class TransientService : ITransientService
{
    public Guid GetTransientGuid { get; } = Guid.NewGuid();
}