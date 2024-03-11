using AutoDI.Attributes;

namespace ExampleWebAPI.Services.Interfaces;

[IsDependency]
public interface IKeyedSingletonService
{
    string GetSingletonGuid { get; }
}