using ScreenshotCreator.Logic;

namespace ScreenshotCreator.Api;

internal static class HeaderDictionaryExtensions
{
    public static void AddWaveshareInstructions(this IHeaderDictionary headers,
                                                ScreenshotOptions screenshotOptions,
                                                string screenshotFile,
                                                Func<string, DateTime>? getLastWriteTimeUtc = null,
                                                string? localTimeZoneId = null)
    {
        headers.Add("waveshare-last-modified-local-time",
                    GetLastModifiedAsLocalTime(screenshotFile, getLastWriteTimeUtc ?? File.GetLastWriteTimeUtc, localTimeZoneId ?? TimeZoneInfo.Local.Id));
        headers.Add("waveshare-sleep-between-updates", screenshotOptions.CalculateSleepBetweenUpdates());
        headers.Add("waveshare-update-screen", screenshotOptions.Activity.DisplayShouldBeActive() ? true.ToString() : false.ToString());
    }

    private static string GetLastModifiedAsLocalTime(string file, Func<string, DateTime> getLastWriteTimeUtc, string localTimeZoneId) =>
        TimeZoneInfo
            .ConvertTimeFromUtc(getLastWriteTimeUtc(file), TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? localTimeZoneId))
            .ToShortTimeString();
}