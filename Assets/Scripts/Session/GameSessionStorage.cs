using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class GameSessionStorage : IGameSessionStorage
    {
        private IGameSession savedSession;
        private readonly IGame game;

        public GameSessionStorage(IGame game)
        {
            this.game = game;
            game.OnBegin.AddListener(OnGameBegin);
            game.OnSuspended.AddListener(OnGameSuspend);
            game.OnMissionCompleted.AddListener(OnGameMissionCompleted);
            game.OnFinish.AddListener(OnGameFinished);
        }

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

        public void SaveGameSession()
        {
            savedSession = game.Save();
            GameSessionPlayerPrefsProxy.Save(savedSession);
        }

        public void RestoreSavedSession()
        {
            if (savedSession == null)
            {
                savedSession = HasSavedSession ? GameSessionPlayerPrefsProxy.Restore() : CreateNew();
            }

            game.Restore(savedSession);
        }

        private IGameSession CreateNew()
        {
            IGameScore score = GameScore.Create(0, 0);
            ISpaceshipState state = game.PlayerSpaceship.Save();

            return GameSession.Create(state, score);
        }

        public void RemoveSaveGame()
        {
            GameSessionPlayerPrefsProxy.Remove();
            savedSession = null;
        }

        private void OnGameBegin()
        {     
            RestoreSavedSession();
        }         

        private void OnGameSuspend()                    
        {
            SaveGameSession();
        }                    

        private void OnGameMissionCompleted()
        {
            SaveGameSession();
        }

        private void OnGameFinished()
        {
            RemoveSaveGame();
        }    
    }
}