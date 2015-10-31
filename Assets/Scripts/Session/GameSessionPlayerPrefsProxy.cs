using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public static class GameSessionPlayerPrefsProxy
    {
        private const string RemainingFuelKey = "RemainingFuelKey";
        private const string ScorePointsKey = "ScorePointsKey";
        private const string LandingsCountKey = "LandingsCountKey";
		private const string TopScoreKey = "TopScoreKey";

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

		public static void SaveTopScore(string difficultyName, long score)
		{
			PlayerPrefsX.SetString (difficultyName + TopScoreKey, score.ToString ());
		}

		public static long GetTopScore(string difficultyName)
		{
			var strResult = PlayerPrefsX.GetString (difficultyName + TopScoreKey);
			long result;
			if (long.TryParse (strResult, out result))
				return result;
			else
				return long.MinValue;
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