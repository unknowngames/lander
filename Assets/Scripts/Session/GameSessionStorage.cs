using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class GameSessionStorage : IGameSessionStorage
    {
        private IGameSession savedSession;

        public IGameSession Current
        {
            get 
            {
                return savedSession;
            }
        }

        public bool HasSavedSession
        {
            get { return GameSessionPlayerPrefsProxy.HasSession; }
        }

        public void SaveGameSession(IGame game)
        {
            savedSession = game.Save();
            GameSessionPlayerPrefsProxy.Save(savedSession);
        }

        public void RestoreSavedSession(IGame game)
        {
            if (savedSession == null)
            {
                savedSession = HasSavedSession ? GameSessionPlayerPrefsProxy.Restore() : CreateNew(game);
            }

            game.Restore(savedSession);
        }

        private IGameSession CreateNew(IGame game)
        {
            IGameScore score = GameScore.Create(0, 0, 0, 0, 0, 0);
            ISpaceshipState state = game.PlayerSpaceship.Save();

            return GameSession.Create(state, score);
        }

        public void RemoveSaveGame()
        {
            GameSessionPlayerPrefsProxy.Remove();
            savedSession = null;
        }      
    }
}