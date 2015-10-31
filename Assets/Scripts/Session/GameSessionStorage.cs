using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Session
{
    public class GameSessionStorage : ScriptableObject, IGameSessionStorage
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

        public void SaveGameSession(IFlight flight)
        {
            savedSession = flight.Save();
            GameSessionPlayerPrefsProxy.Save(savedSession);
        }

        public void RestoreSavedSession(IFlight flight)
        {
            if (savedSession == null)
            {
                savedSession = HasSavedSession ? GameSessionPlayerPrefsProxy.Restore() : CreateNew(flight);
            }

            flight.Restore(savedSession);
        }

		public long GetTopScore(string difficultyName)
		{
			return GameSessionPlayerPrefsProxy.GetTopScore (difficultyName);
		}

		public void SetTopScore(string difficultyName, long score)
		{
			GameSessionPlayerPrefsProxy.SaveTopScore (difficultyName, score);
		}

        private IGameSession CreateNew(IFlight flight)
        {
            IGameScore score = GameScore.Create(0, 0);
            ISpaceshipState state = PlayerSpawner.PlayerSpaceship.Save();

            return GameSession.Create(state, score);
        }

        public void RemoveSavedGame()
        {
            GameSessionPlayerPrefsProxy.Remove();
            savedSession = null;
        }      
    }
}