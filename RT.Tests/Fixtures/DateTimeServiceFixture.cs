using RT.CoffeeMachine.Services;
using Moq;

namespace RT.Tests.Fixtures;

public class DateTimeServiceFixture
{
    public readonly Mock<IDateTimeService> DateTimeService;

    public DateTimeServiceFixture()
    {
        DateTimeService = new Mock<IDateTimeService>();
    }
}
