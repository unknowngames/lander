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

        public string Name { get; set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private  set; }
        public Vector3 Velosity { get; private  set; }
        public Vector3 AngularVelosity { get; private set; }
        public float Mass { get; set; }
        public float RemainingFuel { get; set; }
        public float ThrottleLevel { get; set; }
        public float FlyHeight { get; private set; }  
        public bool IsCrashed { get; private set; }


        public bool IsPaused { get; set; }
        public float EnginePower { get; private set; }

        public OnBumpEvent BumpEvent = new OnBumpEvent();

        public OnCrashEvent CrashEvent = new OnCrashEvent();
        public OnLandEvent LandEvent = new OnLandEvent();

        private Rigidbody cachedRigidbody;

        public void Update ()
        {
            ProcessEngine (); 
            ProcessState ();
        }

        private void ProcessState ()
        {
            ProcessHeight ();
            ProcessRigidbodyState();
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
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                FlyHeight = hit.distance;
            }
            else
            {
                FlyHeight = float.MaxValue;
            }
        }

        private void ProcessEngine ()
        {
            EnginePower = IsPaused ? 0.0f : (RemainingFuel > 0.0f ? ThrottleLevel : 0.0f);
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
                Crash ();
                return;
            }

            if (!VelosityTest(collision))
            {
                Crash();
                return;
            }

            Bump ();
        }

        private void Crash()
        {
            IsCrashed = true;
            BlowUp ();

            CrashEvent.Invoke ();
            BumpEvent.Invoke(new BumpInfo
            {
                IsCrashed = true,
                IsLanded = false
            });
        }

        private void Bump()
        {
            BumpEvent.Invoke(new BumpInfo
            {
                IsCrashed = false,
                IsLanded = false
            });
        }

        private void Landed()
        {
            BumpEvent.Invoke(new BumpInfo
            {
                IsCrashed = false,
                IsLanded = true
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

        private void BlowUp ()
        {                                      
            if (cachedRigidbody == null)
            {
                cachedRigidbody = GetComponent<Rigidbody>();
            }

            cachedRigidbody.isKinematic = true;
            spaceshipModel.Hide();
            spaceshipGhost.BlowUp ();
        }
    }
}