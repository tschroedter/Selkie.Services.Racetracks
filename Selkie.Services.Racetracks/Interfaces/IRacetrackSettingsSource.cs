namespace Selkie.Services.Racetracks.Interfaces
{
    public interface IRacetrackSettingsSource
    {
        bool IsPortTurnAllowed { get; }
        bool IsStarboardTurnAllowed { get; }
        double TurnRadiusForStarboard { get; }
        double TurnRadiusForPort { get; }
    }
}