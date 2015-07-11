using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipGhost : MonoBehaviour
    {
        public void BlowUp()
        {
            gameObject.SetActive(true);
        }

        public void Reset()
        {
            gameObject.SetActive(false);
        }
    }
}