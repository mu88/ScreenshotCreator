using System.Diagnostics.CodeAnalysis;
using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

[ExcludeFromCodeCoverage(Justification = "Testing the dispose implementation offers no real value")]
internal sealed class PlaywrightFacade : IPlaywrightFacade
{
    private IBrowser? _browser;
    private bool _disposed;
    private IPlaywright? _playwright;

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_browser != null)
        {
            await _browser.DisposeAsync();
        }

        _playwright?.Dispose();

        _disposed = true;
    }

    public async ValueTask<IPage> GetPlaywrightPageAsync()
    {
        _playwright?.Dispose();
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();

        return await _browser.NewPageAsync(new BrowserNewPageOptions { TimezoneId = Environment.GetEnvironmentVariable("TZ") });
    }
}