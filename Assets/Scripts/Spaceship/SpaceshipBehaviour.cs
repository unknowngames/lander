using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipBehaviour : MonoBehaviour, ISpaceship
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Velosity { get; set; }
        public Vector3 AngularVelosity { get; set; }
        public float Mass { get; set; }
        public float RemainingFuel { get; set; }      
        public bool IsPaused { get; set; }

		public delegate void LandedEventHandler();
		public event LandedEventHandler OnLanded;

		public delegate void CrashedEventHandler();
		public event CrashedEventHandler OnCrashed;

		public delegate void BumpedEventHandler();
		public event BumpedEventHandler OnBumped;

		public void Update()
		{
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

        public void Reset ()
        {
            RemainingFuel = 750;

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            Rigidbody component = GetComponent<Rigidbody>();
            if (component != null)
            {
                component.velocity = Vector3.zero;
                component.angularVelocity = Vector3.zero;
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Bump");
        } 
    }
}