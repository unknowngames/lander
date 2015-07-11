using System;
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
                return onFinish ?? (onFinish = new UnityEvent ());
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

        public void Start ()
        {
            Begin ();
        }

        public void Begin()
        {
            PlayerSpaceship = playerSpawner.CreatePlayer();
            PlayerSpaceship.CrashEvent.AddListener(Finish);
            PlayerSpaceship.LandEvent.AddListener(Finish);
        }

        public void Abort()
        {
            Clean();
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