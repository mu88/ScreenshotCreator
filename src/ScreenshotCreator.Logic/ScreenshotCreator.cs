using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

internal sealed class ScreenshotCreator(IPlaywrightHelper playwrightHelper, IOptions<ScreenshotOptions> options, ILogger<ScreenshotCreator> logger)
    : IScreenshotCreator
{
    private readonly ScreenshotOptions _screenshotOptions = options.Value;

    public async Task CreateScreenshotAsync(uint width, uint height, CancellationToken cancellationToken)
    {
        try
        {
            await using var playwrightFacade = playwrightHelper.CreatePlaywrightFacade();
            var page = await playwrightFacade.GetPlaywrightPageAsync();

            await page.SetViewportSizeAsync((int)width, (int)height);
            if (await NeedsLoginAsync(page, cancellationToken))
            {
                await LoginAsync(page, cancellationToken);
            }

            if (await PageIsAvailableAsync(page, cancellationToken))
            {
                await page.ScreenshotAsync(new PageScreenshotOptions { Path = _screenshotOptions.ScreenshotFile, Type = ScreenshotType.Png });
                logger.ScreenshotCreated();
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.ScreenshotCreationFailed(ex);
        }
    }

    private async Task<bool> PageIsAvailableAsync(IPage page, CancellationToken cancellationToken)
    {
        await NavigateToUrlAsync(page, cancellationToken);
        if (string.IsNullOrWhiteSpace(_screenshotOptions.AvailabilityIndicator))
        {
            return true;
        }

        return await page.GetByText(_screenshotOptions.AvailabilityIndicator).CountAsync() > 0;
    }

    private async Task NavigateToUrlAsync(IPage page, CancellationToken cancellationToken)
    {
        await page.GotoAsync(_screenshotOptions.Url);
        await playwrightHelper.WaitAsync(cancellationToken);
    }

    private async Task LoginAsync(IPage page, CancellationToken cancellationToken)
    {
        logger.LoggingIn();

        await page.GotoAsync(GetBaseUrl());
        await playwrightHelper.WaitAsync(cancellationToken);

        var usernameTextfield = page.GetByPlaceholder("User Name");
        if (!await usernameTextfield.IsVisibleAsync())
        {
            var loginButton = page.GetByText("lock_shield_fill");
            if (!await loginButton.IsVisibleAsync())
            {
                await page.GetByText("menu").ClickAsync();
            }

            await loginButton.ClickAsync();
        }

        await usernameTextfield.FillAsync(_screenshotOptions.Username);
        await page.GetByPlaceholder("Password", new PageGetByPlaceholderOptions { Exact = true }).FillAsync(_screenshotOptions.Password);
        await page.GetByRole(AriaRole.Button).ClickAsync();
        await playwrightHelper.WaitAsync(cancellationToken);
    }

    private async Task<bool> NeedsLoginAsync(IPage page, CancellationToken cancellationToken)
    {
        if (_screenshotOptions.UrlType != UrlType.OpenHab)
        {
            logger.LoginNotSupported(_screenshotOptions.UrlType);
            return false;
        }

        await NavigateToUrlAsync(page, cancellationToken);

        var needsLogin = await page.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync() > 0;
        logger.LoginNecessaryCheck(needsLogin);

        return needsLogin;
    }

    private string GetBaseUrl() => new Uri(_screenshotOptions.Url).GetLeftPart(UriPartial.Authority);
}
