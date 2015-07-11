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

        public void Update ()
        {
            if (GameHelper.PlayerSpaceship != null)
            {
                string format = System.String.Format("Fuel remain: {0:F0}", GameHelper.PlayerSpaceship.RemainingFuel);
                throttleText.text = format;

                format = System.String.Format("Height : {0:F0}", GameHelper.PlayerSpaceship.FlyHeight);
				heightText.text = format;
            }
        }    
    }
}
