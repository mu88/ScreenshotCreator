using FluentAssertions;
using ScreenshotCreator.Logic;

namespace Tests.Integration.Logic;

[TestFixture]
[Category("Integration")]
public class PlaywrightFacadeTests : PlaywrightTests
{
    [Test]
    public async Task GetPlaywrightPage_ShouldInitializePlaywrightAndCreateNewPage()
    {
        // Arrange
        var testee = new PlaywrightFacade();

        // Act
        var result = await testee.GetPlaywrightPageAsync();

        // Assert
        result.Context.Browser.Should().NotBeNull();
    }

    [Test]
    public async Task Dispose_ShouldDisposeUnderlyingPlaywrightSession()
    {
        // Arrange
        var testee = new PlaywrightFacade();
        var result = await testee.GetPlaywrightPageAsync();

        // Act & Assert
        await testee.DisposeAsync();

        // Assert
        var createScreenShotAsync = async () => await result.ScreenshotAsync();
        await createScreenShotAsync.Should().ThrowAsync<Exception>().WithMessage("*disposed*");
    }
}