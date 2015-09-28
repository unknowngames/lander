using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class FuelObserver : UIBehaviour
    {
        [SerializeField]
        private Text fuelLabel;

        [SerializeField]
        private Text fuelScore;

        public void Update()
        {
            string format = string.Format("{0:F1}", GameHelper.PlayerSpaceship.RemainingFuel);
            if (fuelScore != null)
            {
                fuelScore.text = format;
            }
        }
    }
}
