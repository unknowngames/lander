using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
	public enum LandedType
	{
		Norm, Bumped, Crashed
	};
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

		public delegate void LandedEventHandler(LandedType e);
		public static event LandedEventHandler OnLanded;

        public void Reset ()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            Rigidbody component = GetComponent<Rigidbody>();
            if (component != null)
            {
                component.velocity = Vector3.zero;
                component.angularVelocity = Vector3.zero;
            }
        }
    }
}