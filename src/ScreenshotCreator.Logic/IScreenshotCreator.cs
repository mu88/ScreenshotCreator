namespace ScreenshotCreator.Logic;

public interface IScreenshotCreator
{
    Task CreateScreenshotAsync(uint width, uint height);
}