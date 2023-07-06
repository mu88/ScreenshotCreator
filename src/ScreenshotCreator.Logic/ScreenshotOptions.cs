namespace ScreenshotCreator.Logic;

public class ScreenshotOptions
{
    public const string SectionName = nameof(ScreenshotOptions);

    public const string ScreenshotFileName = "Screenshot.png";

    public string Url { get; set; } = string.Empty;

    public UrlType UrlType { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public uint Width { get; set; }

    public uint Height { get; set; }

    public uint TimeoutBetweenHttpCallsInSeconds { get; set; }

    public uint RefreshIntervalInSeconds { get; set; }

    public bool BackgroundProcessingEnabled { get; set; }

    public Activity? Activity { get; set; }
}

public enum UrlType
{
    Any,
    OpenHab
}

public record Activity(TimeOnly ActiveFrom, TimeOnly ActiveTo, uint RefreshIntervalWhenInactiveInSeconds);