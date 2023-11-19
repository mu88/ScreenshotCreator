using System.ComponentModel.DataAnnotations;

namespace ScreenshotCreator.Logic;

public class ScreenshotOptions
{
    public const string SectionName = nameof(ScreenshotOptions);

    // Stryker disable all : don't mutate default initialization
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    [Required]
    public UrlType UrlType { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ScreenshotFile { get; set; } = "/home/app/Screenshot.png";

    public string? AvailabilityIndicator { get; set; } = string.Empty;
    // Stryker restore all

    [Range(1, uint.MaxValue)]
    public uint Width { get; set; }

    [Range(1, uint.MaxValue)]
    public uint Height { get; set; }

    [Range(1, uint.MaxValue)]
    public uint TimeBetweenHttpCallsInSeconds { get; set; }

    [Range(1, uint.MaxValue)]
    public uint RefreshIntervalInSeconds { get; set; }

    [Required]
    public bool BackgroundProcessingEnabled { get; set; }

    public Activity? Activity { get; set; }

    public string CalculateSleepBetweenUpdates() =>
        Activity.DisplayShouldBeActive()
            ? RefreshIntervalInSeconds.ToString()
            : Activity.RefreshIntervalWhenInactiveInSeconds.ToString();
}

public enum UrlType
{
    Any,
    OpenHab
}

public record Activity([Required]
                       TimeOnly ActiveFrom,
                       [Required]
                       TimeOnly ActiveTo,
                       [Range(1, uint.MaxValue)]
                       uint RefreshIntervalWhenInactiveInSeconds);