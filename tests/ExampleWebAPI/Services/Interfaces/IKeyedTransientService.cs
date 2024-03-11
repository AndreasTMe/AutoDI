using AutoDI.Attributes;

namespace ExampleWebAPI.Services.Interfaces;

[IsDependency]
public interface IKeyedTransientService
{
    string GetTransientGuid { get; }
}