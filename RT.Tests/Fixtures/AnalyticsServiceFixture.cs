using RT.CoffeeMachine.Services;
using Moq;

namespace RT.Tests.Fixtures;

public class AnalyticsServiceFixture
{
    public Mock<IAnalyticsService> AnalyticsService;
    public AnalyticsServiceFixture()
    {
        AnalyticsService = new Mock<IAnalyticsService>();
    }
}
