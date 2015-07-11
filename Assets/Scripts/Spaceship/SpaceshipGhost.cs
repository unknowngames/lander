using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipGhost : MonoBehaviour
    {
        private Transform cashedParent;

        public void Awake ()
        {
            cashedParent = transform.parent;
        }

        public void BlowUp()
        {
            transform.parent = null;
            gameObject.SetActive(true);
        }

        public void Reset()
        {
            transform.parent = cashedParent;
            gameObject.SetActive(false);
        }
    }
}