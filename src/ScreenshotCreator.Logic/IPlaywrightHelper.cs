namespace ScreenshotCreator.Logic;

internal interface IPlaywrightHelper
{
    IPlaywrightFacade CreatePlaywrightFacade();

    Task WaitAsync(CancellationToken cancellationToken);
}
