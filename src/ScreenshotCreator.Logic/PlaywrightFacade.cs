using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

internal sealed class PlaywrightFacade : IPlaywrightFacade
{
    private IBrowser? _browser;
    private bool _disposed;
    private IPage? _page;
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
        _page = await _browser.NewPageAsync(new BrowserNewPageOptions { TimezoneId = Environment.GetEnvironmentVariable("TZ") });

        return _page;
    }
}