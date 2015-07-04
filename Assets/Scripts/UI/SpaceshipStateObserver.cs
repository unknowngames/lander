using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SpaceshipStateObserver : UIBehaviour
    {
        [SerializeField]
        private Text throttleText;

        public void Update ()
        {
            if (Game.PlayerSpaceship != null)
            {
                string format = System.String.Format("Fuel remain: {0:F0}", Game.PlayerSpaceship.RemainingFuel);
                throttleText.text = format;
            }
        }    
    }
}
