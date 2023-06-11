using Microsoft.Extensions.Logging;

namespace ScreenshotCreator.Logic;

public static partial class Log
{
    private const string Prefix = nameof(Logic);

    [LoggerMessage(EventId = 0,
                      EventName = Prefix + nameof(PlaywrightInitialized),
                      Level = LogLevel.Information,
                      Message = "Playwright initialized")]
    public static partial void PlaywrightInitialized(ILogger logger);

    [LoggerMessage(EventId = 1,
                      EventName = Prefix + nameof(PlaywrightInitialized),
                      Level = LogLevel.Information,
                      Message = "Screenshot created")]
    public static partial void ScreenshotCreated(ILogger logger);

    [LoggerMessage(EventId = 2,
                      EventName = Prefix + nameof(ReusingPlaywrightPage),
                      Level = LogLevel.Information,
                      Message = "Reusing Playwright page")]
    public static partial void ReusingPlaywrightPage(ILogger logger);

    [LoggerMessage(EventId = 3,
                      EventName = Prefix + nameof(LoginNecessaryCheck),
                      Level = LogLevel.Information,
                      Message = "Check if login is necessary was {loginIsNecessary}")]
    public static partial void LoginNecessaryCheck(ILogger logger, bool loginIsNecessary);

    [LoggerMessage(EventId = 4,
                      EventName = Prefix + nameof(LoggingIn),
                      Level = LogLevel.Information,
                      Message = "Logging in")]
    public static partial void LoggingIn(ILogger logger);
}