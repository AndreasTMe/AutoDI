using AutoDI.Attributes;

using ExampleWebAPI.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(KeyedAbstractService), ServiceLifetime.Singleton, "2")]
public sealed class KeyedChildService2 : KeyedAbstractService
{
    public override string GetAbstractGuid { get; } = $"{Guid.NewGuid()} from 2";
}