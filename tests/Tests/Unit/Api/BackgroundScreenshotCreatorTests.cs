using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using ScreenshotCreator.Api;
using ScreenshotCreator.Logic;

#pragma warning disable CS4014

namespace Tests.Unit.Api;

[TestFixture]
[Category("Unit")]
public class BackgroundScreenshotCreatorTests
{
    [Test]
    public async Task ProcessInBackground()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { RefreshIntervalInSeconds = 1, BackgroundProcessingEnabled = true, Width = 800, Height = 600 };
        var cancellationTokenSource = new CancellationTokenSource();
        var screenshotCreatorMock = new Mock<IScreenshotCreator>();
        var testee = new BackgroundScreenshotCreator(screenshotCreatorMock.Object,
                                                     Options.Create(screenshotOptions),
                                                     NullLogger<BackgroundScreenshotCreator>.Instance);

        // Act
        testee.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        cancellationTokenSource.Cancel();

        // Assert
        screenshotCreatorMock.Verify(creator => creator.CreateScreenshotAsync(800, 600), Times.Exactly(2));
    }

    [Test]
    public async Task ProcessInBackground_ShouldDoNothing_IfDisabled()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { RefreshIntervalInSeconds = 1, BackgroundProcessingEnabled = false, Width = 800, Height = 600 };
        var cancellationTokenSource = new CancellationTokenSource();
        var screenshotCreatorMock = new Mock<IScreenshotCreator>();
        var testee = new BackgroundScreenshotCreator(screenshotCreatorMock.Object,
                                                     Options.Create(screenshotOptions),
                                                     NullLogger<BackgroundScreenshotCreator>.Instance);

        // Act
        testee.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        cancellationTokenSource.Cancel();

        // Assert
        screenshotCreatorMock.Verify(creator => creator.CreateScreenshotAsync(800, 600), Times.Never);
    }

    [Test]
    public async Task ProcessInBackground_ShouldDoNothing_IfNotActive()
    {
        // Arrange
        var activeFrom = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(4);
        var activeTo = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(5);
        var screenshotOptions = new ScreenshotOptions
        {
            RefreshIntervalInSeconds = 1, BackgroundProcessingEnabled = true, Width = 800, Height = 600, Activity = new Activity(activeFrom, activeTo, 90u)
        };
        var cancellationTokenSource = new CancellationTokenSource();
        var screenshotCreatorMock = new Mock<IScreenshotCreator>();
        var testee = new BackgroundScreenshotCreator(screenshotCreatorMock.Object,
                                                     Options.Create(screenshotOptions),
                                                     NullLogger<BackgroundScreenshotCreator>.Instance);

        // Act
        testee.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        cancellationTokenSource.Cancel();

        // Assert
        screenshotCreatorMock.Verify(creator => creator.CreateScreenshotAsync(800, 600), Times.Once);
    }
}