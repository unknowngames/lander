using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipSpeedObserver : UIBehaviour
    {                               
        [SerializeField]
        private Text speedText;

        public void Update()
        {
            if (PlayerSpawner.PlayerSpaceship != null)
            {
                string format = System.String.Format("{0:F1}", PlayerSpawner.PlayerSpaceship.Velosity.magnitude);
                if (speedText != null)
                {
                    speedText.text = format;
                }
            }
        }
    }
}