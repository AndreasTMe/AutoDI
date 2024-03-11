using AutoDI.Attributes;

namespace ExampleWebAPI.Services.Abstractions;

[IsDependency]
public abstract class KeyedAbstractService
{
    public abstract string GetAbstractGuid { get; }
}