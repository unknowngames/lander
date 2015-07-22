using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class LandingZone : MonoBehaviour
    {
        [SerializeField]
        [Range (1, 10)]
        private int scoreMultiplier;

        public int ScoreMultiplier
        {
            get
            {
                return scoreMultiplier;
            }
        }

    }
}
