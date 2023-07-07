using Microsoft.Extensions.Options;
using ScreenshotCreator.Logic;

namespace ScreenshotCreator.Api;

internal class BackgroundScreenshotCreator : BackgroundService
{
    private readonly Logic.ScreenshotCreator _screenshotCreator;
    private readonly ILogger<BackgroundScreenshotCreator> _logger;
    private readonly ScreenshotOptions _screenshotOptions;

    /// <inheritdoc />
    public BackgroundScreenshotCreator(Logic.ScreenshotCreator screenshotCreator, IOptions<ScreenshotOptions> options, ILogger<BackgroundScreenshotCreator> logger)
    {
        _screenshotCreator = screenshotCreator;
        _logger = logger;
        _screenshotOptions = options.Value;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_screenshotOptions.BackgroundProcessingEnabled)
        {
            _logger.BackgroundServiceDisabled();
            return;
        }

        PeriodicTimer timer = new(TimeSpan.FromSeconds(_screenshotOptions.RefreshIntervalInSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.BackgroundServiceTriggered();
            if (_screenshotOptions.Activity.DisplayShouldBeActive()) await _screenshotCreator.CreateScreenshotAsync(_screenshotOptions.Width, _screenshotOptions.Height);
        }
    }
}