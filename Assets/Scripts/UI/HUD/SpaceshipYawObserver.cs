using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipYawObserver : UIBehaviour
    {
        [SerializeField]
        private RectTransform horizont;

        public void Update ()
        {
            if (GameHelper.PlayerSpaceship != null)
            {
                Quaternion rotation = GameHelper.PlayerSpaceship.Rotation;
                float yaw = rotation.eulerAngles.z;

                Quaternion horisontRotation = Quaternion.Euler(0, 0, yaw);
                horizont.rotation = horisontRotation;
            }
        }    
    }
}
