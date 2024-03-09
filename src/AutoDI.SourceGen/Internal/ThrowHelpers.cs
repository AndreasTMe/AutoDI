namespace AutoDI.SourceGen.Internal;

internal static class ThrowHelpers
{
    // ReSharper disable once InconsistentNaming
    public static void ThrowAutoDIException(string message) => throw new AutoDIException(message);
}