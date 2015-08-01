using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SpaceshipStateObserver : UIBehaviour
    {
        [SerializeField]
        private Text throttleText;

        [SerializeField]
        private Text heightText;

        [SerializeField]
        private Text vSpeedText;

        [SerializeField]
        private Text speedText;

        public void Update ()
        {
            if (GameHelper.PlayerSpaceship != null)
            {
                string format = System.String.Format("Fuel remain: {0:F0}", GameHelper.PlayerSpaceship.RemainingFuel);
                if (throttleText != null)
                {
                    throttleText.text = format;
                }

                format = System.String.Format("{0:F0}", GameHelper.PlayerSpaceship.FlyHeight);
                if (heightText != null)
                {
                    heightText.text = format;
                }

                format = System.String.Format("{0:F0}", GameHelper.PlayerSpaceship.Velosity.magnitude);
                if (speedText != null)
                {
                    speedText.text = format;
                }

                format = System.String.Format("{0:F0}", Mathf.Abs(GameHelper.PlayerSpaceship.Velosity.y));
                if (vSpeedText != null)
                {
                    vSpeedText.text = format;
                }
            }
        }    
    }
}
