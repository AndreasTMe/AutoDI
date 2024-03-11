// ReSharper disable UnusedParameter.Local

using Microsoft.Extensions.DependencyInjection;

using System;

namespace AutoDI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class InjectDependencyAttribute : Attribute
    {
        public InjectDependencyAttribute(Type service, ServiceLifetime lifetime, string key = "") { }
    }
}