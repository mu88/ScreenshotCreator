using Microsoft.Extensions.Logging;

namespace ScreenshotCreator.Logic;

public static partial class LoggerExtensions
{
    private const string Prefix = nameof(Logic);

    [LoggerMessage(EventId = 0,
                      EventName = Prefix + nameof(PlaywrightTestInitialized),
                      Level = LogLevel.Information,
                      Message = "Playwright test initialized")]
    public static partial void PlaywrightTestInitialized(ILogger logger);

    [LoggerMessage(EventId = 1,
                      EventName = Prefix + nameof(PlaywrightTestInitialized),
                      Level = LogLevel.Information,
                      Message = "Screenshot created")]
    public static partial void ScreenshotCreated(ILogger logger);
}