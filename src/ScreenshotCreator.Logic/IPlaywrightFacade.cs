using Microsoft.Playwright;

namespace ScreenshotCreator.Logic;

internal interface IPlaywrightFacade : IAsyncDisposable
{
    ValueTask<IPage> GetPlaywrightPageAsync();
}
