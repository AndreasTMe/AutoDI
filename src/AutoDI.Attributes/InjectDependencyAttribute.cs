// ReSharper disable UnusedParameter.Local

using System;

namespace AutoDI.Attributes
{
    public enum DependencyLifetime
    {
        Singleton,
        Transient,
        Scoped
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class InjectDependencyAttribute : Attribute
    {
        public InjectDependencyAttribute(Type service, DependencyLifetime lifetime, string key = "") { }
    }
}