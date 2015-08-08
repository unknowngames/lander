using Assets.Scripts.Spaceship;
using UnityEngine.Events;

namespace Assets.Scripts.Interfaces
{
    public interface IGame
    {
        UnityEvent OnBegin { get; }
        UnityEvent OnPause { get; }
        UnityEvent OnUnpause { get; }
        UnityEvent OnFinish { get; }
        UnityEvent OnMissionCompleted { get; }
        UnityEvent OnAbort { get; }
        
        SpaceshipBehaviour PlayerSpaceship { get; }

        void Begin();
        void Suspend();
        void Abort();
        void Pause();
        void Unpause();

        IGameSession Save();
        void Restore(IGameSession session);
    }
}