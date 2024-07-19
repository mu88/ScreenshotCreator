using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace ScreenshotCreator.Logic;

public class ImageProcessor(ILogger<ImageProcessor> logger)
{
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
        var contentType = asWaveshareBytes ? MediaTypeNames.Application.Octet : GetImageMimeType(image);

        return new ProcessingResult(bytes, contentType);
    }

    [ExcludeFromCodeCoverage(Justification = "Didn't find a of setting FormatInfo to null")]
    private static string GetImageMimeType(MagickImage image) => MagickFormatInfo.Create(image.Format)?.MimeType ?? "NoClue";

    private byte[] ToWaveshareBytes(MagickImage image)
    {
        if (image.Width != 800 || image.Height != 480)
        {
            logger.InvalidDimensions(image.Width, 800, image.Height, 480);
            return Array.Empty<byte>();
        }

        var newWidth = image.Width / 8;
        var waveshareBytes = new byte[newWidth * image.Height];
        using var unsafePixelCollection = image.GetPixelsUnsafe();
        var pixelByteSpan = unsafePixelCollection.ToByteArray("R").AsSpan();
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