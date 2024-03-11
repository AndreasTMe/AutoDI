using AutoDI.Attributes;

using ExampleWebAPI.Services.Abstractions;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExampleWebAPI.Services;

[InjectDependency(typeof(AbstractService), ServiceLifetime.Singleton)]
public sealed class ChildService : AbstractService
{
    public override Guid GetAbstractGuid { get; } = Guid.NewGuid();
}