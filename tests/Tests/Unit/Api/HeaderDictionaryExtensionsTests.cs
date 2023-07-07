using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using ScreenshotCreator.Api;
using ScreenshotCreator.Logic;

namespace Tests.Unit.Api;

[TestFixture]
[Category("Unit")]
public class HeaderDictionaryExtensionsTests
{
    [Test]
    public void AddWaveshareInstructions()
    {
        // Arrange
        Environment.SetEnvironmentVariable("TZ", null);
        var getLastWriteTimeUtc = (string file) => 12.April(2023).At(19, 53).AsUtc();
        var screenshotOptions = new ScreenshotOptions { RefreshIntervalInSeconds = 1953 };
        var testee = new HeaderDictionary();

        // Act
        testee.AddWaveshareInstructions(screenshotOptions, "testData/Screenshot.png", getLastWriteTimeUtc, "Europe/Berlin");

        // Assert
        testee.Should().Contain(header => header.Key == "waveshare-update-screen" && header.Value.Single() == "True");
        testee.Should().Contain(header => header.Key == "waveshare-sleep-between-updates" && header.Value.Single() == "1953");
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