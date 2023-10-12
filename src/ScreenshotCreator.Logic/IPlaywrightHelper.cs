using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

public interface IPlaywrightHelper
{
    ValueTask<IPage> InitializePlaywrightAsync();

    Task WaitAsync();
}