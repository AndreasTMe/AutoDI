using Microsoft.Extensions.DependencyInjection;

using System;

namespace AutoDI.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterServiceAttribute<TService> : Attribute
    where TService : class
{
    public ServiceLifetime Lifetime { get; }
    
    public string Key { get; }

    public RegisterServiceAttribute(ServiceLifetime lifetime, string key = "")
    {
        Lifetime = lifetime;
        Key = key;
    }
}