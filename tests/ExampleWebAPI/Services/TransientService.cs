using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(ITransientService), ServiceLifetime.Transient)]
public sealed class TransientService : ITransientService
{
    public Guid GetTransientGuid { get; } = Guid.NewGuid();
}