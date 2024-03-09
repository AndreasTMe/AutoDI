using System;

namespace AutoDI.SourceGen.Internal;

// ReSharper disable once InconsistentNaming
internal sealed class AutoDIException : Exception
{
    public AutoDIException(string message) : base(message) { }
}