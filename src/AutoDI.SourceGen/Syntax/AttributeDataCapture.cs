namespace AutoDI.SourceGen.Syntax;

internal readonly record struct AttributeDataCapture(
    (string Name, string Namespace) Service,
    (string Name, string Namespace) Implementation,
    string Lifetime,
    string? Key);

internal static class AttributeDataCaptureExtensions
{
    public static bool IsValid(this AttributeDataCapture capture) =>
        !string.IsNullOrWhiteSpace(capture.Service.Name)
        && !string.IsNullOrWhiteSpace(capture.Implementation.Name)
        && !string.IsNullOrWhiteSpace(capture.Lifetime);
}