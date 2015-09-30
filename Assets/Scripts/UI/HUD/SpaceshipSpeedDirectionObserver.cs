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
            if (PlayerSpawner.PlayerSpaceship != null)
            {
                Vector3 direction = PlayerSpawner.PlayerSpaceship.Velosity;
                Quaternion horisontRotation = Quaternion.FromToRotation(Vector3.down, direction);
                arrow.rotation = Quaternion.Slerp(arrow.rotation, horisontRotation, Time.deltaTime * 2);
            }
        }
    }
}