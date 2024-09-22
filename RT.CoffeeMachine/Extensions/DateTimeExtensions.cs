using NodaTime;

namespace RT.CoffeeMachine.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToLocalDate(this DateTime utcDate, string location)
    {
        var timezone = DateTimeZoneProviders.Tzdb[location];
        var instant = Instant.FromDateTimeUtc(utcDate);
        var localDate = instant.InZone(timezone).ToDateTimeUnspecified();
        return localDate;
    }
}
