using System.Diagnostics.CodeAnalysis;

namespace ScreenshotCreator.Logic;

public static class ActivityExtensions
{
    public static bool DisplayShouldBeActive([NotNullWhen(false)] this Activity? activity, TimeProvider? timeProvider = null)
    {
        if (activity is null)
        {
            return true;
        }

        var currentLocalTime = GetCurrentLocalTime(timeProvider ?? TimeProvider.System);
        return activity.ActiveFrom <= currentLocalTime && currentLocalTime <= activity.ActiveTo;
    }

    private static TimeOnly GetCurrentLocalTime(TimeProvider timeProvider) =>
        TimeOnly.FromDateTime(TimeZoneInfo
                                  .ConvertTimeFromUtc(timeProvider.GetUtcNow().UtcDateTime,
                                                      TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? TimeZoneInfo.Local.Id)));
}