using System.Collections;
using Assets.Scripts.UI.HUD;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuActions : MonoBehaviour
    {
        [SerializeField]
        private SpaceshipPlanetRotation planetRotation;

        [SerializeField]
        private bool reallyLoadLevel = true;

        public void Start()
        {
            planetRotation.DoRotationSlower();
        }

        public void OnStart()
        {
            planetRotation.DoRotationFaster();
            if (reallyLoadLevel)
            {
                Application.LoadLevelAsync (1);
            }
        }
    }
}