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
        private bool doRotationStabilize;


        public float ThrottleLevel { get; set; }

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
            doRotationStabilize = true;
        }

        public void SetImpulse(float impulse)
        {
            rigidbody.AddTorque(transform.forward * impulse * rigidbody.mass * rotationImpulseMultiplyer, ForceMode.Impulse);
        }

        public void Update ()
        {
            Spaceship.ThrottleLevel = ThrottleLevel;
        }

        public void FixedUpdate ()
        {
            if (doRotationStabilize)
            {
                rigidbody.AddTorque (-rigidbody.angularVelocity * rotationImpulseMultiplyer * 5.0f * rigidbody.mass);
                if (rigidbody.angularVelocity.sqrMagnitude<0.1f)
                {
                    rigidbody.angularVelocity = Vector3.zero;
                    doRotationStabilize = false;
                }
            }     

            if (Spaceship.RemainingFuel > 0.0f)
            {
                float throttle = Mathf.Pow (ThrottleLevel, 0.25f);
                rigidbody.AddForce (transform.up * throttle * -Physics.gravity.y * rigidbody.mass * tw);

                Spaceship.RemainingFuel -= Time.fixedDeltaTime * throttle * 2;
            }
        }
    }
}