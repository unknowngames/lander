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
            if (PlayerSpawner.PlayerSpaceship != null)
            {
                string format = System.String.Format("{0:F0}", PlayerSpawner.PlayerSpaceship.RemainingFuel);
                if (fuelText != null)
                {
                    fuelText.text = format;
                }
            }
        }
    }
}