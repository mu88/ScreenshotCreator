using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace ScreenshotCreator.Logic;

public class ScreenshotCreator
{
    private readonly ILogger<ScreenshotCreator> _logger;
    private readonly ScreenshotOptions _screenshotOptions;
    private IPage? _page;

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
            Log.ReusingPlaywrightPage(_logger);

        if (await NeedsLoginAsync(_page)) await LoginAsync(_page);

        await _page.SetViewportSizeAsync((int)width, (int)height);
        await _page.GotoAsync(_screenshotOptions.DashboardUrl);
        await WaitAsync();
        await _page.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotOptions.ScreenshotFileName });

        Log.ScreenshotCreated(_logger);
    }

    private async Task LoginAsync(IPage page)
    {
        Log.LoggingIn(_logger);

        await page.GotoAsync(GetBaseUrl());
        await WaitAsync();
        await page.GetByPlaceholder("User Name").FillAsync(_screenshotOptions.Username);
        await page.GetByPlaceholder("Password", new PageGetByPlaceholderOptions { Exact = true }).FillAsync(_screenshotOptions.Password);
        await page.GetByRole(AriaRole.Button).ClickAsync();
        await WaitAsync();
    }

    private async Task<bool> NeedsLoginAsync(IPage page)
    {
        await page.GotoAsync(_screenshotOptions.DashboardUrl);
        await WaitAsync();

        var needsLogin = await page.GetByText("Page Unavailable").CountAsync() == 0;
        Log.LoginNecessaryCheck(_logger, needsLogin);

        return needsLogin;
    }

    private async Task<IPage> InitializePlaywrightAsync()
    {
        var pageTest = new PageTest();
        pageTest.WorkerSetup();
        await pageTest.PlaywrightSetup();
        await pageTest.BrowserSetup();
        await pageTest.ContextSetup();
        await pageTest.PageSetup();
        var page = pageTest.Page;

        Log.PlaywrightTestInitialized(_logger);

        return page;
    }

    private async Task WaitAsync() => await Task.Delay(TimeSpan.FromSeconds(_screenshotOptions.TimeoutBetweenHttpCallsInSeconds));

    private string GetBaseUrl() => new Uri(_screenshotOptions.DashboardUrl).GetLeftPart(UriPartial.Authority);
}