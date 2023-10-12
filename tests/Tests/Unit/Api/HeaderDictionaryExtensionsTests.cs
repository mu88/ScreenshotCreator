using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using ScreenshotCreator.Api;
using ScreenshotCreator.Logic;

namespace Tests.Unit.Api;

[TestFixture]
[Category("Unit")]
public class HeaderDictionaryExtensionsTests
{
    [TestCase(true,
                 "",
                 "",
                 "15:00",
                 true,
                 "1953")]
    [TestCase(false,
                 "16:00",
                 "13:00",
                 "15:00",
                 false,
                 "15")]
    public void AddWaveshareInstructions(bool isNull,
                                         string activeFrom,
                                         string activeTo,
                                         string now,
                                         bool expectedDisplayState,
                                         string expectedSleep)
    {
        // Arrange
        Environment.SetEnvironmentVariable("TZ", null);
        var timeProviderMock = new Mock<TimeProvider>();
        timeProviderMock.Setup(provider => provider.GetUtcNow()).Returns(12.April(1953).Add(TimeOnly.Parse(now).ToTimeSpan()));
        var activity = isNull ? null : new Activity(TimeOnly.Parse(activeFrom), TimeOnly.Parse(activeTo), 15u);
        var getLastWriteTimeUtc = (string file) => 12.April(2023).At(19, 53).AsUtc();
        var screenshotOptions = new ScreenshotOptions { RefreshIntervalInSeconds = 1953, Activity = activity };
        var testee = new HeaderDictionary();

        // Act
        testee.AddWaveshareInstructions(screenshotOptions, "testData/Screenshot.png", getLastWriteTimeUtc, "Europe/Berlin");

        // Assert
        testee.Should().Contain(header => header.Key == "waveshare-update-screen" && header.Value.Single() == expectedDisplayState.ToString());
        testee.Should().Contain(header => header.Key == "waveshare-sleep-between-updates" && header.Value.Single() == expectedSleep);
        testee.Should().Contain(header => header.Key == "waveshare-last-modified-local-time");
        TimeOnly.Parse(testee["waveshare-last-modified-local-time"].Single()!)
            .Should()
            .BeCloseTo(TimeOnly.Parse("21:53"), TimeSpan.FromSeconds(1));
    }

    [TestCase("America/Havana", "15:53")]
    [TestCase("Europe/Berlin", "21:53")]
    public void AddWaveshareInstructions_ShouldConsumeTimezoneIdFromEnvironmentVariable(string timeZoneId, string expectedTime)
    {
        // Arrange
        Environment.SetEnvironmentVariable("TZ", timeZoneId);
        var getLastWriteTimeUtc = (string file) => 12.April(2023).At(19, 53).AsUtc();
        var screenshotOptions = new ScreenshotOptions { RefreshIntervalInSeconds = 1953 };
        var testee = new HeaderDictionary();

        // Act
        testee.AddWaveshareInstructions(screenshotOptions, "testData/Screenshot.png", getLastWriteTimeUtc);

        // Assert
        testee.Should().Contain(header => header.Key == "waveshare-last-modified-local-time");
        TimeOnly.Parse(testee["waveshare-last-modified-local-time"].Single()!)
            .Should()
            .BeCloseTo(TimeOnly.Parse(expectedTime), TimeSpan.FromSeconds(10));
    }
}