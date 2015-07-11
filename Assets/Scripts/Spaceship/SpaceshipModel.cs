using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipModel : MonoBehaviour
    {
        public void Reset ()
        {
            gameObject.SetActive (true);
        }

        public void Hide ()
        {
            gameObject.SetActive(false);
        }
    }
}