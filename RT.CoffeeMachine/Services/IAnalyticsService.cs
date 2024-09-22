namespace RT.CoffeeMachine.Services;

public interface IAnalyticsService
{
    public void Track(string trackId);
    public int GetCount(string trackId);
    public void Invalidate(string trackId);
}
