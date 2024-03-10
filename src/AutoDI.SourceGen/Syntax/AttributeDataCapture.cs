namespace AutoDI.SourceGen.Syntax;

internal readonly record struct AttributeDataCapture(
    string Service,
    string Implementation,
    string Lifetime,
    string? Key);

internal static class AttributeDataCaptureExtensions
{
    public static bool IsValid(this AttributeDataCapture capture) =>
        !string.IsNullOrWhiteSpace(capture.Service)
        && !string.IsNullOrWhiteSpace(capture.Implementation)
        && !string.IsNullOrWhiteSpace(capture.Lifetime);
}