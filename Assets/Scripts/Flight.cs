using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Session;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerSpawner))]
    public abstract class Flight : MonoBehaviour, IFlight
    {
        public static Flight Current { get; private set; }

        [SerializeField]
        protected CameraController cameraController;

        [SerializeField]
        protected GameDifficultyStorage difficultyStorage;
        [SerializeField]
        protected GameSessionStorage gameSessionStorage;

        public ILevelInfo LevelInfo { get; protected set; }

        public bool IsPaused { get; protected set; }
        
		[SerializeField]
        private UnityEvent onBegin;
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

        public UnityEvent OnFlightFailed
        {
            get
            {
                return onMissionFailed ?? (onMissionFailed = new UnityEvent());
            }
        }

        public UnityEvent OnFlightCompleted
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

        protected void OnBeginCall()
        {
            if (OnBegin != null)
            {
                OnBegin.Invoke();
            }
        }

        protected void OnPauseCall()
        {
            if (OnPause != null)
            {
                OnPause.Invoke ();
            }
        }

        protected void OnUnpauseCall()
        {
            if (OnUnpause != null)
            {
                OnUnpause.Invoke();
            }
        }

        protected void OnFlightFailedCall()
        {
            if (OnFlightFailed != null)
            {
                OnFlightFailed.Invoke();
            }
        }

        protected void OnFlightCompletedCall()
        {
            if (OnFlightCompleted != null)
            {
                OnFlightCompleted.Invoke();
            }
        }

        protected void OnAbortCall()
        {
            if (OnAbort != null)
            {
                OnAbort.Invoke();
            }
        }

        public void Awake()
        {
            LevelInfo = FindObjectOfType<LevelInfo>();
        }

        public virtual void OnBeginTune()
        {
        }

        public void Begin()
        {                   
            PlayerSpawner.Current.CreatePlayerAndRandomMove();
            difficultyStorage.ApplyDifficulty(this);
            IsPaused = false;
            cameraController.SetTarget(PlayerSpawner.PlayerSpaceship.GetComponent<CameraTarget>(), true);
            OnBeginCall();
        }

        public abstract void Abort();

        public abstract void Pause();

        public abstract void Unpause();

        public abstract IGameSession Save();

        public abstract void Restore(IGameSession session);

        public abstract void CompleteFlight();

        public abstract void FailFlight();

        protected void OnEnable()
        {
            if (Current == null)
            {
                Current = this;
            }
            else
            {
                Debug.LogWarning("Multiple Flight in scene... this is not supported");
            }
        }

        protected void OnDisable()
        {
            if (Current == this)
            {
                Current = null;
            }
        }

        protected void Clean ()
        {
            PlayerSpawner.Current.RemovePlayer ();
        }
    }
}