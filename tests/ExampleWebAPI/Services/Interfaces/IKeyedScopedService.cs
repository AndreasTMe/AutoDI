using AutoDI.Attributes;

namespace ExampleWebAPI.Services.Interfaces;

[IsDependency]
public interface IKeyedScopedService
{
    string GetScopedGuid { get; }
}