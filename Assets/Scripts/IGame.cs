﻿using Assets.Scripts.Spaceship;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public interface IGame
    {                     
        UnityEvent OnPause { get; }   
        UnityEvent OnUnpause { get; }  
        UnityEvent OnFinish { get; }

        SpaceshipBehaviour PlayerSpaceship { get; }
        void Begin();
        void Abort();
        void Pause();
        void Unpause();
        void Finish();
    }
}