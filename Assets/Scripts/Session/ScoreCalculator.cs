using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly IGame game;

        private GameScore current;


        public ScoreCalculator(IGame game)
        {
            this.game = game;
        }

        public IGameScore Current
        {
            get { return current; }
        }

        public void SetInitialScore(IGameScore gameScore)
        {
            current = GameScore.Create(gameScore);
        }

        public void Update()
        {
            //current.ScorePoints++;
        }

        public IGameScore Calculate()
        {
            if (game.PlayerSpaceship.IsLanded)
            {
                current.LandingsCount++;
                current.ScorePoints += 50;
                current.ScorePoints += (int)game.PlayerSpaceship.RemainingFuel;
            }

            return current;
        }
    }
}