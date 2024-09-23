using RT.CoffeeMachine.Constants;
using RT.CoffeeMachine.Exceptions;
using RT.CoffeeMachine.Models;
using RT.CoffeeMachine.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RT.CoffeeMachine.Controllers;

[ApiController]
[Route("brew-coffee")]
public class CoffeeController(ICoffeeService coffeeService,
    IAnalyticsService analyticsService) : ControllerBase
{
    [HttpGet]
    public ActionResult<CoffeeResponse> GetCoffee([FromHeader(Name = "location")][Required] string location)
    {
        try
        {
            analyticsService.Track(TrackingTypes.CoffeeOrders);

            if (string.IsNullOrEmpty(location))
            {
                return BadRequest();
            }

            var coffee = coffeeService.GetCoffee(location);

            if (coffee == null)
            {
                return StatusCode(418, null);
            }

            return Ok(coffee);
        }
        catch (ServiceNotAvailableException)
        {
            return StatusCode(503, null);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = ex.Message
            });
        }
        catch
        {
            return StatusCode(500);
        }
    }
}
