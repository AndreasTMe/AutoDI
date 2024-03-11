using AutoDI.Attributes;

using System;

namespace ExampleWebAPI.Services.Interfaces;

[IsDependency]
public interface ITransientService
{
    Guid GetTransientGuid { get; }
}