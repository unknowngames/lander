using System;
using UnityEngine;

namespace Assets.Scripts.Spaceship.Effects.Animators
{
    public class RocketEngineAnimator : MonoBehaviour 
    {
        [Serializable]
        private class ParticleSystemParameters
        {
            [SerializeField]
            private float minSpeedScale;
            [SerializeField]
            private float maxSpeedScale;

            public float MinSpeedScale
            {
                get { return minSpeedScale; }
            }

            public float MaxSpeedScale
            {
                get { return maxSpeedScale; }
            }
        }

        [SerializeField]
        private SpaceshipBehaviour spaceship;

        [SerializeField]
        private ParticleSystem particleSystem;

        [SerializeField]
        private ParticleSystemParameters parameters;


        private ParticleSystemRenderer mainRenderer;
        private ParticleSystemRenderer MainRenderer
        {
            get { return mainRenderer ?? (mainRenderer = particleSystem.GetComponent<ParticleSystemRenderer>()); }
        }

        [SerializeField]
        private bool debugMode;

        [SerializeField]
        [Range(0, 1)]   
        private float throttleLevel;

        // ReSharper disable once UnusedMember.Global
        public void Update ()
        {
            if (!debugMode)
            {
                throttleLevel = spaceship.EnginePower;
            }

            float scale = spaceship.transform.localScale.x;

            MainRenderer.velocityScale = (parameters.MaxSpeedScale - parameters.MinSpeedScale) * throttleLevel * scale;
        }   
    }
}
