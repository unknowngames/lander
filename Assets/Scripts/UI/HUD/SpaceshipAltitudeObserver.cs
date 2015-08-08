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
            if (GameHelper.PlayerSpaceship != null)
            {             
                string format = System.String.Format("{0:F0}", GameHelper.PlayerSpaceship.FlyHeight);
                if (altitudeText != null)
                {
                    altitudeText.text = format;
                }
            }
        }
    }
}