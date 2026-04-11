using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using ScreenshotCreator.Logic;

namespace ScreenshotCreator.Api;

internal class BackgroundScreenshotCreator(
    IScreenshotCreator screenshotCreator,
    IOptions<ScreenshotOptions> options,
    ILogger<BackgroundScreenshotCreator> logger) : BackgroundService
{
    private readonly ScreenshotOptions _screenshotOptions = options.Value;

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_screenshotOptions.BackgroundProcessingEnabled)
        {
            logger.BackgroundServiceDisabled();
            return;
        }

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(_screenshotOptions.RefreshIntervalInSeconds));

        // There should always be at least one image present in case the background processor is enabled
        await screenshotCreator.CreateScreenshotAsync(_screenshotOptions.Width, _screenshotOptions.Height, stoppingToken);

        // Code coverage false positive
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            logger.BackgroundServiceTriggered();
            await screenshotCreator.CreateScreenshotAsync(_screenshotOptions.Width, _screenshotOptions.Height, stoppingToken);
        }
    }
}
