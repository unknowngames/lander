using Assets.Scripts.Spaceship;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GameMOC : IGame
    {
        private UnityEvent onBegin;
        private UnityEvent onPause;
        private UnityEvent onUnpause;
        private UnityEvent onFinish;
        private UnityEvent onAbort;

        public UnityEvent OnBegin
        {
            get
            {
                return onBegin ?? (onBegin = new UnityEvent());
            }
        }

        public UnityEvent OnPause
        {
            get
            {
                return onPause ?? (onPause = new UnityEvent());
            }
        }

        public UnityEvent OnUnpause
        {
            get
            {
                return onUnpause ?? (onUnpause = new UnityEvent());
            }
        }

        public UnityEvent OnFinish
        {
            get
            {
                return onFinish ?? (onFinish = new UnityEvent());
            }
        }

        public UnityEvent OnAbort
        {
            get
            {
                return onAbort ?? (onAbort = new UnityEvent());
            }
        }

        public SpaceshipBehaviour PlayerSpaceship
        {
            get
            {
                return null;
            }
        }

        public void Begin ()
        {
            Debug.Log ("GameMOC.Begin()");
        }

        public void Abort ()
        {
            Debug.Log("GameMOC.Abort()");
        }

        public void Pause ()
        {
            Debug.Log("GameMOC.Pause()");
        }

        public void Unpause ()
        {
            Debug.Log("GameMOC.Unpause()");
        }

        public void Finish ()
        {
            Debug.Log("GameMOC.Finish()");
        }
    }
}