using RT.CoffeeMachine.Constants;
using RT.CoffeeMachine.Exceptions;
using RT.CoffeeMachine.Extensions;
using RT.CoffeeMachine.Models;

namespace RT.CoffeeMachine.Services;

public class CoffeeService(IAnalyticsService analyticsService,
    IDateTimeService dateTimeService) : ICoffeeService
{
    private const string Greeting = 
        "Your piping hot coffee is ready";

    public CoffeeResponse GetCoffee(string location)
    {
        var utcNow = dateTimeService.GetNowInUtc();
        var localDate = utcNow.ToLocalDate(location);

        if (localDate.Month is 4 && localDate.Day is 1)
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
            Prepared = localDate.ToString(DateFormats.IsoDateTimeFormat)
        };
    }
}
