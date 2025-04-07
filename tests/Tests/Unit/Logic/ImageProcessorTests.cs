using System.Net.Mime;
using FluentAssertions;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ScreenshotCreator.Logic;

namespace Tests.Unit.Logic;

[TestFixture]
[Category("Unit")]
public class ImageProcessorTests
{
    [Test]
    public async Task ProcessImage()
    {
        // Arrange
        var testee = new ImageProcessor(Substitute.For<ILogger<ImageProcessor>>());

        // Act
        var result = await testee.ProcessAsync("testData/Screenshot.png", false, false);

        // Assert
        result.Data.Should().HaveCount(26152);
        result.MediaType.Should().Be("image/png");
    }

    [Test]
    public async Task ProcessImage_ShouldCreateBlackWhiteImage()
    {
        // Arrange
        var testee = new ImageProcessor(NullLogger<ImageProcessor>.Instance);

        // Act
        var result = await testee.ProcessAsync("testData/Screenshot.png", true, false);

        // Assert
        result.Data.Length.Should().BeGreaterThan(3000, "because there is a certain variance in the size with every Magick version");
        result.MediaType.Should().Be("image/png");
        new MagickImage(result.Data).GetPixels().Select(pixel => pixel.GetChannel(0)).Distinct().Should().BeEquivalentTo(new List<ushort> { 0, 65535 });
    }

    [Test]
    public async Task ProcessImage_ShouldCreateBlackWhiteImageInWaveshareFormat()
    {
        // Arrange
        var testee = new ImageProcessor(Substitute.For<ILogger<ImageProcessor>>());

        // Act
        var result = await testee.ProcessAsync("testData/Screenshot.png", true, true);

        // Assert
        result.Data.Should().HaveCount(48000).And.BeEquivalentTo(await File.ReadAllBytesAsync("testData/Screenshot_bw.blob"));
        result.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [TestCase("testData/Screenshot_invalidByInvalid.png")]
    [TestCase("testData/Screenshot_invalidBy480.png")]
    [TestCase("testData/Screenshot_800ByInvalid.png")]
    public async Task ProcessImage_ShouldReturnEmptyIfImageHasInvalidDimensions(string fileName)
    {
        // Arrange
        var testee = new ImageProcessor(Substitute.For<ILogger<ImageProcessor>>());

        // Act
        var result = await testee.ProcessAsync(fileName, true, true);

        // Assert
        result.Data.Should().HaveCount(0);
        result.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }
}