using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly IGame game;

        private IGameScore initialScore;
        private int scorePoints;


        public ScoreCalculator(IGame game)
        {
            this.game = game;
        }

        public void SetInitialScore(IGameScore gameScore)
        {
            initialScore = gameScore;
            scorePoints = initialScore.ScorePoints;
        }

        public void Update()
        {
            scorePoints++;
        }

        public IGameScore Calculate()
        {
            return GameScore.Create(scorePoints, initialScore.LandingsCount + 1);
        }
    }
}