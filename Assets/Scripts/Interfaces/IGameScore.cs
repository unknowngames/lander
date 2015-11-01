namespace Assets.Scripts.Interfaces
{
    public interface IGameScore
    {
        int ScorePoints { get; }

        int LandingsCount { get; }

		long CurrentTopScore { get; }

        float LandingTime { get; set; }

        int SuccessLandingScore { get; set; }

        int SoftLandingScore { get; set; }

        int PreciseLandingScore { get; set; }

        int LandingTimeScore { get; set; }

        float FuelConsumptionScorePoints { get; set; }

        float FuelBonus { get; set; }

		int LastFlightScorePoints { get; }

		bool IsTopScoreBeaten { get; }
    }
}