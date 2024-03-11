using AutoDI.Attributes;

using System;

namespace ExampleWebAPI.Services.Abstractions;

[IsDependency]
public abstract class AbstractService
{
    public abstract Guid GetAbstractGuid { get; }
}