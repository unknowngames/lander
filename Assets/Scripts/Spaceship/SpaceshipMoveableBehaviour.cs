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
        private float force;

        [SerializeField]
        private float rotateSpeed;

        public ISpaceship Spaceship
        {
            get
            {
                return spaceship ?? (spaceship = GetComponent<ISpaceship> ());
            }
        }

        public void SetImpulse(float impulse)
        {

        }

        public float ThrottleLevel { get; set; }

        public void FixedUpdate ()
        {
            if (RotateClockwiseButton)
            {
                GetComponent<Rigidbody>().AddTorque(transform.forward * rotateSpeed);
            }
            if (RotateCounterClockwiseButton)
            {
                GetComponent<Rigidbody>().AddTorque(-transform.forward * rotateSpeed);
            }

            GetComponent<Rigidbody>().AddForce(transform.up * ThrottleLevel * force);
        }
    }
}