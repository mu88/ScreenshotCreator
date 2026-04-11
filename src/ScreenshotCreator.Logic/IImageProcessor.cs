namespace ScreenshotCreator.Logic;

public interface IImageProcessor
{
    Task<ProcessingResult> ProcessAsync(string screenshotFile, bool blackAndWhite, bool asWaveshareBytes);
}
