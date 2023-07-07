namespace ScreenshotCreator.Logic;

public interface IScreenshotCreator
    : IAsyncDisposable
{
    Task CreateScreenshotAsync(uint width, uint height);
}