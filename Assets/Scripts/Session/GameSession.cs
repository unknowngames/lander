using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class GameSession : IGameSession
    {
        public ISpaceshipState Spaceship { get; private set; }
        public IGameScore Score { get; private set; }

        private GameSession(ISpaceshipState spaceship, IGameScore gameScore)
        {
            Score = gameScore;
            Spaceship = spaceship;
        }

        public static GameSession Create(ISpaceshipState spaceship, IGameScore gameScore)
        {
            return new GameSession(spaceship, gameScore);
        }

        public static GameSession Create(IGameSession gameSession)
        {
            return new GameSession(gameSession.Spaceship, gameSession.Score);
        }
    }
}