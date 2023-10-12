using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

[ExcludeFromCodeCoverage]
public sealed class PlaywrightHelper : IPlaywrightHelper, IAsyncDisposable
{
    private readonly ILogger<PlaywrightHelper> _logger;
    private readonly ScreenshotOptions _screenshotOptions;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;
    private bool _disposed;

    public PlaywrightHelper(IOptions<ScreenshotOptions> options, ILogger<PlaywrightHelper> logger)
    {
        _logger = logger;
        _screenshotOptions = options.Value;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_browser != null) await _browser.DisposeAsync();
        _playwright?.Dispose();

        _disposed = true;
    }

    /// <inheritdoc />
    public async ValueTask<IPage> InitializePlaywrightAsync()
    {
        if (_page != null)
        {
            _logger.ReusingPlaywrightPage();
            return _page;
        }

        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        _page = await _browser.NewPageAsync(new BrowserNewPageOptions { TimezoneId = Environment.GetEnvironmentVariable("TZ") });

        _logger.PlaywrightInitialized();

        return _page;
    }

    /// <inheritdoc />
    public async Task WaitAsync() => await Task.Delay(TimeSpan.FromSeconds(_screenshotOptions.TimeBetweenHttpCallsInSeconds));
}