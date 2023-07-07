using FluentAssertions;
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
        var activeFrom = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(-1);
        var activeTo = TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(1);
        var activity = isNull ? null : new Activity(activeFrom, activeTo, refreshIntervalWhenInactiveInSeconds);
        var testee = new ScreenshotOptions { Activity = activity, RefreshIntervalInSeconds = refreshIntervalInSeconds };

        // Act
        var result = testee.CalculateSleepBetweenUpdates();

        // Assert
        result.Should().Be(expectedResult);
    }
}