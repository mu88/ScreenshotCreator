namespace ScreenshotCreator.Logic;

public class ScreenshotOptions
{
    public const string SectionName = nameof(ScreenshotOptions);

    public string DashboardUrl { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public uint Width { get; set; }

    public uint Height { get; set; }

    public uint TimeoutBetweenHttpCallsInSeconds { get; set; }

    public uint RefreshIntervalInSeconds { get; set; }

    public string ScreenshotFileName { get; set; } = string.Empty;
    
    public bool BackgroundProcessingEnabled { get; set; }
}