using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedTransientService), DependencyLifetime.Transient, "2")]
public sealed class KeyedTransientService2 : IKeyedTransientService
{
    public string GetTransientGuid { get; } = $"{Guid.NewGuid()} from 2";
}