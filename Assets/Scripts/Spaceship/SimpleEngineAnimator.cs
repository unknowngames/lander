using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SimpleEngineAnimator : MonoBehaviour 
    {
        [SerializeField]
        private SpaceshipBehaviour spaceship;

        [SerializeField]
        private ParticleEmitter emitter;

        public void Update ()
        {
            emitter.maxEmission = spaceship.EnginePower * 10000.0f;
        }   
    }
}
