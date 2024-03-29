﻿using ScreenshotCreator.Logic;

namespace ScreenshotCreator.Api;

internal static class HeaderDictionaryExtensions
{
    public static void AddWaveshareInstructions(this IHeaderDictionary headers,
                                                ScreenshotOptions screenshotOptions,
                                                string screenshotFile,
                                                Func<string, DateTime>? getLastWriteTimeUtc = null,
                                                string? localTimeZoneId = null)
    {
        headers.Append("waveshare-last-modified-local-time",
                       GetLastModifiedAsLocalTime(screenshotFile, getLastWriteTimeUtc ?? File.GetLastWriteTimeUtc, localTimeZoneId ?? TimeZoneInfo.Local.Id));
        headers.Append("waveshare-sleep-between-updates", screenshotOptions.CalculateSleepBetweenUpdates());
        headers.Append("waveshare-update-screen", screenshotOptions.Activity.DisplayShouldBeActive().ToString());
    }

    private static string GetLastModifiedAsLocalTime(string file, Func<string, DateTime> getLastWriteTimeUtc, string localTimeZoneId) =>
        TimeZoneInfo
            .ConvertTimeFromUtc(getLastWriteTimeUtc(file), TimeZoneInfo.FindSystemTimeZoneById(Environment.GetEnvironmentVariable("TZ") ?? localTimeZoneId))
            .ToShortTimeString();
}