using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipAltitudeObserver : UIBehaviour
    {

        [SerializeField]
        private Text altitudeText;

        public void Update()
        {
            if (PlayerSpawner.PlayerSpaceship != null)
            {
                string format = System.String.Format("{0:F1}", PlayerSpawner.PlayerSpaceship.FlyHeight);
                if (altitudeText != null)
                {
                    altitudeText.text = format;
                }
            }
        }
    }
}