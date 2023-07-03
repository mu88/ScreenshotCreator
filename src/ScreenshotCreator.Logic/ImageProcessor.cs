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

        var newWidth = image.Width % 8 == 0 ? image.Width / 8 : image.Width / 8 + 1;
        var waveshareBytes = new byte[newWidth * image.Height];
        var binaryValues = new bool[image.Width * image.Height];

        var pixels = image.GetPixels().ToList();
        for (var i = 0; i < pixels.Count; i++)
        {
            var pixel = pixels[i];
            var currentPixelValue = pixel[0];
            binaryValues[i] = currentPixelValue == 65535;
        }

        for (var i = 0; i < binaryValues.Length / 8; i++)
        {
            var currentBitBatch = binaryValues.Skip(i * 8).Take(8).Reverse().ToArray();
            var bitArray = new BitArray(currentBitBatch);
            bitArray.CopyTo(waveshareBytes, i);
        }

        return waveshareBytes;
    }
}