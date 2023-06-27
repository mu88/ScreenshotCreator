using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

public sealed class ScreenshotCreator : IAsyncDisposable
{
    private readonly ILogger<ScreenshotCreator> _logger;
    private readonly ScreenshotOptions _screenshotOptions;
    private IPage? _page;
    private bool _disposed;
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public ScreenshotCreator(IOptions<ScreenshotOptions> options, ILogger<ScreenshotCreator> logger)
    {
        _logger = logger;
        _screenshotOptions = options.Value;
    }

    public async Task CreateScreenshotAsync(uint width, uint height)
    {
        if (_page == null)
            _page = await InitializePlaywrightAsync();
        else
            _logger.ReusingPlaywrightPage();

        await _page.SetViewportSizeAsync((int)width, (int)height);
        if (await NeedsLoginAsync(_page))
        {
            await LoginAsync(_page);
            await NavigateToDashboardAsync(_page);
        }

        await _page.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotOptions.ScreenshotFileName, Type = ScreenshotType.Png });

        _logger.ScreenshotCreated();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_browser != null) await _browser.DisposeAsync();
        _playwright?.Dispose();

        _disposed = true;
    }

    private async Task NavigateToDashboardAsync(IPage page)
    {
        await page.GotoAsync(_screenshotOptions.DashboardUrl);
        await WaitAsync();
    }

    private async Task LoginAsync(IPage page)
    {
        _logger.LoggingIn();

        await page.GotoAsync(GetBaseUrl());
        await WaitAsync();
        await page.GetByPlaceholder("User Name").FillAsync(_screenshotOptions.Username);
        await page.GetByPlaceholder("Password", new PageGetByPlaceholderOptions { Exact = true }).FillAsync(_screenshotOptions.Password);
        await page.GetByRole(AriaRole.Button).ClickAsync();
        await WaitAsync();
    }

    private async Task<bool> NeedsLoginAsync(IPage page)
    {
        await NavigateToDashboardAsync(page);

        var needsLogin = await page.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync() > 0;
        _logger.LoginNecessaryCheck(needsLogin);

        return needsLogin;
    }

    private async Task<IPage> InitializePlaywrightAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        var page = await _browser.NewPageAsync(new BrowserNewPageOptions { TimezoneId = Environment.GetEnvironmentVariable("TZ") });

        _logger.PlaywrightInitialized();

        return page;
    }

    private async Task WaitAsync() => await Task.Delay(TimeSpan.FromSeconds(_screenshotOptions.TimeoutBetweenHttpCallsInSeconds));

    private string GetBaseUrl() => new Uri(_screenshotOptions.DashboardUrl).GetLeftPart(UriPartial.Authority);
}