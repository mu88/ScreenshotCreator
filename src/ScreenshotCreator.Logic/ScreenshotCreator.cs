using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace ScreenshotCreator.Logic;

public class ScreenshotCreator
{
    private readonly ILogger<ScreenshotCreator> _logger;
    private readonly ScreenshotOptions _screenshotOptions;

    public ScreenshotCreator(IOptions<ScreenshotOptions> options, ILogger<ScreenshotCreator> logger)
    {
        _logger = logger;
        _screenshotOptions = options.Value;
    }

    public async Task CreateScreenshotAsync(uint width, uint height)
    {
        var page = await InitializePlaywrightAsync();

        await page.SetViewportSizeAsync((int)width, (int)height);
        await page.GotoAsync(GetBaseUrl());
        await WaitAsync();
        await page.GetByPlaceholder("User Name").FillAsync(_screenshotOptions.Username);
        await page.GetByPlaceholder("Password", new PageGetByPlaceholderOptions { Exact = true }).FillAsync(_screenshotOptions.Password);
        await page.GetByRole(AriaRole.Button).ClickAsync();
        await WaitAsync();
        await page.GotoAsync(_screenshotOptions.DashboardUrl);
        await WaitAsync();
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotOptions.ScreenshotFileName });
        
        LoggerExtensions.ScreenshotCreated(_logger);
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
        
        LoggerExtensions.PlaywrightTestInitialized(_logger);

        return page;
    }

    private async Task WaitAsync() => await Task.Delay(TimeSpan.FromSeconds(_screenshotOptions.TimeoutBetweenHttpCallsInSeconds));

    private string GetBaseUrl() => new Uri(_screenshotOptions.DashboardUrl).GetLeftPart(UriPartial.Authority);
}