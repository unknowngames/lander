using Assets.Scripts.Spaceship;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GameMOC : IGame
    {
        private UnityEvent onPause;
        private UnityEvent onUnpause;
        private UnityEvent onFinish;
                                                                            
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