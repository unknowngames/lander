using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipSpeedDirectionObserver : UIBehaviour
    {
        [SerializeField]
        private RectTransform arrow;

        public void Update()
        {
            if (GameHelper.PlayerSpaceship != null)
            {
                Vector3 direction = GameHelper.PlayerSpaceship.Velosity;
                Quaternion horisontRotation = Quaternion.FromToRotation(Vector3.down, direction);
                arrow.rotation = horisontRotation;
            }
        }
    }
}