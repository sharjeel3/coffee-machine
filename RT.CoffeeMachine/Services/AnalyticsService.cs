namespace RT.CoffeeMachine.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly Dictionary<string, int?> _analytics = [];

    public void Track(string trackId)
    {
        if (!_analytics.TryGetValue(trackId, out _))
        {
            _analytics.Add(trackId, 1);
            return;
        }

        _analytics[trackId] = _analytics[trackId] + 1;
    }

    public int GetCount(string trackId)
    {
        return (int)(_analytics.TryGetValue(trackId, out _) 
            ? _analytics[trackId] 
            : 0);
    }

    public void Invalidate(string trackId)
    {
        if (!_analytics.TryGetValue(trackId, out _))
        {
            return;
        }
        _analytics[trackId] = 0;
    }
}
