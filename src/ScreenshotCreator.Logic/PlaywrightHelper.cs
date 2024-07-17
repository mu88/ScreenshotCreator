using Microsoft.Extensions.Options;

namespace ScreenshotCreator.Logic;

public class PlaywrightHelper(IOptions<ScreenshotOptions> options) : IPlaywrightHelper
{
    private readonly ScreenshotOptions _screenshotOptions = options.Value;

    /// <inheritdoc />
    public IPlaywrightFacade CreatePlaywrightFacade() => new PlaywrightFacade();

    /// <inheritdoc />
    public async Task WaitAsync() => await Task.Delay(TimeSpan.FromSeconds(_screenshotOptions.TimeBetweenHttpCallsInSeconds));
}