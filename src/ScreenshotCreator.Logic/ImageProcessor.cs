using System.Collections;
using System.Net.Mime;
using ImageMagick;

namespace ScreenshotCreator.Logic;

public class ImageProcessor
{
    public async Task<ProcessingResult> ProcessAsync(string screenshotFile, bool blackAndWhite, bool forWaveshare)
    {
        using var image = new MagickImage();
        await image.ReadAsync(screenshotFile);

        if (blackAndWhite)
        {
            image.Alpha(AlphaOption.Off);
            image.Threshold(new Percentage(95));
        }

        var bytes = forWaveshare
                        ? ToWaveshareBytes(image)
                        : image.ToByteArray();
        var contentType = forWaveshare ? MediaTypeNames.Application.Octet : image.FormatInfo?.MimeType;

        return new ProcessingResult(bytes, contentType);
    }

    private static byte[] ToWaveshareBytes(MagickImage image)
    {
        var newWidth = image.Width % 8 == 0 ? image.Width / 8 : image.Width / 8 + 1;
        var waveshareBytes = new byte[newWidth * image.Height];
        var originalBytes = image.GetPixelsUnsafe().ToByteArray("R") ?? Array.Empty<byte>();
        var currentBatchAsBinary = new bool[8];
        for (var x = 0; x < newWidth; x++)
        {
            for (var y = 0; y < image.Height; y++)
            {
                var currentBatchIndex = x * y;
                var currentBatchValue = originalBytes.Skip(currentBatchIndex).Take(8).ToArray();
                for (var i = 0; i < currentBatchValue.Length; i++) { currentBatchAsBinary[i] = currentBatchValue[i] == 255; }

                var bitArray = new BitArray(currentBatchAsBinary);
                bitArray.CopyTo(waveshareBytes, currentBatchIndex);
            }
        }

        return waveshareBytes;
    }
}