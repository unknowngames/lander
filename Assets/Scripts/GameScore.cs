using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class GameScore : IGameScore
    {
        [SerializeField]
        private DateTime timestamp;
        [SerializeField]
        private int scorePoints;
        [SerializeField]
        private int landingsCount;

        private GameScore(int scorePoints, int landingsCount)
        {
            timestamp = DateTime.Now;
            this.scorePoints = scorePoints;
            this.landingsCount = landingsCount;
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
        }

        public int ScorePoints
        {
            get { return scorePoints; }
        }

        public int LandingsCount
        {
            get { return landingsCount; }
        }

        public static GameScore Create(int scorePoints, int landingsCount)
        {
            return new GameScore(scorePoints, landingsCount);
        }
    }
}