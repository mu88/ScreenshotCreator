using FluentAssertions;
using ImageMagick;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using ScreenshotCreator.Logic;

namespace Tests.Unit.Logic;

[TestFixture]
[Category("Unit")]
public class ScreenshotCreatorTests
{
    [Test]
    public async Task CreateScreenshotWithoutLogin()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = "https://www.dynamo-dresden.de", UrlType = UrlType.Any };
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(Options.Create(screenshotOptions),
                                                                   NullLogger<ScreenshotCreator.Logic.ScreenshotCreator>.Instance);

        // Act
        await testee.CreateScreenshotAsync(800, 600);

        // Assert
        var screenshot = new MagickImage(await File.ReadAllBytesAsync("Screenshot.png"));
        screenshot.ColorSpace.Should().Be(ColorSpace.sRGB);
        screenshot.Width.Should().Be(800);
        screenshot.Height.Should().Be(600);
    }
}