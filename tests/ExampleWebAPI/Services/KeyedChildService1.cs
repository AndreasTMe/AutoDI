using AutoDI.Attributes;

using ExampleWebAPI.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(KeyedAbstractService), ServiceLifetime.Singleton, "1")]
public sealed class KeyedChildService1 : KeyedAbstractService
{
    public override string GetAbstractGuid { get; } = $"{Guid.NewGuid()} from 1";
}