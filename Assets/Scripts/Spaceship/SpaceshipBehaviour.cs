using System.Collections.ObjectModel;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Session;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipBehaviour : MonoBehaviour, ISpaceship
    {
        [SerializeField]
        [FormerlySerializedAs("initialFuel")]
        private float maxFuel;
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

        public float MaxFuel
        {
            get { return maxFuel; }
        }

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

        private List<Collision> collisions = new List<Collision>();

        /// <summary>
        /// список последних коллизий корабля 
        /// </summary>
        public ReadOnlyCollection<Collision> Collisions
        {
            get
            {
                return new ReadOnlyCollection<Collision>(collisions);
            }
        }

        public float EnginePower
        {
            get { return Flight.Current.IsPaused || IsCrashed ? 0.0f : (RemainingFuel > 0.0f ? ThrottleLevel : 0.0f); }
        }

        public float RightStabilizerEnginePower
        {
            get { return Flight.Current.IsPaused || IsCrashed ? 0.0f : RightStabilizerThrottleLevel; }
        }

        public float LeftStabilizerEnginePower
        {
            get { return Flight.Current.IsPaused || IsCrashed ? 0.0f : LeftStabilizerThrottleLevel; }
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
            RemainingFuel = MaxFuel;
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

            spaceshipModel.Reset();
            spaceshipGhost.Reset();
            touchdownTrigger.Reset();
            crashTrigger.Reset();

            collisions.Clear();
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

            // коснулись зоны приземления, сохраняем информацию о касании для последующей обработки
            //Debug.Log("Collision: " + collision.relativeVelocity.magnitude);
            collisions.Add(collision);
        }

        private void Crash()
        {
            if (!IsCrashed)
            {
                IsCrashed = true;
                BlowUp();

                Flight.Current.FailFlight();
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
            Flight.Current.CompleteFlight();
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