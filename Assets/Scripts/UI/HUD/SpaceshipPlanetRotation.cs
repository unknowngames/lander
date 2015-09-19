using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.UI.HUD
{
    public class SpaceshipPlanetRotation : MonoBehaviour
    {
        [SerializeField]
        private float rotationInertia = 1.0f;
        [SerializeField]
        [FormerlySerializedAs("Speed")]
        private float normalSpeed = 1.0f;
        [SerializeField]
        private float fastSpeed = 1.0f;
        [SerializeField]
        [FormerlySerializedAs("Axis")]
        private Transform rotationAxis;

        private float speed;
        private bool isSpeedIncreased;

        public void Update ()
        {
            float maxSpeed = isSpeedIncreased ? fastSpeed : normalSpeed;
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * rotationInertia);

            transform.Rotate(rotationAxis.transform.forward, Time.deltaTime * speed);
        }

        public void DoRotationFaster()
        {
            isSpeedIncreased = true;
        }

        public void DoRotationSlower()
        {
            isSpeedIncreased = false;
        }
    }
}
