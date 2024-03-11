using AutoDI.Attributes;

using ExampleWebAPI.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(IKeyedTransientService), ServiceLifetime.Transient, "2")]
public sealed class KeyedTransientService2 : IKeyedTransientService
{
    public string GetTransientGuid { get; } = $"{Guid.NewGuid()} from 2";
}