using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SpaceshipSpeedObserver : UIBehaviour
    {                               
        [SerializeField]
        private Text speedText;

        public void Update()
        {
            if (GameHelper.PlayerSpaceship != null)
            {
                string format = System.String.Format("{0:F2}", GameHelper.PlayerSpaceship.Velosity.magnitude);
                if (speedText != null)
                {
                    speedText.text = format;
                }
            }
        }
    }
}