using Microsoft.Extensions.Logging;

namespace ScreenshotCreator.Logic;

internal static partial class Log
{
    [LoggerMessage(EventId = 0,
        EventName = Prefix + nameof(PlaywrightInitialized),
        Level = LogLevel.Information,
        SkipEnabledCheck = true,
        Message = "Playwright initialized")]
    public static partial void PlaywrightInitialized(this ILogger logger);

    [LoggerMessage(EventId = 1,
        EventName = Prefix + nameof(ScreenshotCreated),
        Level = LogLevel.Information,
        SkipEnabledCheck = true,
        Message = "Screenshot created")]
    public static partial void ScreenshotCreated(this ILogger logger);

    [LoggerMessage(EventId = 2,
        EventName = Prefix + nameof(ReusingPlaywrightPage),
        Level = LogLevel.Information,
        SkipEnabledCheck = true,
        Message = "Reusing Playwright page")]
    public static partial void ReusingPlaywrightPage(this ILogger logger);

    [LoggerMessage(EventId = 3,
        EventName = Prefix + nameof(LoginNecessaryCheck),
        Level = LogLevel.Information,
        SkipEnabledCheck = true,
        Message = "Check if login is necessary was {loginIsNecessary}")]
    public static partial void LoginNecessaryCheck(this ILogger logger, bool loginIsNecessary);

    [LoggerMessage(EventId = 4,
        EventName = Prefix + nameof(LoggingIn),
        Level = LogLevel.Information,
        SkipEnabledCheck = true,
        Message = "Logging in")]
    public static partial void LoggingIn(this ILogger logger);

    [LoggerMessage(EventId = 5,
        EventName = Prefix + nameof(LoginNotSupported),
        Level = LogLevel.Debug,
        Message = "Login is not supported for URL type {urlType}")]
    public static partial void LoginNotSupported(this ILogger logger, UrlType urlType);

    [LoggerMessage(EventId = 6,
        EventName = Prefix + nameof(InvalidDimensions),
        Level = LogLevel.Warning,
        SkipEnabledCheck = true,
        Message =
            "The image's dimensions are invalid. Width: {currentWidth} (expected {expectedWidth}); Height: {currentHeight} (expected {expectedHeight})")]
    public static partial void InvalidDimensions(this ILogger logger, int currentWidth, int expectedWidth, int currentHeight, int expectedHeight);

    [LoggerMessage(EventId = 7,
        EventName = Prefix + nameof(ScreenshotCreationFailed),
        Level = LogLevel.Error,
        SkipEnabledCheck = true,
        Message = "Screenshot creation failed")]
    public static partial void ScreenshotCreationFailed(this ILogger logger, Exception exception);
}
