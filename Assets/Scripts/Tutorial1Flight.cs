using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tutorial1Flight : Flight
    {
        public void Start()
        {
            Begin();
        }

        public override void Begin()
        {
            PlayerSpawner.Current.CreatePlayerAndRandomMove();
            difficultyStorage.ApplyDifficulty(this);
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

        public override void CompleteFlight()
        {
            IsPaused = true;
            OnFlightCompletedCall();
        }

        public override void FailFlight()
        {
            IsPaused = true;
            OnFlightFailedCall();
        }

        public override IGameSession Save()
        {
            Debug.LogWarning("Did not use in tutorial");
            throw new System.NotImplementedException();
        }

        public override void Restore(IGameSession session)
        {
            Debug.LogWarning("Did not use in tutorial");
            throw new System.NotImplementedException();
        }
    }
}