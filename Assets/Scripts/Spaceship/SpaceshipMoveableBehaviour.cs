using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    [RequireComponent (typeof (SpaceshipBehaviour))]
    public class SpaceshipMoveableBehaviour : MonoBehaviour, ISpaceshipMoveable
    {
        [SerializeField]
        private ISpaceship spaceship;

        [SerializeField]
        private float t;

        [SerializeField]
        private float rotationStepAngle;

        private float rotationImpulse = 0.0f;

        public ISpaceship Spaceship
        {
            get
            {
                return spaceship ?? (spaceship = GetComponent<ISpaceship> ());
            }
        }

        public void OnEnable ()
        {
            rotationImpulse = 0.0f;
        }

        public void SetImpulse(float impulse)
        {
            rotationImpulse += impulse;
        }

        public float ThrottleLevel { get; set; }

        public void FixedUpdate ()
        {
            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, rotationImpulse * rotationStepAngle);
            rotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, rotation, Time.fixedDeltaTime);
            GetComponent<Rigidbody>().MoveRotation (rotation);

            float throttle = Mathf.Pow (ThrottleLevel, 0.25f);
            GetComponent<Rigidbody>().AddForce(transform.up * throttle * -Physics.gravity.y * GetComponent<Rigidbody>().mass * t);
        }
    }
}