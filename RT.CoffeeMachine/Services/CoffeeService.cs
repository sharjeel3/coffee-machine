using RT.CoffeeMachine.Constants;
using RT.CoffeeMachine.Exceptions;
using RT.CoffeeMachine.Extensions;
using RT.CoffeeMachine.Models;

namespace RT.CoffeeMachine.Services;

public class CoffeeService(IAnalyticsService analyticsService,
    IDateTimeService dateTimeService,
    ILogger<ICoffeeService> logger) : ICoffeeService
{
    private const string Greeting = 
        "Your piping hot coffee is ready";

    public CoffeeResponse GetCoffee(string location)
    {
        var utcNow = dateTimeService.GetNowInUtc();
        var localDate = utcNow.ToLocalDate(location);

        if (!localDate.HasValue)
        {
            logger.LogInformation("{service} received invalid location", nameof(CoffeeService));
            throw new BadRequestException("Invalid location");
        }

        if (localDate.Value.Month is 4 && localDate.Value.Day is 1)
        {
            return null;
        }

        var count = analyticsService.GetCount(TrackingTypes.CoffeeOrders);

        if (count % 5 == 0)
        {
            throw new ServiceNotAvailableException();
        }

        return new CoffeeResponse
        {
            Message = Greeting,
            Prepared = localDate.Value.ToString(DateFormats.IsoDateTimeFormat)
        };
    }
}
