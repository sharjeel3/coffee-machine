namespace RT.CoffeeMachine.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime GetNowInUtc()
    {
        return DateTime.UtcNow;
    }
}
