using Microsoft.Extensions.Logging;

namespace ScreenshotCreator.Logic;

public static partial class Log
{
    private const string Prefix = nameof(Logic);

    [LoggerMessage(EventId = 0,
                      EventName = Prefix + nameof(PlaywrightInitialized),
                      Level = LogLevel.Information,
                      Message = "Playwright initialized")]
    public static partial void PlaywrightInitialized(this ILogger logger);

    [LoggerMessage(EventId = 1,
                      EventName = Prefix + nameof(PlaywrightInitialized),
                      Level = LogLevel.Information,
                      Message = "Screenshot created")]
    public static partial void ScreenshotCreated(this ILogger logger);

    [LoggerMessage(EventId = 2,
                      EventName = Prefix + nameof(ReusingPlaywrightPage),
                      Level = LogLevel.Information,
                      Message = "Reusing Playwright page")]
    public static partial void ReusingPlaywrightPage(this ILogger logger);

    [LoggerMessage(EventId = 3,
                      EventName = Prefix + nameof(LoginNecessaryCheck),
                      Level = LogLevel.Information,
                      Message = "Check if login is necessary was {loginIsNecessary}")]
    public static partial void LoginNecessaryCheck(this ILogger logger, bool loginIsNecessary);

    [LoggerMessage(EventId = 4,
                      EventName = Prefix + nameof(LoggingIn),
                      Level = LogLevel.Information,
                      Message = "Logging in")]
    public static partial void LoggingIn(this ILogger logger);

    [LoggerMessage(EventId = 5,
                      EventName = Prefix + nameof(LoginNotSupported),
                      Level = LogLevel.Warning,
                      Message = "Login is not supported for URL type {urlType}")]
    public static partial void LoginNotSupported(this ILogger logger, string urlType);

    [LoggerMessage(EventId = 6,
                      EventName = Prefix + nameof(InvalidDimensions),
                      Level = LogLevel.Warning,
                      Message =
                          "The image's dimensions are invalid. Width: {currentWidth} (expected {expectedWidth}); Height: {currentHeight} (expected {expectedHeight})")]
    public static partial void InvalidDimensions(this ILogger logger, int currentWidth, int expectedWidth, int currentHeight, int expectedHeight);
}