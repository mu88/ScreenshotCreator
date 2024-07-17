namespace ScreenshotCreator.Logic;

public interface IPlaywrightHelper
{
    IPlaywrightFacade CreatePlaywrightFacade();

    Task WaitAsync();
}