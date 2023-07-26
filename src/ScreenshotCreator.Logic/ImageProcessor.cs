using System.Collections;
using System.Net.Mime;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace ScreenshotCreator.Logic;

public class ImageProcessor
{
    private readonly ILogger<ImageProcessor> _logger;

    public ImageProcessor(ILogger<ImageProcessor> logger) => _logger = logger;

    public async Task<ProcessingResult> ProcessAsync(string screenshotFile, bool blackAndWhite, bool asWaveshareBytes)
    {
        using var image = new MagickImage();
        await image.ReadAsync(screenshotFile);

        if (blackAndWhite)
        {
            image.Alpha(AlphaOption.Off);
            image.Threshold(new Percentage(95));
        }

        var bytes = asWaveshareBytes
                        ? ToWaveshareBytes(image)
                        : image.ToByteArray();
        var contentType = asWaveshareBytes ? MediaTypeNames.Application.Octet : image.FormatInfo?.MimeType;

        return new ProcessingResult(bytes, contentType);
    }

    private byte[] ToWaveshareBytes(MagickImage image)
    {
        if (image.Width != 800 || image.Height != 480)
        {
            _logger.InvalidDimensions(image.Width, 800, image.Height, 480);
            return Array.Empty<byte>();
        }

        var newWidth = image.Width / 8;
        var waveshareBytes = new byte[newWidth * image.Height];
        var pixelByteSpan = image.GetPixelsUnsafe().ToByteArray("R").AsSpan();
        var array = new BitArray(8);
        for (var finalBytePosition = 0; finalBytePosition < pixelByteSpan.Length / 8; finalBytePosition++)
        {
            var currentSlice = pixelByteSpan.Slice(finalBytePosition * 8, 8);
            currentSlice.Reverse();
            for (var currentSlicePosition = 0; currentSlicePosition < 8; currentSlicePosition++)
            {
                array[currentSlicePosition] = currentSlice[currentSlicePosition] == 255;
            }

            array.CopyTo(waveshareBytes, finalBytePosition);
        }

        return waveshareBytes;
    }
}