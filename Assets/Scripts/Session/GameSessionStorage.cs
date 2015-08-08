using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class GameSessionStorage : IGameSessionStorage
    {
        private IGameSession savedSession;

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
                savedSession = GameSessionPlayerPrefsProxy.Restore();
            }

            game.Restore(savedSession);
        }

        public void RemoveSaveGame()
        {
            GameSessionPlayerPrefsProxy.Remove();
            savedSession = null;
        }
    }
}