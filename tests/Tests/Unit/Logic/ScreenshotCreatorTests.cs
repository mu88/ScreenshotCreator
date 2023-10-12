using FluentAssertions;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Moq;
using ScreenshotCreator.Logic;

namespace Tests.Unit.Logic;

[TestFixture]
[Category("Unit")]
public class ScreenshotCreatorTests
{
    [TestCase("")]
    [TestCase("/custom")]
    public async Task CreateScreenshotWithoutLoginByConfig(string subresource)
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = $"https://www.mysite.com{subresource}", UrlType = UrlType.Any };
        var playwrightHelperMock = new Mock<IPlaywrightHelper>();
        var pageMock = new Mock<IPage>();
        pageMock
            .Setup(page => page.ScreenshotAsync(It.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFileName &&
                                                                                        options.Type == ScreenshotType.Png)))
            .ReturnsAsync(await File.ReadAllBytesAsync("testData/Screenshot.png"));
        playwrightHelperMock.Setup(helper => helper.InitializePlaywrightAsync()).ReturnsAsync(pageMock.Object);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock.Object,
                                                                   Options.Create(screenshotOptions),
                                                                   NullLogger<ScreenshotCreator.Logic.ScreenshotCreator>.Instance);

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        var screenshot = new MagickImage(await File.ReadAllBytesAsync("testData/Screenshot.png"));
        screenshot.ColorSpace.Should().Be(ColorSpace.sRGB);
        screenshot.Width.Should().Be(800);
        screenshot.Height.Should().Be(480);
        pageMock.Verify(page => page.SetViewportSizeAsync(800, 480), Times.Once);
        pageMock.Verify(page => page.GotoAsync(screenshotOptions.Url, null), Times.Once);
        playwrightHelperMock.Verify(helper => helper.InitializePlaywrightAsync(), Times.Once);
        playwrightHelperMock.Verify(helper => helper.WaitAsync(), Times.Once);
    }

    [TestCase("")]
    [TestCase("/custom")]
    public async Task CreateScreenshotWithoutLoginByPageContent(string subresource)
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = $"https://www.mysite.com{subresource}", UrlType = UrlType.OpenHab };
        var playwrightHelperMock = new Mock<IPlaywrightHelper>();
        var pageMock = new Mock<IPage>();
        pageMock
            .Setup(page => page.ScreenshotAsync(It.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFileName &&
                                                                                        options.Type == ScreenshotType.Png)))
            .ReturnsAsync(await File.ReadAllBytesAsync("testData/Screenshot.png"));
        pageMock.Setup(page => page.GetByText("You are not allowed to view this page because of visibility restrictions.", null).CountAsync()).ReturnsAsync(0);
        playwrightHelperMock.Setup(helper => helper.InitializePlaywrightAsync()).ReturnsAsync(pageMock.Object);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock.Object,
                                                                   Options.Create(screenshotOptions),
                                                                   NullLogger<ScreenshotCreator.Logic.ScreenshotCreator>.Instance);

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        var screenshot = new MagickImage(await File.ReadAllBytesAsync("testData/Screenshot.png"));
        screenshot.ColorSpace.Should().Be(ColorSpace.sRGB);
        screenshot.Width.Should().Be(800);
        screenshot.Height.Should().Be(480);
        pageMock.Verify(page => page.SetViewportSizeAsync(800, 480), Times.Once);
        pageMock.Verify(page => page.GotoAsync(screenshotOptions.Url, null), Times.Exactly(2));
        playwrightHelperMock.Verify(helper => helper.InitializePlaywrightAsync(), Times.Once);
        playwrightHelperMock.Verify(helper => helper.WaitAsync(), Times.Exactly(2));
    }

    [TestCase("", 3)]
    [TestCase("/custom", 2)]
    public async Task CreateScreenshotWithLogin(string subresource, int expectedCallsOfGoto)
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = $"https://www.mysite.com{subresource}", UrlType = UrlType.OpenHab };
        var playwrightHelperMock = new Mock<IPlaywrightHelper>();
        var pageMock = new Mock<IPage>();
        pageMock.Setup(page => page.GetByPlaceholder("User Name", null).FillAsync(screenshotOptions.Username, null));
        pageMock.Setup(page => page
                           .GetByPlaceholder("Password", It.Is<PageGetByPlaceholderOptions>(options => options.Exact == true))
                           .FillAsync(screenshotOptions.Password, null));
        pageMock.Setup(page => page.GetByRole(AriaRole.Button, null).ClickAsync(null));
        pageMock.Setup(page => page.GetByText("You are not allowed to view this page because of visibility restrictions.", null).CountAsync()).ReturnsAsync(1);
        playwrightHelperMock.Setup(helper => helper.InitializePlaywrightAsync()).ReturnsAsync(pageMock.Object);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock.Object,
                                                                   Options.Create(screenshotOptions),
                                                                   new Mock<ILogger<ScreenshotCreator.Logic.ScreenshotCreator>>().Object);

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        pageMock.Verify(page => page.GetByPlaceholder("User Name", null).FillAsync(screenshotOptions.Username, null), Times.Once);
        pageMock.Verify(page => page
                            .GetByPlaceholder("Password", It.Is<PageGetByPlaceholderOptions>(options => options.Exact == true))
                            .FillAsync(screenshotOptions.Password, null),
                        Times.Once);
        pageMock.Verify(page => page.GetByRole(AriaRole.Button, null).ClickAsync(null), Times.Once);
        pageMock.Verify(page => page.SetViewportSizeAsync(800, 480), Times.Once);
        pageMock.Verify(page => page.GotoAsync(screenshotOptions.Url, null), Times.Exactly(expectedCallsOfGoto));
        pageMock.Verify(page => page.ScreenshotAsync(It.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFileName &&
                                                                                             options.Type == ScreenshotType.Png)),
                        Times.Once);
        pageMock.Verify(page => page.GetByText("You are not allowed to view this page because of visibility restrictions.", null).CountAsync(), Times.Once);
        playwrightHelperMock.Verify(helper => helper.InitializePlaywrightAsync(), Times.Once);
        playwrightHelperMock.Verify(helper => helper.WaitAsync(), Times.Exactly(4));
    }
}