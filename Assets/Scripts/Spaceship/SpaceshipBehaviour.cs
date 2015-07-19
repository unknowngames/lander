using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipBehaviour : MonoBehaviour, ISpaceship
    {
        [SerializeField]
        private float initialFuel;
        [SerializeField]
        private float safeSpeed;
                                            
        [SerializeField]
        private SpaceshipGhost spaceshipGhost;     
        [SerializeField]
        private SpaceshipModel spaceshipModel;
        [SerializeField]
        private TouchdownTrigger touchdownTrigger;

        public string Name { get; set; }
        public float Mass { get; set; }
        public float RemainingFuel { get; set; }
        public bool IsPaused { get; set; }

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private  set; }
        public Vector3 Velosity { get; private  set; }
        public Vector3 AngularVelosity { get; private set; }
        public float FlyHeight { get; private set; }
        public bool IsCrashed { get; private set; }
        public float EnginePower { get; private set; }
        public float RightStabilizerEnginePower { get; private set; }
        public float LeftStabilizerEnginePower { get; private set; }


        private float throttleLevel;
        private float leftStabilizerThrottleLevel;
        private float rightStabilizerThrottleLevel;

        public float ThrottleLevel
        {
            get { return throttleLevel; }
            set
            {
                throttleLevel = value;
                EnginePower = IsPaused || IsCrashed ? 0.0f : (RemainingFuel > 0.0f ? ThrottleLevel : 0.0f);
            }
        }

        public float LeftStabilizerThrottleLevel
        {
            get { return leftStabilizerThrottleLevel; }
            set
            {
                leftStabilizerThrottleLevel = value;
                LeftStabilizerEnginePower = IsPaused || IsCrashed ? 0.0f : leftStabilizerThrottleLevel;
            }
        }

        public float RightStabilizerThrottleLevel
        {
            get { return rightStabilizerThrottleLevel; }
            set
            {
                rightStabilizerThrottleLevel = value;
                RightStabilizerEnginePower = IsPaused || IsCrashed ? 0.0f : rightStabilizerThrottleLevel;
            }
        }

        public OnBumpEvent BumpEvent = new OnBumpEvent();

        public OnCrashEvent CrashEvent = new OnCrashEvent();
        public OnLandEvent LandEvent = new OnLandEvent();

        private Rigidbody cachedRigidbody;

        public void Update ()
        {
            ProcessState ();
        }

        private void ProcessState ()
        {
            ProcessHeight ();
            ProcessRigidbodyState();
            ProcessTouchdown();
        }

        private void ProcessTouchdown()
        {
            if (touchdownTrigger.Landed)
            {
                Landed();
                GameHelper.Finish();
            }
        }

        private void ProcessRigidbodyState ()
        {
            if (cachedRigidbody == null)
            {
                cachedRigidbody = GetComponent<Rigidbody> ();
            }

            Velosity = cachedRigidbody.velocity;
            Position = cachedRigidbody.position;
            Rotation = cachedRigidbody.rotation;     
            AngularVelosity = cachedRigidbody.angularVelocity;
        }

        private void ProcessHeight ()
        {
            RaycastHit hit;
            FlyHeight = Physics.Raycast(transform.position, Vector3.down, out hit) ? hit.distance : float.MaxValue;
        }

        public void Reset ()
        {
            RemainingFuel = initialFuel;

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            if (cachedRigidbody == null)
            {
                cachedRigidbody = GetComponent<Rigidbody> ();
            }

            cachedRigidbody.velocity = Vector3.zero;
            cachedRigidbody.angularVelocity = Vector3.zero;
            cachedRigidbody.isKinematic = false;


            IsCrashed = false;
            IsPaused = false;
            spaceshipModel.Reset ();
            spaceshipGhost.Reset ();
        }

        public void OnCollisionEnter (Collision collision)
        {
            ProcessCollisionEvent (collision);
        }

        private void ProcessCollisionEvent (Collision collision)
        {   
            if (!LandingPlaceTest (collision))
            {
				Crash (collision);
                return;
            }

            if (!VelosityTest(collision))
            {
				Crash(collision);
                return;
            }

			Bump (collision);
        }

		private void Crash(Collision collision)
        {
            IsCrashed = true;
			BlowUp (collision, RemainingFuel);

            CrashEvent.Invoke ();
            BumpEvent.Invoke(new BumpInfo
            {
                IsCrashed = true,
                IsLanded = false,
				mcollision = collision
            });
        }

		private void Bump(Collision collision)
        {
            BumpEvent.Invoke(new BumpInfo
            {
                IsCrashed = false,
                IsLanded = false,
				mcollision = collision
            });
        }

		private void Landed()
        {
            BumpEvent.Invoke(new BumpInfo
            {
                IsCrashed = false,
                IsLanded = true,
            });
        }

        private bool VelosityTest (Collision collision)
        {
            return collision.relativeVelocity.magnitude < safeSpeed;
        }       

        private bool LandingPlaceTest(Collision collision)
        {
            return collision.gameObject.layer == LayerMask.NameToLayer("Landing place");
        }

		private void BlowUp (Collision collision, float remainingFuel)
        {                                      
            if (cachedRigidbody == null)
            {
                cachedRigidbody = GetComponent<Rigidbody>();
            }

            cachedRigidbody.isKinematic = true;
            spaceshipModel.Hide();
			spaceshipGhost.BlowUp (collision, remainingFuel);
        }
    }
}