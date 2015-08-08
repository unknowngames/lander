using System.Diagnostics;
using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Session;
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
        [SerializeField]
        private TriggerArray crashTrigger;

        public string Name { get; set; }
        public float Mass { get; set; }
        public float RemainingFuel { get; set; }
        public bool IsPaused { get; set; }

        public float ThrottleLevel { get; set; }
        public float LeftStabilizerThrottleLevel { get; set; }
        public float RightStabilizerThrottleLevel { get; set; }

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector3 Velosity { get; private set; }
        public Vector3 AngularVelosity { get; private set; }
        public float FlyHeight { get; private set; }
        public bool IsCrashed { get; private set; }
        public bool IsLanded { get; private set; }
        public TouchdownTrigger TouchdownTrigger { get { return touchdownTrigger; } }

        public float EnginePower
        {
            get { return IsPaused || IsCrashed ? 0.0f : (RemainingFuel > 0.0f ? ThrottleLevel : 0.0f); }
        }

        public float RightStabilizerEnginePower
        {
            get { return IsPaused || IsCrashed ? 0.0f : RightStabilizerThrottleLevel; }
        }

        public float LeftStabilizerEnginePower
        {
            get { return IsPaused || IsCrashed ? 0.0f : LeftStabilizerThrottleLevel; }
        }

        public OnCrashEvent CrashEvent = new OnCrashEvent();
        public OnLandEvent LandEvent = new OnLandEvent();

        private Rigidbody cachedRigidbody;

        public void Update()
        {
            ProcessState();
        }

        private void ProcessState()
        {
            ProcessCrashTriggers();
            ProcessHeight();
            ProcessRigidbodyState();
            ProcessTouchdown();
        }

        private void ProcessCrashTriggers()
        {
            if (crashTrigger.IsTriggered)
            {
                Crash();
            }
        }

        private void ProcessTouchdown()
        {
            if (touchdownTrigger.Landed && !IsLanded)
            {
                DoLand();
            }
        }

        private void ProcessRigidbodyState()
        {
            if (cachedRigidbody == null)
            {
                cachedRigidbody = GetComponent<Rigidbody>();
            }

            Velosity = cachedRigidbody.velocity;
            Position = cachedRigidbody.position;
            Rotation = cachedRigidbody.rotation;
            AngularVelosity = cachedRigidbody.angularVelocity;
        }

        private void ProcessHeight()
        {
            RaycastHit hit;
            FlyHeight = Physics.Raycast(transform.position, Vector3.down, out hit) ? hit.distance : float.MaxValue;
        }

        public void Reset()
        {
            RemainingFuel = initialFuel;
            ThrottleLevel = 0.0f;
            LeftStabilizerThrottleLevel = 0.0f;
            RightStabilizerThrottleLevel = 0.0f;

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            if (cachedRigidbody == null)
            {
                cachedRigidbody = GetComponent<Rigidbody>();
            }

            cachedRigidbody.velocity = Vector3.zero;
            cachedRigidbody.angularVelocity = Vector3.zero;
            cachedRigidbody.isKinematic = false;

            IsLanded = false;
            IsCrashed = false;
            IsPaused = false;

            spaceshipModel.Reset();
            spaceshipGhost.Reset();
            touchdownTrigger.Reset();
            crashTrigger.Reset();
        }

        public ISpaceshipState Save()
        {
            return SpaceshipState.Create(RemainingFuel);
        }

        public void Restore(ISpaceshipState state)
        {
            RemainingFuel = state.RemainingFuel;
        }

        public void OnCollisionEnter(Collision collision)
        {
            ProcessCollisionEvent(collision);
        }

        private void ProcessCollisionEvent(Collision collision)
        {
            if (!LandingPlaceTest(collision))
            {
                Crash();
                return;
            }

            if (!VelosityTest(collision))
            {
                Crash();
                return;
            }
        }

        private void Crash()
        {
            if (!IsCrashed)
            {
                IsCrashed = true;
                BlowUp();

                CrashEvent.Invoke();
            }
        }

        private void DoLand()
        {
            if (!IsLanded)
            {
                IsLanded = true;
                Landed();
            }
        }

        private void Landed()
        {
            LandEvent.Invoke();
        }

        private bool VelosityTest(Collision collision)
        {
            return collision.relativeVelocity.magnitude < safeSpeed;
        }

        private bool LandingPlaceTest(Collision collision)
        {
            return collision.gameObject.layer == LayerMask.NameToLayer("Landing place");
        }

        private void BlowUp()
        {
            if (cachedRigidbody == null)
            {
                cachedRigidbody = GetComponent<Rigidbody>();
            }

            cachedRigidbody.isKinematic = true;
            spaceshipModel.Hide();
            spaceshipGhost.BlowUp(Velosity, RemainingFuel);
        }

		public void SetVelocity(Vector3 vel)
		{
			cachedRigidbody.velocity = vel;
		}
    }
}