namespace RT.CoffeeMachine.Exceptions;

public class ServiceNotAvailableException : Exception
{
    public ServiceNotAvailableException()
    {
        
    }

    public ServiceNotAvailableException(string message)
        : base(message)
    {
        
    }

    public ServiceNotAvailableException(string message, Exception ex)
        : base(message, ex)
    {
        
    }
}
