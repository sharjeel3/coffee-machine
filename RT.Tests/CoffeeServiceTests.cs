using RT.Tests.Fixtures;
using RT.CoffeeMachine.Services;
using RT.CoffeeMachine.Exceptions;
using Moq;
using FluentAssertions;

namespace RT.Tests;

public class CoffeeServiceTests(AnalyticsServiceFixture analyticsServiceFixture,
    DateTimeServiceFixture dateTimeServiceFixture) 
    : IClassFixture<AnalyticsServiceFixture>,
        IClassFixture<DateTimeServiceFixture>
{
    [Fact]
    public void CoffeeService__GetCoffee__WhenCalled_ReturnsCoffeeResponseMessage()
    {
        // Assert
        analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(1);
        var dateTimeService = new DateTimeService();
        var location = "Australia/Brisbane";

        var service = new CoffeeService(analyticsServiceFixture.AnalyticsService.Object,
                dateTimeService);

        // Act
        var coffeeResponse = service.GetCoffee(location);

        // Arrange
        coffeeResponse.Message.Should().Be("Your piping hot coffee is ready");
    }

    [Fact]
    public void CoffeeService__GetCoffee__WhenCalled_ReturnsCoffeePreparedDateInIsoFormat()
    {
        // Assert
        var testDate = new DateTime(2024, 12, 22, 12, 0, 0, DateTimeKind.Utc);
        
        analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(1);
        dateTimeServiceFixture.DateTimeService
            .Setup(x => x.GetNowInUtc())
            .Returns(testDate);

        var location = "Australia/Brisbane";

        var service = new CoffeeService(analyticsServiceFixture.AnalyticsService.Object,
                dateTimeServiceFixture.DateTimeService.Object);

        var expected = "2024-12-22T22:00:00+10:00";

        // Act
        var coffeeResponse = service.GetCoffee(location);

        // Arrange
        coffeeResponse.Prepared.Should().Be(expected);
    }

    [Fact]
    public void CoffeeService__GetCoffee__WhenCalledFiveTimes_ThrowsException()
    {
        // Assert
        analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(5);
        var dateTimeService = new DateTimeService();
        var location = "Australia/Brisbane";

        var service = new CoffeeService(analyticsServiceFixture.AnalyticsService.Object,
                dateTimeService);

        // Act
        var exception = Record.Exception(() => service.GetCoffee(location));

        // Arrange
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ServiceNotAvailableException>();
    }

    [Fact]
    public void CoffeeService__GetCoffee__WhenCalledOnFirstApril_ReturnsNoCoffee()
    {
        // Assert
        var testDate = new DateTime(2024, 04, 01, 12, 0, 0, DateTimeKind.Utc);

        analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(1);
        dateTimeServiceFixture.DateTimeService
            .Setup(x => x.GetNowInUtc())
            .Returns(testDate);

        var location = "Australia/Brisbane";

        var service = new CoffeeService(analyticsServiceFixture.AnalyticsService.Object,
                dateTimeServiceFixture.DateTimeService.Object);

        // Act
        var coffeeResponse = service.GetCoffee(location);

        // Arrange
        coffeeResponse.Should().BeNull();
    }
}
