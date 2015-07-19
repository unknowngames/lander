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
        private Vector3 cashedPosition;
        private Quaternion cashedRotation;

        private SpaceshipPart[] spaceshipParts;

        public void Awake ()
        {
            cashedParent = transform.parent;
            cashedPosition = transform.localPosition;
            cashedRotation = transform.localRotation;

            Rigidbody[] parts = GetComponentsInChildren<Rigidbody>();

            spaceshipParts = new SpaceshipPart[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                spaceshipParts[i]=new SpaceshipPart(parts[i]);
            }
        }

        public void BlowUp(Collision collision, float remainingFuel)
        {
            if (transform.parent == null)
            {
                return;
            }

            gameObject.SetActive(true);

            foreach (SpaceshipPart part in spaceshipParts)
            {
                part.Rigidbody.AddExplosionForce(collision.relativeVelocity.magnitude*crashExplosionForceMultiplier*remainingFuel, transform.parent.position, crashExplosionRadius);
            }
            transform.parent = null;
        }

        public void Reset()
        {
            transform.parent = cashedParent;

            transform.localPosition = cashedPosition;
            transform.localRotation = cashedRotation;

            gameObject.SetActive(false);

            foreach (SpaceshipPart part in spaceshipParts)
            {
                part.Reset();
            }
        }
    }
}