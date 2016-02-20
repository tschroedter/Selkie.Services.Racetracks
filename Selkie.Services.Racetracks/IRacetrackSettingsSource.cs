namespace Selkie.Services.Racetracks
{
    public interface IRacetrackSettingsSource
    {
        bool IsPortTurnAllowed { get; }
        bool IsStarboardTurnAllowed { get; }
        double TurnRadiusForStarboard { get; }
        double TurnRadiusForPort { get; }
    }
}