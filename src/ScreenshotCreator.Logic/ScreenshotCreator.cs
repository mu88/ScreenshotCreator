using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

public sealed class ScreenshotCreator : IScreenshotCreator
{
    private readonly IPlaywrightHelper _playwrightHelper;
    private readonly ILogger<ScreenshotCreator> _logger;
    private readonly ScreenshotOptions _screenshotOptions;

    public ScreenshotCreator(IPlaywrightHelper playwrightHelper, IOptions<ScreenshotOptions> options, ILogger<ScreenshotCreator> logger)
    {
        _playwrightHelper = playwrightHelper;
        _logger = logger;
        _screenshotOptions = options.Value;
    }

    public async Task CreateScreenshotAsync(uint width, uint height)
    {
        var page = await _playwrightHelper.InitializePlaywrightAsync();

        await page.SetViewportSizeAsync((int)width, (int)height);
        if (await NeedsLoginAsync(page)) await LoginAsync(page);

        await NavigateToUrlAsync(page);

        await page.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotOptions.ScreenshotFileName, Type = ScreenshotType.Png });

        _logger.ScreenshotCreated();
    }

    private async Task NavigateToUrlAsync(IPage page)
    {
        await page.GotoAsync(_screenshotOptions.Url);
        await _playwrightHelper.WaitAsync();
    }

    private async Task LoginAsync(IPage page)
    {
        _logger.LoggingIn();

        await page.GotoAsync(GetBaseUrl());
        await _playwrightHelper.WaitAsync();
        await page.GetByPlaceholder("User Name").FillAsync(_screenshotOptions.Username);
        await page.GetByPlaceholder("Password", new PageGetByPlaceholderOptions { Exact = true }).FillAsync(_screenshotOptions.Password);
        await page.GetByRole(AriaRole.Button).ClickAsync();
        await _playwrightHelper.WaitAsync();
    }

    private async Task<bool> NeedsLoginAsync(IPage page)
    {
        if (_screenshotOptions.UrlType != UrlType.OpenHab)
        {
            _logger.LoginNotSupported(_screenshotOptions.UrlType.ToString());
            return false;
        }

        await NavigateToUrlAsync(page);

        var needsLogin = await page.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync() > 0;
        _logger.LoginNecessaryCheck(needsLogin);

        return needsLogin;
    }

    private string GetBaseUrl() => new Uri(_screenshotOptions.Url).GetLeftPart(UriPartial.Authority);
}