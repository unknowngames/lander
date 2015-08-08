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

        private GameScore(int scorePoints, int landingsCount)
        {
            this.scorePoints = scorePoints;
            this.landingsCount = landingsCount;
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