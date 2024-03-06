using System;

namespace AutoDI.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterServiceAttribute : Attribute
{
    public RegisterServiceAttribute() { }
}