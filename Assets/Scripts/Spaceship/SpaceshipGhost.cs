using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipGhost : MonoBehaviour
    {
		[SerializeField]
		private float crashExplosionRadius = 400.0f;
		[SerializeField]
		private float crashExplosionForceMultiplier	= 1.0f;
		
        private Transform cashedParent;

        public void Awake ()
        {
            cashedParent = transform.parent;
        }

		public void BlowUp(Collision collision, float remainingFuel)
        {
            gameObject.SetActive(true);

			Rigidbody[] parts = GetComponentsInChildren<Rigidbody> ();
			foreach(Rigidbody part in parts)
			{
				part.AddExplosionForce(collision.relativeVelocity.magnitude * crashExplosionForceMultiplier * remainingFuel, transform.parent.position, crashExplosionRadius);
			}
            transform.parent = null;
        }

        public void Reset()
        {
            transform.parent = cashedParent;
            gameObject.SetActive(false);
        }
    }
}