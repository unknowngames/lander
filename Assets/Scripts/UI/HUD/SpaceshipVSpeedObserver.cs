﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipVSpeedObserver : UIBehaviour
    {                          
        [SerializeField]
        private Text vSpeedText;

        public void Update()
        {
            if (PlayerSpawner.PlayerSpaceship != null)
            {
                string format = System.String.Format("{0:F2}", Mathf.Abs(PlayerSpawner.PlayerSpaceship.Velosity.y));
                if (vSpeedText != null)
                {
                    vSpeedText.text = format;
                }
            }
        }
    }
}