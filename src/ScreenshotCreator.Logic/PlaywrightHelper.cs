using Microsoft.Extensions.Options;

namespace ScreenshotCreator.Logic;

internal sealed class PlaywrightHelper(IOptions<ScreenshotOptions> options) : IPlaywrightHelper
{
    private readonly ScreenshotOptions _screenshotOptions = options.Value;

    /// <inheritdoc />
    public IPlaywrightFacade CreatePlaywrightFacade() => new PlaywrightFacade();

    /// <inheritdoc />
    public async Task WaitAsync(CancellationToken cancellationToken)
        => await Task.Delay(TimeSpan.FromSeconds(_screenshotOptions.TimeBetweenHttpCallsInSeconds), cancellationToken);
}
