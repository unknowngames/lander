using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipFuelObserverImage : UIBehaviour
    {
        [SerializeField]
        private Image indicator;

        public void Update()
        {
            if (PlayerSpawner.PlayerSpaceship != null)
            {
                float remaingFuelPercent = PlayerSpawner.PlayerSpaceship.RemainingFuel/PlayerSpawner.PlayerSpaceship.MaxFuel;
                if (indicator != null)
                {
                    indicator.fillAmount = remaingFuelPercent;
                }
            }
        }
    }
}