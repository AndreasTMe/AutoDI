using AutoDI.Attributes;

using System;

namespace ExampleWebAPI.Services.Interfaces;

[IsDependency]
public interface IScopedService
{
    Guid GetScopedGuid { get; }
}