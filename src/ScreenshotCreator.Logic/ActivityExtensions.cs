using System.Diagnostics.CodeAnalysis;

namespace ScreenshotCreator.Logic;

public static class ActivityExtensions
{
    public static bool DisplayShouldBeActive([NotNullWhen(false)] this Activity? activity)
    {
        if (activity is null) return true;

        var currentLocalTime = GetCurrentLocalTime();
        return activity.ActiveFrom <= currentLocalTime && currentLocalTime <= activity.ActiveTo;
    }

    private static TimeOnly GetCurrentLocalTime() =>
        TimeOnly.FromDateTime(TimeZoneInfo
                                  .ConvertTimeFromUtc(DateTime.UtcNow,
                                                      TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? TimeZoneInfo.Local.Id)));
}