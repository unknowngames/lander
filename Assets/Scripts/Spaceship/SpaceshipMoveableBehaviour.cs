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
            Debug.Log("Impulse: " + impulse);
            Debug.Log("Angular speed: " + cachedRigidbody.angularVelocity);
            if (impulse < -0.0f)
            {
                spaceship.LeftStabilizerThrottleLevel = -impulse;
                spaceship.RightStabilizerThrottleLevel = 0.0f;
            }
            else if (impulse > 0.0f)
            {
                spaceship.LeftStabilizerThrottleLevel = 0.0f;
                spaceship.RightStabilizerThrottleLevel = impulse;
            }
            else
            {
                spaceship.LeftStabilizerThrottleLevel = 0.0f;
                spaceship.RightStabilizerThrottleLevel = 0.0f; 
            }
        }

        private void AddTorque(Vector3 impulse)
        {
            if (Spaceship.IsCrashed)
            {
                return;
            }

            Vector3 totalImpulse = impulse*cachedRigidbody.mass*rotationImpulseMultiplyer;

            cachedRigidbody.AddTorque(totalImpulse, ForceMode.Impulse);
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
                float impulse = -Mathf.Clamp(cachedRigidbody.angularVelocity.z, -1.0f, 1.0f);
                
                SetImpulse(impulse);

                if (cachedRigidbody.angularVelocity.sqrMagnitude<0.05f)
                {
                    SetImpulse(0.0f);
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
            
            AddTorque(transform.forward * -spaceship.LeftStabilizerEnginePower);
            AddTorque(transform.forward * spaceship.RightStabilizerEnginePower);
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