using AutoDI.Attributes;

using System;

namespace ExampleWebAPI.Services.Interfaces;

[IsDependency]
public interface ISingletonService
{
    Guid GetSingletonGuid { get; }
}