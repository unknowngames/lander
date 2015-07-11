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

        private Rigidbody cachedRigidbody;
        private bool doRotationStabilize;


        private bool isPaused; 
        private Vector3 savedVelocity;
        private Vector3 savedAngularVelocity;

        public float ThrottleLevel
        {
            get
            {
                return Spaceship.ThrottleLevel;
            }
            set
            {
                Spaceship.ThrottleLevel = value;
            }
        }

        public ISpaceship Spaceship
        {
            get
            {
                return spaceship ?? (spaceship = GetComponent<ISpaceship> ());
            }
        }

        public void Awake ()
        {
            cachedRigidbody = GetComponent<Rigidbody> ();
        }

        public void OnEnable ()
        {                                                     
            cachedRigidbody.maxAngularVelocity = maxAngularVelocity;
            RotationStabilize ();
        }

        public void RotationStabilize ()
        {
            doRotationStabilize = true;
        }

        public void SetImpulse(float impulse)
        {
            if (Spaceship.IsCrashed)
            {
                return;
            }

            cachedRigidbody.AddTorque(transform.forward * impulse * cachedRigidbody.mass * rotationImpulseMultiplyer, ForceMode.Impulse);
        }

        public void FixedUpdate ()
        {
            if (Spaceship.IsCrashed)
            {
                return;
            }

            if (Spaceship.IsPaused != isPaused)
            {
                isPaused = Spaceship.IsPaused;

                if (isPaused)
                {
                    StoreRigidbody ();
                }
                else
                {
                    RestoreRigidbody ();
                }

                return;
            }

            if (doRotationStabilize)
            {
                cachedRigidbody.AddTorque (-cachedRigidbody.angularVelocity * rotationImpulseMultiplyer * 5.0f * cachedRigidbody.mass);
                if (cachedRigidbody.angularVelocity.sqrMagnitude<0.1f)
                {
                    cachedRigidbody.angularVelocity = Vector3.zero;
                    doRotationStabilize = false;
                }
            }     

            if (Spaceship.RemainingFuel > 0.0f)
            {
                float throttle = Mathf.Pow (ThrottleLevel, 0.25f);
                cachedRigidbody.AddForce (transform.up * throttle * -Physics.gravity.y * cachedRigidbody.mass * tw);

                Spaceship.RemainingFuel -= Time.fixedDeltaTime * throttle * 2;
            }
        }

        private void StoreRigidbody()
        {
            savedVelocity = cachedRigidbody.velocity;
            savedAngularVelocity = cachedRigidbody.angularVelocity;
            cachedRigidbody.isKinematic = true;
        }

        private void RestoreRigidbody()
        {
            cachedRigidbody.isKinematic = false;
            cachedRigidbody.AddForce(savedVelocity, ForceMode.VelocityChange);
            cachedRigidbody.AddTorque(savedAngularVelocity, ForceMode.VelocityChange);
        }
    }
}