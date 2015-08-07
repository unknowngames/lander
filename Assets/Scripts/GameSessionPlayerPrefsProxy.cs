namespace Assets.Scripts
{
    public static class GameSessionPlayerPrefsProxy
    {
        private const string RemainingFuelKey = "RemainingFuelKey";
        private const string ScorePointsKey = "ScorePointsKey";
        private const string LandingsCountKey = "LandingsCountKey";

        public static bool HasSession
        {
            get
            {
                return  PlayerPrefsX.HasKey(RemainingFuelKey) &&
                        PlayerPrefsX.HasKey(ScorePointsKey) &&
                        PlayerPrefsX.HasKey(LandingsCountKey);
            }
        }

        public static void Save(IGameSession session)
        {
            float remainingFuel = session.Spaceship.RemainingFuel;
            int scorePoints = session.Score.ScorePoints;
            int landingsCount = session.Score.LandingsCount;

            PlayerPrefsX.SetFloat(RemainingFuelKey, remainingFuel);
            PlayerPrefsX.SetInt(ScorePointsKey, scorePoints);
            PlayerPrefsX.SetInt(LandingsCountKey, landingsCount);
        }

        public static IGameSession Restore()
        {
            float remainingFuel = PlayerPrefsX.GetFloat(RemainingFuelKey);
            int scorePoints = PlayerPrefsX.GetInt(ScorePointsKey);
            int landingsCount = PlayerPrefsX.GetInt(LandingsCountKey);

            return GameSession.Create(SpaceshipState.Create(remainingFuel), GameScore.Create(scorePoints, landingsCount));
        }

        public static void Remove()
        {
            PlayerPrefsX.DeleteKey(RemainingFuelKey);
            PlayerPrefsX.DeleteKey(ScorePointsKey);
            PlayerPrefsX.DeleteKey(LandingsCountKey);
        }
    }
}