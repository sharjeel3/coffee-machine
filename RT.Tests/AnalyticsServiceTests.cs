using RT.CoffeeMachine.Services;
using FluentAssertions;

namespace RT.Tests;

public class AnalyticsServiceTests
{
    [Fact]
    public void AnalyticsService__GetCount__ReturnsDefaultValue()
    {
        // Arrange
        var service = new AnalyticsService();
        var trackId = "trackId";

        // Act
        var count = service.GetCount(trackId);

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public void AnalyticsService__Track__CanRememberHits()
    {
        // Arrange
        var service = new AnalyticsService();
        var trackId = "trackId";

        // Act
        service.Track(trackId);
        var count = service.GetCount(trackId);

        // Assert
        count.Should().Be(1);
    }

    [Fact]
    public void AnalyticsService__Track__WhenCalledMultipleTimes__ReturnsCorrectValue()
    {
        // Arrange
        var service = new AnalyticsService();
        var trackId = "trackId";

        // Act
        service.Track(trackId);
        service.Track(trackId);
        service.Track(trackId);
        var count = service.GetCount(trackId);

        // Assert
        count.Should().Be(3);
    }

    [Fact]
    public void AnalyticsService__Invalidate__ClearsHistoryCount()
    {
        // Arrange
        var service = new AnalyticsService();
        var trackId = "trackId";

        // Act
        service.Track(trackId);
        service.Invalidate(trackId);
        var count = service.GetCount(trackId);

        // Assert
        count.Should().Be(0);
    }
}
