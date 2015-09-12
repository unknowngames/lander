﻿using System;
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

        private GameScore(int scorePoints, int landingsCount, int successLandingScore, int softLandingScore, int preciseLandingScore, int landingTimeScore)
        {
            this.scorePoints = scorePoints;
            this.landingsCount = landingsCount;
			this.successLandingScorePoints = successLandingScore;
			this.softLandingScorePoints = softLandingScore;
			this.preciseLandingScorePoints = preciseLandingScore;
			this.landingTimeScorePoints = landingTimeScore;
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

        public static GameScore Create(int scorePoints, int landingsCount, int successLandingScore, int softLandingScore, int preciseLandingScore, int landingTimeScore)
        {
            return new GameScore(scorePoints, landingsCount, successLandingScore, softLandingScore, preciseLandingScore, landingTimeScore);
        }

        public static GameScore Create(IGameScore score)
        {
            return new GameScore(score.ScorePoints, score.LandingsCount, score.SuccessLandingScore, score.SoftLandingScore, score.PreciseLandingScore, score.LandingTimeScore);
        }
    }
}