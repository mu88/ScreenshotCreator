namespace ScreenshotCreator.Api;

public static partial class Log
{
    private const string Prefix = nameof(Api);

    [LoggerMessage(EventId = 0,
                      EventName = Prefix + nameof(BackgroundServiceTriggered),
                      Level = LogLevel.Information,
                      Message = "Background service triggered")]
    public static partial void BackgroundServiceTriggered(ILogger logger);

    [LoggerMessage(EventId = 1,
                      EventName = Prefix + nameof(BackgroundServiceDisabled),
                      Level = LogLevel.Information,
                      Message = "Background service disabled")]
    public static partial void BackgroundServiceDisabled(ILogger logger);
}