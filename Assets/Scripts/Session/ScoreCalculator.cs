using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly IGame game;

        private IGameSession lastSession;
        private GameScore current;


        public ScoreCalculator(IGame game)
        {
            this.game = game;
        }

        public IGameScore Current
        {
            get { return current; }
        }

        public void SetInitialScore(IGameSession gameSession)
        {
            lastSession = gameSession;
            current = GameScore.Create(lastSession.Score);
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
                current.ScorePoints += 50 * game.PlayerSpaceship.TouchdownTrigger.Zone.ScoreMultiplier;
                current.ScorePoints += (int) (lastSession.Spaceship.RemainingFuel - game.PlayerSpaceship.RemainingFuel);
            }

            return current;
        }
    }
}