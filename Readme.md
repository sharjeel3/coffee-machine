# Internet Coffee Machine

This is a Web API created using .NET 8. You'll need .NET 8 runtime to run this application.

You can run the app using IIS Express in Visual Studio. 

It contains one endpoint:
```
/brew-coffee
```
Which has one required parameter `location`. Example location header would be:
```
Australia/Brisbane
```

We can use one of the values from `TZ Identifier` mentioned on IANA database https://en.wikipedia.org/wiki/List_of_tz_database_time_zones#List
 
## Greeting
Normally the endpoint returns a `message` and the `prepared` timestamp in specified timezone.

```json
{
    "message": "Your piping hot coffee is ready",
    "prepared": "2024-09-22T11:16:02+10:00"
}
```

**Time off exception**: Every fifth API request returns 503 response code.


## April 1st (Tea day)

The coffee service returns 418 response code if the today's date is April 1 in specified timezone.

This will be the case for all the requests sent on this day.

## Solution design

### AnalyticsService
It tracks the requests sent to the endpoint.

### DateTimeService
It returns the current time in UTC.

### CoffeeService
Main application logic lives here such as managing the response based on number of API requests. 


## Tests

I have added some integration and unit tests to cover for the business logic.
