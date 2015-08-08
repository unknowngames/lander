using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
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
