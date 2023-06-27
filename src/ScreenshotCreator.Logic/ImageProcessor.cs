using System.Net.Mime;
using ImageMagick;

namespace ScreenshotCreator.Logic;

public class ImageProcessor
{
    public async Task<ProcessingResult> ProcessAsync(string screenshotFile, bool blackAndWhite, bool returnPixelValuesOnly)
    {
        using var image = new MagickImage();
        await image.ReadAsync(screenshotFile);

        if (blackAndWhite)
        {
            image.Alpha(AlphaOption.Off);
            image.Threshold(new Percentage(95));
        }

        var bytes = returnPixelValuesOnly
                        ? image.GetPixelsUnsafe().ToByteArray("R") ?? Array.Empty<byte>()
                        : image.ToByteArray();
        var contentType = returnPixelValuesOnly ? MediaTypeNames.Application.Octet : image.FormatInfo?.MimeType;

        return new ProcessingResult(bytes, contentType);
    }
}