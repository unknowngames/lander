using Assets.Scripts.Interfaces;
using Assets.Scripts.Session;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(ScoreCalculator))]
    public class GameFlight : Flight
    {
        public void Start()
        {
            Begin();
        }

        public override void Begin()
        {
            PlayerSpawner.Current.CreatePlayerAndRandomMove();
            gameSessionStorage.RestoreSavedSession(this, difficultyStorage.GetSavedDifficulty().Name);
            difficultyStorage.ApplyDifficulty(this);
            ScoreCalculator.Current.Begin();
            IsPaused = false;
            OnBeginCall();
        }


        public override void Abort()
        {
            Clean();
            OnAbortCall();
            Application.LoadLevelAsync(0);
        }

        public override void Pause()
        {
            IsPaused = true;
            OnPauseCall();
        }

        public override void Unpause()
        {
            IsPaused = false;
            OnUnpauseCall();
        }

        public override IGameSession Save()
        {
            IGameSession gameSession = ScoreCalculator.Current.Calculate();

			var currentScore = gameSession.Score.LastFlightScorePoints;
			var difficulty = difficultyStorage.GetSavedDifficulty ();
			if (gameSession.Score.IsTopScoreBeaten) 
			{
				gameSessionStorage.SetTopScore(difficulty.Name, gameSession.Score.ScorePoints);
			}
			UnityEngine.Social.ReportScore (currentScore, difficulty.LeaderboardID, processReportScore);

			return gameSession;
        }

		void processReportScore(bool success)
		{
			Debug.Log ("Report top score status: " + success);
		}

        public override void Restore(IGameSession session)
        {
            ScoreCalculator.Current.SetInitialScore(session);
            PlayerSpawner.PlayerSpaceship.Restore(session.Spaceship);
        }

        public override void CompleteFlight()
        {
            gameSessionStorage.SaveGameSession(this);
            IsPaused = true;
            OnFlightCompletedCall();
        }

        public override void FailFlight()
        {
            gameSessionStorage.RemoveSavedGame();
            IsPaused = true;
            OnFlightFailedCall();
        }
    }
}