using RT.CoffeeMachine.Models;

namespace RT.CoffeeMachine.Services;

public interface ICoffeeService
{
    public CoffeeResponse GetCoffee(string location);
}
