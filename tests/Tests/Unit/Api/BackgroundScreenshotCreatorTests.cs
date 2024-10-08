﻿using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
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
        var screenshotCreatorMock = Substitute.For<IScreenshotCreator>();
        var testee = new BackgroundScreenshotCreator(screenshotCreatorMock,
            Options.Create(screenshotOptions),
            NullLogger<BackgroundScreenshotCreator>.Instance);

        // Act
        testee.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        cancellationTokenSource.Cancel();

        // Assert
        await screenshotCreatorMock.Received(2).CreateScreenshotAsync(800, 600);
        await screenshotCreatorMock.Received(2).CreateScreenshotAsync(800, 600);
    }

    [Test]
    public async Task ProcessInBackground_ShouldDoNothing_IfDisabled()
    {
        // Arrange
        var screenshotOptions = new ScreenshotOptions { RefreshIntervalInSeconds = 1, BackgroundProcessingEnabled = false, Width = 800, Height = 600 };
        var cancellationTokenSource = new CancellationTokenSource();
        var screenshotCreatorMock = Substitute.For<IScreenshotCreator>();
        var testee = new BackgroundScreenshotCreator(screenshotCreatorMock,
            Options.Create(screenshotOptions),
            NullLogger<BackgroundScreenshotCreator>.Instance);

        // Act
        testee.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        cancellationTokenSource.Cancel();

        // Assert
        await screenshotCreatorMock.DidNotReceive().CreateScreenshotAsync(800, 600);
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
        var screenshotCreatorMock = Substitute.For<IScreenshotCreator>();
        var testee = new BackgroundScreenshotCreator(screenshotCreatorMock,
            Options.Create(screenshotOptions),
            NullLogger<BackgroundScreenshotCreator>.Instance);

        // Act
        testee.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(TimeSpan.FromSeconds(1.5));
        cancellationTokenSource.Cancel();

        // Assert
        await screenshotCreatorMock.Received(2).CreateScreenshotAsync(800, 600);
    }
}