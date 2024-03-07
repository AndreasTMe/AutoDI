using Microsoft.Extensions.DependencyInjection;

using System;

namespace AutoDI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RegisterServiceAttribute : Attribute
    {
        public Type Service { get; }

        public string ServiceNamespace { get; } // TODO: Is this needed?

        public ServiceLifetime Lifetime { get; }

        public string Key { get; }

        public RegisterServiceAttribute(Type service, ServiceLifetime lifetime, string key = "")
        {
            Service = service;
            ServiceNamespace = service.Namespace ?? string.Empty;
            Lifetime = lifetime;
            Key = key;
        }
    }
}