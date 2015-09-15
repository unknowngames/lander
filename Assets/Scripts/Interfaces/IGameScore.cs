namespace Assets.Scripts.Interfaces
{
    public interface IGameScore
    {
        int ScorePoints { get; }

        int LandingsCount { get; }

        int SuccessLandingScore { get; }
        int SoftLandingScore { get; }
        int PreciseLandingScore { get; }
		int LandingTimeScore { get; }
		float LandingTime { get; }
		float FuelConsumptionScore { get; }
    } 
}