﻿using System;
using Assets.Scripts.Spaceship;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour, IGame
    {
        private static Game cachedGame;

        public static IGame Instance
        {
            get
            {
                if (cachedGame == null)
                {
                    Game[] objects = FindObjectsOfType<Game>();
                    if (objects.Length > 1)
                    {
                        Debug.LogError("На сцене находится более одного объекта Game!");
                        return new GameMOC();
                    } 
                    if (objects.Length == 0)
                    {
                        Debug.LogError("На сцене не найдено ни одного объекта Game!");
                        return new GameMOC ();
                    }
                    cachedGame = objects[0];
                }
                return cachedGame;
            }
        }

        [SerializeField]
        private PlayerSpawner playerSpawner = new PlayerSpawner();
        public SpaceshipBehaviour PlayerSpaceship { get; private set; }

		[SerializeField]
		private float playerStartImpulsePower = 500;

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
                return onFinish ?? (onFinish = new UnityEvent ());
            }
        }

        public UnityEvent OnAbort
        {
            get
            {
                return onAbort ?? (onAbort = new UnityEvent());
            }
        }

        private void OnBeginCall()
        {
            if (OnBegin != null)
            {
                OnBegin.Invoke();
            }
        }

        private void OnPauseCall()
        {
            if (OnPause != null)
            {
                OnPause.Invoke ();
            }
        }

        private void OnUnpauseCall()
        {
            if (OnUnpause != null)
            {
                OnUnpause.Invoke();
            }
        }

        private void OnFinishCall()
        {
            if (OnFinish != null)
            {
                OnFinish.Invoke();
            }
        }

        private void OnAbortCall()
        {
            if (OnAbort != null)
            {
                OnAbort.Invoke();
            }
        }

        public void Start ()
        {
            Begin ();
        }

        public void Begin()
        {
            PlayerSpaceship = playerSpawner.CreatePlayer();

			Vector3 randomDirection = Vector3.right;
			randomDirection *= UnityEngine.Random.Range (0, 2) == 0 ? -1 : 1;
			randomDirection *= playerStartImpulsePower;
			PlayerSpaceship.SetVelocity(randomDirection);

            PlayerSpaceship.CrashEvent.AddListener(Finish);
            PlayerSpaceship.LandEvent.AddListener(Finish);
            OnBeginCall ();
        }

        public void Abort()
        {
            Clean();
            OnAbortCall();
            Application.LoadLevelAsync(0);
        }

        public void Pause()
        {
            SetPlayerPause(true);
            OnPauseCall();
        }

        public void Unpause()
        {
            SetPlayerPause(false);
            OnUnpauseCall();
        }

        public void Finish()
        {
            SetPlayerPause(true);
            OnFinishCall();
        }

        private void SetPlayerPause (bool state)
        {
            PlayerSpaceship.IsPaused = state;
        }

        private void Clean ()
        {
            PlayerSpaceship.CrashEvent.RemoveListener(Finish);
            PlayerSpaceship.LandEvent.RemoveListener(Finish);

            PlayerSpaceship = null;
            playerSpawner.RemovePlayer ();
        }
    }
}