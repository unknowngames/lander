using Assets.Scripts.Interfaces;
using Assets.Scripts.Spaceship;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public static class GameHelper
    {
        public static UnityEvent OnBegin
        {
            get { return Game.Instance.OnBegin; }
        }

        public static UnityEvent OnPause
        {
            get { return Game.Instance.OnPause; }
        }

        public static UnityEvent OnUnpause
        {
            get { return Game.Instance.OnUnpause; }
        }

        public static UnityEvent OnFinish
        {
            get { return Game.Instance.OnFinish; }
        }

        public static UnityEvent OnMissionCompleted
        {
            get { return Game.Instance.OnMissionCompleted; }
        }

        public static UnityEvent OnAbort
        {
            get { return Game.Instance.OnAbort; }
        }

        public static SpaceshipBehaviour PlayerSpaceship
        {
            get { return Game.Instance.PlayerSpaceship; }
        }

        public static IGameScore CurrentScore
        {
            get { return Game.Instance.CurrentScore; }
        }

        public static ILevelInfo LevelInfo
        {
            get { return Game.Instance.LevelInfo; }
        }

        public static void Begin()
        {
            Game.Instance.Begin();
        }

        public static void Suspend()
        {
            Game.Instance.Suspend();
        }

        public static void Abort()
        {
            Game.Instance.Abort();
        }

        public static void Pause()
        {
            Game.Instance.Pause();
        }

        public static void Unpause()
        {
            Game.Instance.Unpause();
        }

        public static void CompleteMission()
        {
           Game.Instance.CompleteMission(); 
        }

        public static void FailMission()
        {
            Game.Instance.FailMission(); 
        }

		public static float LandingTime
		{
			get
			{
				return Game.Instance.CurrentLandingTime;
			}
		}
    }
}