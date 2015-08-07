using UnityEngine;

namespace Assets.Scripts
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
    }
}