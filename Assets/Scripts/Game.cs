using System;
using Assets.Scripts.Spaceship;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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
                        return null;
                    } 
                    if (objects.Length == 0)
                    {
                        Debug.LogError("На сцене не найдено ни одного объекта Game!");
                        return null;
                    }
                    cachedGame = objects[0];
                }
                return cachedGame;
            }
        }

        [SerializeField]
        private PlayerSpawner playerSpawner = new PlayerSpawner();
        public SpaceshipBehaviour PlayerSpaceship { get; private set; }

        private IGameSessionStorage gameSessionStorage;

        private UnityEvent onBegin; 
        private UnityEvent onPause;
        private UnityEvent onUnpause;
        private UnityEvent onFinish;
        private UnityEvent onMissionCompleted;
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
        public UnityEvent OnMissionCompleted
        {
            get
            {
                return onMissionCompleted ?? (onMissionCompleted = new UnityEvent());
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

        private void OnMissionCompletedCall()
        {
            if (OnMissionCompleted != null)
            {
                OnMissionCompleted.Invoke();
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
            gameSessionStorage = new GameSessionStorage();
            Begin ();
        }

        public void Begin()
        {
            PlayerSpaceship = playerSpawner.CreatePlayerAndRandomMove();

            PlayerSpaceship.CrashEvent.AddListener(OnSpaceshipCrashHandler);
            PlayerSpaceship.LandEvent.AddListener(OnSpaceshipLandHandler);


            if (gameSessionStorage.HasSavedSession)
            {
                gameSessionStorage.RestoreSavedSession(this);
            }

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

        public IGameSession Save()
        {
            ISpaceshipState spaceshipState = PlayerSpaceship.Save();
            IGameScore gameScore = GameScore.Create(Random.Range(0, 100), 10);

            return GameSession.Create(spaceshipState, gameScore);
        }

        public void Restore(IGameSession session)
        {
            PlayerSpaceship.Restore(session.Spaceship);
        }

        private void CompleteMission()
        {
            SetPlayerPause(true);
            gameSessionStorage.SaveGameSession(this);
            OnMissionCompletedCall();
        }

        private void FailMission()
        {
            SetPlayerPause(true);
            gameSessionStorage.RemoveSaveGame();
            OnFinishCall();
        }

        private void SetPlayerPause (bool state)
        {
            PlayerSpaceship.IsPaused = state;
        }

        private void Clean ()
        {
            PlayerSpaceship.CrashEvent.RemoveListener(OnSpaceshipCrashHandler);
            PlayerSpaceship.LandEvent.RemoveListener(OnSpaceshipLandHandler);

            PlayerSpaceship = null;
            playerSpawner.RemovePlayer ();
        }


        private void OnSpaceshipLandHandler()
        {
            CompleteMission();
        }

        private void OnSpaceshipCrashHandler()
        {
            FailMission();
        }
    }
}