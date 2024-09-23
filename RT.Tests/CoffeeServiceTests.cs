using RT.Tests.Fixtures;
using RT.CoffeeMachine.Services;
using RT.CoffeeMachine.Exceptions;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace RT.Tests;

public class CoffeeServiceTests : IClassFixture<AnalyticsServiceFixture>,
        IClassFixture<DateTimeServiceFixture>
{
    private readonly Mock<ILogger<ICoffeeService>> _mockLogger;
    private readonly AnalyticsServiceFixture _analyticsServiceFixture;
    private readonly DateTimeServiceFixture _dateTimeServiceFixture;

    public CoffeeServiceTests(AnalyticsServiceFixture analyticsServiceFixture,
        DateTimeServiceFixture dateTimeServiceFixture)
    {
        _mockLogger = new Mock<ILogger<ICoffeeService>>();
        _analyticsServiceFixture = analyticsServiceFixture;
        _dateTimeServiceFixture = dateTimeServiceFixture;
    }

    [Fact]
    public void CoffeeService__GetCoffee__WhenCalled_ReturnsCoffeeResponseMessage()
    {
        // Assert
        _analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(1);
        var dateTimeService = new DateTimeService();
        var location = "Australia/Brisbane";

        var service = new CoffeeService(_analyticsServiceFixture.AnalyticsService.Object,
                dateTimeService, _mockLogger.Object);

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
        
        _analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(1);
        _dateTimeServiceFixture.DateTimeService
            .Setup(x => x.GetNowInUtc())
            .Returns(testDate);

        var location = "Australia/Brisbane";

        var service = new CoffeeService(_analyticsServiceFixture.AnalyticsService.Object,
                _dateTimeServiceFixture.DateTimeService.Object, _mockLogger.Object);

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
        _analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(5);
        var dateTimeService = new DateTimeService();
        var location = "Australia/Brisbane";

        var service = new CoffeeService(_analyticsServiceFixture.AnalyticsService.Object,
                dateTimeService, _mockLogger.Object);

        // Act
        var exception = Record.Exception(() => service.GetCoffee(location));

        // Arrange
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ServiceNotAvailableException>();
    }

    [Fact]
    public void CoffeeService__GetCoffee__WhenCalledWithBadLocation_ThrowsException()
    {
        // Assert
        _analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(1);
        var dateTimeService = new DateTimeService();
        var location = "SuperBigLocation";

        var service = new CoffeeService(_analyticsServiceFixture.AnalyticsService.Object,
                dateTimeService, _mockLogger.Object);

        // Act
        var exception = Record.Exception(() => service.GetCoffee(location));

        // Arrange
        exception.Should().NotBeNull();
        exception.Should().BeOfType<BadRequestException>();

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) => string.Equals(state.ToString(), $"{nameof(CoffeeService)} received invalid location")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
    }

    [Fact]
    public void CoffeeService__GetCoffee__WhenCalledOnFirstApril_ReturnsNoCoffee()
    {
        // Assert
        var testDate = new DateTime(2024, 04, 01, 12, 0, 0, DateTimeKind.Utc);

        _analyticsServiceFixture.AnalyticsService
            .Setup(x => x.GetCount(It.IsAny<string>()))
            .Returns(1);
        _dateTimeServiceFixture.DateTimeService
            .Setup(x => x.GetNowInUtc())
            .Returns(testDate);

        var location = "Australia/Brisbane";

        var service = new CoffeeService(_analyticsServiceFixture.AnalyticsService.Object,
                _dateTimeServiceFixture.DateTimeService.Object, _mockLogger.Object);

        // Act
        var coffeeResponse = service.GetCoffee(location);

        // Arrange
        coffeeResponse.Should().BeNull();
    }
}
