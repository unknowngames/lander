using Assets.Scripts.Spaceship;
using UnityEngine.Events;

namespace Assets.Scripts.Interfaces
{
    public interface IFlight
    {
        UnityEvent OnBegin { get; }      
        UnityEvent OnPause { get; }
        UnityEvent OnUnpause { get; }
        UnityEvent OnFlightFailed { get; }
        UnityEvent OnFlightCompleted { get; }
        UnityEvent OnAbort { get; }

        ILevelInfo LevelInfo { get; }

        bool IsPaused { get; }

        void Begin();
        void Abort();
        void Pause();
        void Unpause();
        void CompleteFlight();   
        void FailFlight();

        IGameSession Save();
        void Restore(IGameSession session);
    }
}