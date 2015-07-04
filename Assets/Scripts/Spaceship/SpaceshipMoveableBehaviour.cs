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
        [Tooltip("Thrust to weight ratio")]
        private float tw;

        [SerializeField]
        [Tooltip("Rotation impulse multiplyer")]
        private float rotationImpulseMultiplyer;

        [SerializeField]
        [Tooltip("Max angular velocity")]
        private float maxAngularVelocity=7;


        private Rigidbody rigidbody;

        public ISpaceship Spaceship
        {
            get
            {
                return spaceship ?? (spaceship = GetComponent<ISpaceship> ());
            }
        }

        public void Awake ()
        {
            rigidbody = GetComponent<Rigidbody> ();
        }

        public void OnEnable ()
        {                                                     
            rigidbody.maxAngularVelocity = maxAngularVelocity;
            RotationStabilize ();
        }

        public void RotationStabilize ()
        {
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void SetImpulse(float impulse)
        {
            rigidbody.AddTorque(transform.forward * impulse * rigidbody.mass * rotationImpulseMultiplyer, ForceMode.Impulse);
        }

        public float ThrottleLevel { get; set; }

        public void FixedUpdate ()
        {                                                         
            float throttle = Mathf.Pow (ThrottleLevel, 0.25f);
            rigidbody.AddForce(transform.up * throttle * -Physics.gravity.y * rigidbody.mass * tw);
        }
    }
}