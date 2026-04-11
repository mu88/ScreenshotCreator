using System.Diagnostics.CodeAnalysis;

namespace ScreenshotCreator.Api;

[ExcludeFromCodeCoverage]
internal static class ValidationErrorMessages
{
    public static readonly string[] WidthMustBeGreaterThanZero = new[] { "Width must be greater than 0." };
    public static readonly string[] HeightMustBeGreaterThanZero = new[] { "Height must be greater than 0." };
}
