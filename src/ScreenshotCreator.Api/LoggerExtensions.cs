namespace ScreenshotCreator.Api;

public static partial class LoggerExtensions
{
    private const string Prefix = nameof(Api);

    [LoggerMessage(EventId = 0,
                      EventName = Prefix + nameof(BackgroundServiceTriggered),
                      Level = LogLevel.Information,
                      Message = "Background service triggered")]
    public static partial void BackgroundServiceTriggered(ILogger logger);
}