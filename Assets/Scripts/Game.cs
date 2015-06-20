using System;
using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Spaceship;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Game : Singleton<Game>
    {
        [SerializeField]
        private PlayerSpawner playerSpawner = new PlayerSpawner ();

        public static SpaceshipBehaviour PlayerSpaceship { get; private set; }

        public delegate void GameEventDelegate (object sender, EventArgs e);

        public static GameEventDelegate OnBegin;
        public static GameEventDelegate OnAbort;
        public static GameEventDelegate OnPause;
        public static GameEventDelegate OnUnpause;
        public static GameEventDelegate OnFinish;

        private void OnBeginCall()
        {
            if (OnBegin != null)
            {
                OnBegin(this, EventArgs.Empty);
            }
        }

        private void OnAbortCall()
        {
            if (OnAbort != null)
            {
                OnAbort(this, EventArgs.Empty);
            }
        }

        private void OnPauseCall()
        {
            if (OnPause != null)
            {
                OnPause(this, EventArgs.Empty);
            }
        }

        private void OnUnpauseCall()
        {
            if (OnUnpause != null)
            {
                OnUnpause(this, EventArgs.Empty);
            }
        }

        private void OnFinishCall()
        {
            if (OnFinish != null)
            {
                OnFinish(this, EventArgs.Empty);
            }
        }

        public static void Begin()
        {
            PlayerSpaceship = Instance.playerSpawner.CreatePlayer();
            Instance.OnBeginCall ();
        }

        public static void Abort()
        {
            Instance.Clean ();
            Instance.OnAbortCall();
        }

        public static void Pause()
        {
            SetPlayerPause(true);
            Instance.OnPauseCall ();
        }

        public static void Unpause()
        {
            SetPlayerPause (false);
            Instance.OnUnpauseCall();
        }

        public static void Finish()
        {
            Instance.OnFinishCall();
        }

        private static void SetPlayerPause (bool state)
        {
            PlayerSpaceship.IsPaused = state;
        }

        private void Clean ()
        {
            PlayerSpaceship = null;
            playerSpawner.RemovePlayer ();
        }
    }
}