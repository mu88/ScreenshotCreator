using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using ScreenshotCreator.Logic;

namespace Tests.Unit.Logic;

[TestFixture]
[Category("Unit")]
public class ActivityExtensionsTests
{
    [TestCase(true, "", "", "15:00", true)]
    [TestCase(false, "13:00", "16:00", "15:00", true)]
    [TestCase(false, "16:00", "13:00", "15:00", false)]
    public void DisplayShouldBeActive(bool isNull, string activeFrom, string activeTo, string now, bool expectedResult)
    {
        // Arrange
        var timeProviderMock = new Mock<TimeProvider>();
        timeProviderMock.Setup(provider => provider.GetUtcNow()).Returns(12.April(1953).Add(TimeOnly.Parse(now).ToTimeSpan()));
        var testee = isNull ? null : new Activity(TimeOnly.Parse(activeFrom), TimeOnly.Parse(activeTo), 0u);

        // Act
        var result = testee.DisplayShouldBeActive(timeProviderMock.Object);

        // Assert
        result.Should().Be(expectedResult);
    }

    [TestCase("America/Havana", false)]
    [TestCase("Europe/Berlin", true)]
    public void DisplayShouldBeActive_ShouldConsumeTimezoneIdFromEnvironmentVariable(string timezoneId, bool expectedResult)
    {
        // Arrange
        Environment.SetEnvironmentVariable("TZ", timezoneId);
        var timeProviderMock = new Mock<TimeProvider>();
        timeProviderMock.Setup(provider => provider.GetUtcNow()).Returns(12.April(1953).Add(TimeOnly.Parse("15:00").ToTimeSpan()));
        var testee = new Activity(TimeOnly.Parse("14:00"), TimeOnly.Parse("16:00"), 0u);

        // Act
        var result = testee.DisplayShouldBeActive(timeProviderMock.Object);

        // Assert
        result.Should().Be(expectedResult);
    }
}