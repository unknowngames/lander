using System;
using UnityEngine;

namespace Assets.Scripts.Spaceship.Effects.Sounds
{
    public class RocketEngineSound : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        private SpaceshipBehaviour spaceshipBehaviour; 
        
        [SerializeField]
        private float pitchMultiplier = 1.0f;
        [SerializeField]
        private float minPitch = 0.5f;
        [SerializeField]
        private float maxPitch = 1.0f;

        [SerializeField]
        private float volumeMultiplier = 1.0f;
        [SerializeField]
        private float minVolume = 0.3f;
        [SerializeField]
        private float maxVolume = 1.0f;

        public void Start()
        {
            audioSource.clip = audioClip;
        }

        public void Update()
        {
            UpdateEngineSound();
        }

        private void UpdateEngineSound()
        {
            if (Math.Abs(spaceshipBehaviour.EnginePower) < 0.01f)
            {
                audioSource.volume = 0.0f;
                return;
            }

            audioSource.pitch = Mathf.Clamp(spaceshipBehaviour.ThrottleLevel * pitchMultiplier, minPitch, maxPitch);
            audioSource.volume = Mathf.Clamp(spaceshipBehaviour.ThrottleLevel * volumeMultiplier, minVolume, maxVolume);
        }
    }
}