using FluentAssertions;
using FluentAssertions.Extensions;
using NSubstitute;
using ScreenshotCreator.Logic;

namespace Tests.Unit.Logic;

[TestFixture]
[Category("Unit")]
public class ScreenshotOptionsTests
{
    [TestCase(true, 90u, 120u, "90")]
    [TestCase(false, 90u, 120u, "120")]
    public void CalculateSleepBetweenUpdate(bool isNull, uint refreshIntervalInSeconds, uint refreshIntervalWhenInactiveInSeconds, string expectedResult)
    {
        // Arrange
        Environment.SetEnvironmentVariable("TZ", null);
        var timeProviderMock = Substitute.For<TimeProvider>();
        // Active window 13:00–15:00, current time 16:00 → inactive → RefreshIntervalWhenInactiveInSeconds
        // null activity → always active → RefreshIntervalInSeconds
        timeProviderMock.GetUtcNow().Returns(12.April(2023).At(16, 0).AsUtc());
        var activeFrom = TimeOnly.Parse("13:00");
        var activeTo = TimeOnly.Parse("15:00");
        var activity = isNull ? null : new Activity(activeFrom, activeTo, refreshIntervalWhenInactiveInSeconds);
        var testee = new ScreenshotOptions { Activity = activity, RefreshIntervalInSeconds = refreshIntervalInSeconds };

        // Act
        var result = testee.CalculateSleepBetweenUpdates(timeProviderMock);

        // Assert
        result.Should().Be(expectedResult);
    }
}
