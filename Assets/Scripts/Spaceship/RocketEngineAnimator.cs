using System;
using UnityEngine;

namespace Assets.Scripts.Spaceship
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
        private ParticleSystem main;
        [SerializeField]
        private ParticleSystem glow;

        [SerializeField]
        private ParticleSystemParameters mainParameters;
        [SerializeField]
        private ParticleSystemParameters glowParameters;


        private ParticleSystemRenderer mainRenderer;
        private ParticleSystemRenderer MainRenderer
        {
            get { return mainRenderer ?? (mainRenderer = main.GetComponent<ParticleSystemRenderer>()); }
        }
        
        private ParticleSystemRenderer glowRenderer;
        private ParticleSystemRenderer GlowRenderer
        {
            get { return glowRenderer ?? (glowRenderer = glow.GetComponent<ParticleSystemRenderer>()); }
        }

        [SerializeField]
        private bool debugMode;

        [SerializeField]
        [Range(0, 1)]   
        private float throttleLevel;

        public void Update ()
        {
            if (!debugMode)
            {
                throttleLevel = spaceship.EnginePower;
            }

            MainRenderer.velocityScale = (mainParameters.MaxSpeedScale - mainParameters.MinSpeedScale) * throttleLevel;
            GlowRenderer.velocityScale = (glowParameters.MaxSpeedScale - glowParameters.MinSpeedScale) * throttleLevel;
        }   
    }
}
