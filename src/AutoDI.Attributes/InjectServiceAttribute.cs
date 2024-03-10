using Microsoft.Extensions.DependencyInjection;

using System;

namespace AutoDI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class InjectServiceAttribute : Attribute
    {
        public Type Service { get; }

        public ServiceLifetime Lifetime { get; }

        public string ServiceNamespace { get; }

        public string Key { get; }

        public InjectServiceAttribute(
            Type service,
            ServiceLifetime lifetime,
            string serviceNamespace = "",
            string key = "")
        {
            Service = service;
            Lifetime = lifetime;
            ServiceNamespace = serviceNamespace;
            Key = key;
        }
    }
}