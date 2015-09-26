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

        private GameScore(int scorePoints, int landingsCount)
        {
            this.scorePoints = scorePoints;
            this.landingsCount = landingsCount;
			successLandingScorePoints = 0;
			softLandingScorePoints = 0;
			preciseLandingScorePoints = 0;
			landingTimeScorePoints = 0;
            fuelConsumptionScorePointsPoints = 0;
        }

        public int ScorePoints
        {
            get { return scorePoints; }
            set { scorePoints = value; }
        }

        public int LandingsCount
        {
            get { return landingsCount; }
            set { landingsCount = value; }
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

        public static GameScore Create(int scorePoints, int landingsCount)
        {
            return new GameScore(scorePoints, landingsCount);
        }

        public static GameScore Create(IGameScore score)
        {
            return new GameScore(score.ScorePoints, score.LandingsCount);
        }
    }
}