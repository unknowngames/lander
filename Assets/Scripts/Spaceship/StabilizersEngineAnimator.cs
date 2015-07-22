using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class StabilizersEngineAnimator : MonoBehaviour
    {
        [SerializeField]
        private SpaceshipBehaviour spaceship;

        [SerializeField]
        private ParticleSystem leftStabilizer;

        [SerializeField]
        private ParticleSystem rightStabilizer;


        public void Update ()
        {
            leftStabilizer.enableEmission = (spaceship.LeftStabilizerEnginePower > 0.0f);
            rightStabilizer.enableEmission = (spaceship.RightStabilizerEnginePower > 0.0f);
        }   
    }
}
