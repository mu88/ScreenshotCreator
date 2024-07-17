using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using NSubstitute;
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
        var pageMock = Substitute.For<IPage>();
        var playwrightFacadeMock = Substitute.For<IPlaywrightFacade>();
        playwrightFacadeMock.GetPlaywrightPageAsync().Returns(pageMock);
        var playwrightHelperMock = Substitute.For<IPlaywrightHelper>();
        playwrightHelperMock.CreatePlaywrightFacade().Returns(playwrightFacadeMock);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock,
                                                                   Options.Create(screenshotOptions),
                                                                   NullLogger<ScreenshotCreator.Logic.ScreenshotCreator>.Instance);

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        await pageMock.Received(1).SetViewportSizeAsync(800, 480);
        await pageMock.Received(1).GotoAsync(screenshotOptions.Url);
        await pageMock.Received(1)
            .ScreenshotAsync(Arg.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFile &&
                                                                      options.Type == ScreenshotType.Png));
        playwrightHelperMock.Received(1).CreatePlaywrightFacade();
        await playwrightHelperMock.Received(1).WaitAsync();
        await playwrightFacadeMock.Received(1).GetPlaywrightPageAsync();
    }

    [TestCase("")]
    [TestCase("/custom")]
    public async Task CreateScreenshotWithoutLoginByPageContent(string subresource)
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = $"https://www.mysite.com{subresource}", UrlType = UrlType.OpenHab };
        var pageMock = Substitute.For<IPage>();
        pageMock.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync().Returns(0);
        var playwrightFacadeMock = Substitute.For<IPlaywrightFacade>();
        playwrightFacadeMock.GetPlaywrightPageAsync().Returns(pageMock);
        var playwrightHelperMock = Substitute.For<IPlaywrightHelper>();
        playwrightHelperMock.CreatePlaywrightFacade().Returns(playwrightFacadeMock);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock,
                                                                   Options.Create(screenshotOptions),
                                                                   NullLogger<ScreenshotCreator.Logic.ScreenshotCreator>.Instance);

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        await pageMock.Received(1).SetViewportSizeAsync(800, 480);
        await pageMock.Received(2).GotoAsync(screenshotOptions.Url);
        await pageMock.Received(1)
            .ScreenshotAsync(Arg.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFile &&
                                                                      options.Type == ScreenshotType.Png));
        playwrightHelperMock.Received(1).CreatePlaywrightFacade();
        await playwrightHelperMock.Received(2).WaitAsync();
        await playwrightFacadeMock.Received(1).GetPlaywrightPageAsync();
    }

    [TestCase("", 3)]
    [TestCase("/custom", 2)]
    public async Task CreateScreenshotWithLogin(string subresource, int expectedCallsOfGoto)
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = $"https://www.mysite.com{subresource}", UrlType = UrlType.OpenHab };
        var pageMock = Substitute.For<IPage>();
        pageMock.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync().Returns(1);
        pageMock.GetByText("lock_shield_fill").IsVisibleAsync().Returns(true);
        var playwrightFacadeMock = Substitute.For<IPlaywrightFacade>();
        playwrightFacadeMock.GetPlaywrightPageAsync().Returns(pageMock);
        var playwrightHelperMock = Substitute.For<IPlaywrightHelper>();
        playwrightHelperMock.CreatePlaywrightFacade().Returns(playwrightFacadeMock);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock,
                                                                   Options.Create(screenshotOptions),
                                                                   Substitute.For<ILogger<ScreenshotCreator.Logic.ScreenshotCreator>>());

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        await pageMock.GetByPlaceholder("User Name").Received(1).FillAsync(screenshotOptions.Username);
        await pageMock.Received(1)
            .GetByPlaceholder("Password", Arg.Is<PageGetByPlaceholderOptions>(options => options.Exact == true))
            .FillAsync(screenshotOptions.Password);
        await pageMock.GetByRole(AriaRole.Button).Received(1).ClickAsync();
        await pageMock.Received(1).SetViewportSizeAsync(800, 480);
        await pageMock.Received(expectedCallsOfGoto).GotoAsync(screenshotOptions.Url);
        await pageMock.Received(1)
            .ScreenshotAsync(Arg.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFile &&
                                                                      options.Type == ScreenshotType.Png));
        await pageMock.Received(2).GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync();
        playwrightHelperMock.Received(1).CreatePlaywrightFacade();
        await playwrightHelperMock.Received(4).WaitAsync();
        await playwrightFacadeMock.Received(1).GetPlaywrightPageAsync();
    }

    [Test]
    public async Task CreateScreenshotWithLogin_ShouldOpenMenuAndClickLogin_IfLoginPageIsNotShown()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = "https://www.mysite.com", UrlType = UrlType.OpenHab };
        var pageMock = Substitute.For<IPage>();
        pageMock.GetByPlaceholder("User Name").IsVisibleAsync().Returns(false);
        pageMock.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync().Returns(1);
        pageMock.GetByText("lock_shield_fill").IsVisibleAsync().Returns(false);
        var playwrightFacadeMock = Substitute.For<IPlaywrightFacade>();
        playwrightFacadeMock.GetPlaywrightPageAsync().Returns(pageMock);
        var playwrightHelperMock = Substitute.For<IPlaywrightHelper>();
        playwrightHelperMock.CreatePlaywrightFacade().Returns(playwrightFacadeMock);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock,
                                                                   Options.Create(screenshotOptions),
                                                                   Substitute.For<ILogger<ScreenshotCreator.Logic.ScreenshotCreator>>());

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        await pageMock.Received(1)
            .ScreenshotAsync(Arg.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFile &&
                                                                      options.Type == ScreenshotType.Png));
        await pageMock.GetByText("menu").Received(1).ClickAsync();
        pageMock.Received(2).GetByText("lock_shield_fill");
        await pageMock.GetByText("lock_shield_fill").Received(1).ClickAsync();
    }

    [Test]
    public async Task CreateScreenshotWithLogin_ShouldNotOpenMenuAndLogin_IfLoginPageIsAlreadyShown()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = "https://www.mysite.com", UrlType = UrlType.OpenHab };
        var pageMock = Substitute.For<IPage>();
        pageMock.GetByPlaceholder("User Name").IsVisibleAsync().Returns(true);
        pageMock.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync().Returns(1);
        pageMock.GetByText("lock_shield_fill").IsVisibleAsync().Returns(false);
        var playwrightFacadeMock = Substitute.For<IPlaywrightFacade>();
        playwrightFacadeMock.GetPlaywrightPageAsync().Returns(pageMock);
        var playwrightHelperMock = Substitute.For<IPlaywrightHelper>();
        playwrightHelperMock.CreatePlaywrightFacade().Returns(playwrightFacadeMock);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock,
                                                                   Options.Create(screenshotOptions),
                                                                   Substitute.For<ILogger<ScreenshotCreator.Logic.ScreenshotCreator>>());

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        await pageMock.Received(1)
            .ScreenshotAsync(Arg.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFile &&
                                                                      options.Type == ScreenshotType.Png));
        await pageMock.GetByText("menu").DidNotReceive().ClickAsync();
        await pageMock.Received(1).GetByText("lock_shield_fill").ClickAsync();
    }

    [Test]
    public async Task CreateScreenshotWithLogin_ShouldNotCreateScreenshot_IfSiteDoesNotIndicateAvailability()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = "https://www.mysite.com", UrlType = UrlType.OpenHab, AvailabilityIndicator = "Success" };
        var pageMock = Substitute.For<IPage>();
        pageMock.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync().Returns(1);
        pageMock.GetByText(screenshotOptions.AvailabilityIndicator).CountAsync().Returns(0);
        pageMock.GetByText("lock_shield_fill").IsVisibleAsync().Returns(true);
        var playwrightFacadeMock = Substitute.For<IPlaywrightFacade>();
        playwrightFacadeMock.GetPlaywrightPageAsync().Returns(pageMock);
        var playwrightHelperMock = Substitute.For<IPlaywrightHelper>();
        playwrightHelperMock.CreatePlaywrightFacade().Returns(playwrightFacadeMock);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock,
                                                                   Options.Create(screenshotOptions),
                                                                   Substitute.For<ILogger<ScreenshotCreator.Logic.ScreenshotCreator>>());

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        await pageMock.DidNotReceive()
            .ScreenshotAsync(Arg.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFile &&
                                                                      options.Type == ScreenshotType.Png));
    }

    [Test]
    public async Task CreateScreenshotWithLogin_ShouldCreateScreenshot_IfSiteIndicatesAvailability()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { Url = "https://www.mysite.com", UrlType = UrlType.OpenHab, AvailabilityIndicator = "Success" };
        var pageMock = Substitute.For<IPage>();
        pageMock.GetByText("You are not allowed to view this page because of visibility restrictions.").CountAsync().Returns(1);
        pageMock.GetByText(screenshotOptions.AvailabilityIndicator).CountAsync().Returns(1);
        pageMock.GetByText("lock_shield_fill").IsVisibleAsync().Returns(true);
        var playwrightFacadeMock = Substitute.For<IPlaywrightFacade>();
        playwrightFacadeMock.GetPlaywrightPageAsync().Returns(pageMock);
        var playwrightHelperMock = Substitute.For<IPlaywrightHelper>();
        playwrightHelperMock.CreatePlaywrightFacade().Returns(playwrightFacadeMock);
        var testee = new ScreenshotCreator.Logic.ScreenshotCreator(playwrightHelperMock,
                                                                   Options.Create(screenshotOptions),
                                                                   Substitute.For<ILogger<ScreenshotCreator.Logic.ScreenshotCreator>>());

        // Act
        await testee.CreateScreenshotAsync(800, 480);

        // Assert
        await pageMock.Received(1)
            .ScreenshotAsync(Arg.Is<PageScreenshotOptions>(options => options.Path == screenshotOptions.ScreenshotFile &&
                                                                      options.Type == ScreenshotType.Png));
    }
}