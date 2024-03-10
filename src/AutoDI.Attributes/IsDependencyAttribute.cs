using System;

namespace AutoDI.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false)]
    public sealed class IsDependencyAttribute : Attribute;
}