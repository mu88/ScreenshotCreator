using System.Net.Mime;
using FluentAssertions;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
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
        var testee = new ImageProcessor(new Mock<ILogger<ImageProcessor>>().Object);

        // Act
        var result = await testee.ProcessAsync("testData/Screenshot.png", false, false);

        // Assert
        result.Data.Should().HaveCount(26121);
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
        result.Data.Should().HaveCount(3245);
        result.MediaType.Should().Be("image/png");
        new MagickImage(result.Data).GetPixels().Select(pixel => pixel.GetChannel(0)).Distinct().Should().BeEquivalentTo(new List<ushort> { 0, 65535 });
    }

    [Test]
    public async Task ProcessImage_ShouldCreateBlackWhiteImageInWaveshareFormat()
    {
        // Arrange
        var testee = new ImageProcessor(new Mock<ILogger<ImageProcessor>>().Object);

        // Act
        var result = await testee.ProcessAsync("testData/Screenshot.png", true, true);

        // Assert
        result.Data.Should().HaveCount(48000).And.BeEquivalentTo(await File.ReadAllBytesAsync("testData/Screenshot_bw.blob"));
        result.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Test]
    public async Task ProcessImage_ShouldReturnEmptyIfImageHasInvalidDimensions()
    {
        // Arrange
        var testee = new ImageProcessor(new Mock<ILogger<ImageProcessor>>().Object);

        // Act
        var result = await testee.ProcessAsync("testData/Screenshot_invalid_dimensions.png", true, true);

        // Assert
        result.Data.Should().HaveCount(0);
        result.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }
}