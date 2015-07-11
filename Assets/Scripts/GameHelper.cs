﻿using Assets.Scripts.Spaceship;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public static class GameHelper
    {
        public static UnityEvent OnPause
        {
            get
            {
                return Game.Instance.OnPause;
            }
        }

        public static UnityEvent OnUnpause
        {
            get
            {
                return Game.Instance.OnUnpause;
            }
        }
        public static UnityEvent OnFinish
        {
            get
            {
                return Game.Instance.OnFinish;
            }
        }

        public static SpaceshipBehaviour PlayerSpaceship
        {
            get
            {
                return Game.Instance.PlayerSpaceship;
            }
        }

        public static void Begin ()
        {
            Game.Instance.Begin ();
        }

        public static void Abort ()
        {
            Game.Instance.Abort ();
        }

        public static void Pause ()
        {
            Game.Instance.Pause ();
        }

        public static void Unpause ()
        {
            Game.Instance.Unpause ();
        }

        public static void Finish ()
        {
            Game.Instance.Finish ();
        }
    }
}