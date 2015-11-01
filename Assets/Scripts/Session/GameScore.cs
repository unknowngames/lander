using System;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Session
{
    [Serializable]
    public class GameScore : IGameScore
    {
        [SerializeField]
        private int scorePoints;
        [SerializeField]
        private int landingsCount;

        private int successLandingScorePoints = 0;
        private int softLandingScorePoints = 0;
        private int preciseLandingScorePoints = 0;
		private int landingTimeScorePoints = 0;
        private float fuelConsumptionScorePointsPoints = 0;

        private GameScore(int scorePoints, int landingsCount, long currentTopScore)
        {
            this.scorePoints = scorePoints;
            this.landingsCount = landingsCount;
			CurrentTopScore = currentTopScore;

			successLandingScorePoints = 0;
			softLandingScorePoints = 0;
			preciseLandingScorePoints = 0;
			landingTimeScorePoints = 0;
            fuelConsumptionScorePointsPoints = 0;
        }

		public long CurrentTopScore 
		{
			get;
			private set;
		}

        public int ScorePoints
        {
            get { return scorePoints; }
            set { scorePoints = value; }
        }

		public int LastFlightScorePoints 
		{
			get;
			set;
		}

		public bool IsTopScoreBeaten 
		{
			get
			{
				return CurrentTopScore < ScorePoints;
			}
		}

        public int LandingsCount
        {
            get { return landingsCount; }
            set { landingsCount = value; }
        }

        public float FuelBonus
        {
            get; set;
        }

		public float LandingTime 
		{
			get;
			set;
		}

        public int SuccessLandingScore
        {
            get
            {
                return successLandingScorePoints;
            }
            set
            {
                successLandingScorePoints = value;
            }
        }

        public int SoftLandingScore
        {
            get
            {
                return softLandingScorePoints;
            }
            set
            {
                softLandingScorePoints = value;
            }
        }

        public int PreciseLandingScore
        {
            get
            {
                return preciseLandingScorePoints;
            }
            set
            {
                preciseLandingScorePoints = value;
            }
        }

		public int LandingTimeScore
		{
			get
			{
				return landingTimeScorePoints;
			}
			set
			{
				landingTimeScorePoints = value;
			}
		}

        public float FuelConsumptionScorePoints
		{
			get
			{
                return fuelConsumptionScorePointsPoints;
			}
			set
			{
                fuelConsumptionScorePointsPoints = value;
			}
		}

        public static GameScore Create(int scorePoints, int landingsCount, long currentTopScore)
        {
            return new GameScore(scorePoints, landingsCount, currentTopScore);
        }

        public static GameScore Create(IGameScore score)
        {
			var result = new GameScore(score.ScorePoints, score.LandingsCount, score.CurrentTopScore);
			return result;
        }
    }
}