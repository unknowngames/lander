using Assets.Scripts.Interfaces;
using Assets.Scripts.Session;
using Assets.Scripts.Settings;
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

        [SerializeField]
        private GameDifficultyStorage difficultyStorage;
        [SerializeField]
        private GameSessionStorage gameSessionStorage;
        
        public SpaceshipBehaviour PlayerSpaceship { get; private set; }

        public IGameScore CurrentScore
        {
            get { return scoreCalculator.Current; }
        }

        public ILevelInfo LevelInfo { get; private set; }

        public bool IsPaused { get; private set; }

        private IScoreCalculator scoreCalculator;

		[SerializeField]
        private UnityEvent onBegin;
		[SerializeField]
		private UnityEvent onSuspended;
		[SerializeField]
		private UnityEvent onPause;
		[SerializeField]
		private UnityEvent onUnpause;
		[SerializeField]
		private UnityEvent onMissionFailed;
		[SerializeField]
		private UnityEvent onMissionCompleted;
		[SerializeField]
        private UnityEvent onAbort;

        public UnityEvent OnBegin
        {
            get
            {
                return onBegin ?? (onBegin = new UnityEvent());
            }
        }

        public UnityEvent OnSuspended
        {
            get
            {
                return onSuspended ?? (onSuspended = new UnityEvent());
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

        public UnityEvent OnMissionFailed
        {
            get
            {
                return onMissionFailed ?? (onMissionFailed = new UnityEvent());
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

        private void OnSuspendedCall()
        {
            if (OnSuspended != null)
            {
                OnSuspended.Invoke();
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

        private void OnMissionFailedCall()
        {
            if (OnMissionFailed != null)
            {
                OnMissionFailed.Invoke();
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

        public void Awake()
        {       
            scoreCalculator = new ScoreCalculator(this);
            LevelInfo = FindObjectOfType<LevelInfo>();
        }

        public void Start ()
        {                                                        
            Begin ();
        }

        public void Begin()
        {
            PlayerSpaceship = playerSpawner.CreatePlayerAndRandomMove();   
            gameSessionStorage.RestoreSavedSession(this);
            difficultyStorage.ApplyDifficulty(this);
            scoreCalculator.Begin();
            IsPaused = false;
            OnBeginCall ();
        }

        public void Suspend()
        {
            Clean();
            OnSuspendedCall();    
            Application.LoadLevelAsync(0);
        }

        public void Abort()
        {
            OnAbortCall();
            Clean();
            Application.LoadLevelAsync(0);
        }

        public void Pause()
        {
            IsPaused = true;
            OnPauseCall();
        }

        public void Unpause()
        {
            IsPaused = false;
            OnUnpauseCall();
        }

        public IGameSession Save()
        {
            IGameSession gameScore = scoreCalculator.Calculate();
            return gameScore;
        }

        public void Restore(IGameSession session)
        {
            scoreCalculator.SetInitialScore(session);
            PlayerSpaceship.Restore(session.Spaceship);
        }

        public void CompleteMission()
        {
            gameSessionStorage.SaveGameSession(this);
            IsPaused = true;
            OnMissionCompletedCall();
        }

        public void FailMission()
        {
            gameSessionStorage.RemoveSavedGame();
            IsPaused = true;
            OnMissionFailedCall();
        }

        public void Update()
        {
            scoreCalculator.Update();
        }

        private void Clean ()
        {
            PlayerSpaceship = null;
            playerSpawner.RemovePlayer ();
        }
    }
}