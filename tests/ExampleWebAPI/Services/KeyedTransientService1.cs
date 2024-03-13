using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedTransientService), DependencyLifetime.Transient, "1")]
public sealed class KeyedTransientService1 : IKeyedTransientService
{
    public string GetTransientGuid { get; } = $"{Guid.NewGuid()} from 1";
}