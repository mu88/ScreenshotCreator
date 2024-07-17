using System.Diagnostics;
using FluentAssertions;
using Microsoft.Extensions.Options;
using ScreenshotCreator.Logic;

namespace Tests.Integration.Logic;

[TestFixture]
[Category("Integration")]
public class PlaywrightHelperTests : PlaywrightTests
{
    [Test]
    public void CreatePlaywrightFacade_ShouldCreateNewObjectEveryTime()
    {
        // Arrange
        var testee = new PlaywrightHelper(Options.Create(new ScreenshotOptions()));

        // Act
        var result1 = testee.CreatePlaywrightFacade();
        var result2 = testee.CreatePlaywrightFacade();

        // Assert
        result1.Should().NotBeSameAs(result2);
    }

    [Test]
    public async Task Wait_ShouldWaitTheConfiguredAmountOfSeconds()
    {
        // Arrange
        var testee = new PlaywrightHelper(Options.Create(new ScreenshotOptions { TimeBetweenHttpCallsInSeconds = 1 }));

        // Act
        var stopwatch = Stopwatch.StartNew();
        await testee.WaitAsync();
        stopwatch.Stop();

        // Assert
        stopwatch.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(100));
    }
}