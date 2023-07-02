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

        image.Draw(new DrawableText(750, 470, File.GetLastWriteTimeUtc(screenshotFile).ToShortTimeString()));

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