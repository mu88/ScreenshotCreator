using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

public interface IPlaywrightFacade : IAsyncDisposable
{
    ValueTask<IPage> GetPlaywrightPageAsync();
}