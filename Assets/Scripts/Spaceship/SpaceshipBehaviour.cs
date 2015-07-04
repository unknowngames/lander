using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipBehaviour : MonoBehaviour, ISpaceship
    {
        [SerializeField]
        private float initialFuel;

        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Velosity { get; set; }
        public Vector3 AngularVelosity { get; set; }
        public float Mass { get; set; }
        public float RemainingFuel { get; set; }  
        public float ThrottleLevel { get; set; }


        public bool IsPaused { get; set; }
        public float EnginePower { get; private set; }

        public delegate void LandedEventHandler();
		public event LandedEventHandler OnLanded;

		public delegate void CrashedEventHandler();
		public event CrashedEventHandler OnCrashed;

		public delegate void BumpedEventHandler();
		public event BumpedEventHandler OnBumped;

		public void Update()
		{
		    ProcessEngine ();

			// тут всякие проверки и вызов OnLanded, OnCrashed, OnBumped
			if (Input.GetKeyDown(KeyCode.L)) 
			{
				if (OnLanded != null) 
				{
					OnLanded ();
				}
			}
			if (Input.GetKeyDown(KeyCode.B)) 
			{
				if (OnBumped != null) 
				{
					OnBumped ();
				}
			}
			if (Input.GetKeyDown(KeyCode.C)) 
			{
				if (OnCrashed != null) 
				{
					OnCrashed ();
				}
			}
		}

        private void ProcessEngine ()
        {
            EnginePower = RemainingFuel > 0.0f ? ThrottleLevel : 0.0f;
        }

        public void Reset ()
        {
            RemainingFuel = initialFuel;

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            Rigidbody component = GetComponent<Rigidbody>();
            if (component != null)
            {
                component.velocity = Vector3.zero;
                component.angularVelocity = Vector3.zero;
            }
        }

		private bool isCrashed(Collision collision)
		{
			return false;
		}
		private bool isLanded(Collision collision)
		{
			if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Landing place")))
			{
				return true;
			}
			return false;
		}


        public void OnCollisionEnter(Collision collision)
        {
			if (isCrashed (collision)) 
			{
				OnCrashed();
			}
			else if (isLanded (collision)) 
			{
				OnLanded();
			}
			else 
			{
				OnBumped();
			}
        } 
    }
}