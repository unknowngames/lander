using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipFuelObserver : UIBehaviour
    {
        [SerializeField]
        private Text fuelText;

        public void Update()
        {
            if (GameHelper.PlayerSpaceship != null)
            {
                string format = System.String.Format("{0:F0}", GameHelper.PlayerSpaceship.RemainingFuel);
                if (fuelText != null)
                {
                    fuelText.text = format;
                }
            }
        }
    }
}