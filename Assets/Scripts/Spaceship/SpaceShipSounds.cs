using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Spaceship
{
    [Serializable]
    public class SpaceshipSoundStorage
    {
        [SerializeField]
        public AudioSource[] engineSounds;
        [SerializeField]
        public AudioSource[] bumpSounds;
        [SerializeField]
        public AudioSource[] crashSounds;
		[SerializeField]
		public AudioSource landedSound;

    }

	[RequireComponent(typeof(SpaceshipBehaviour))]
	public class SpaceShipSounds : MonoBehaviour 
	{
        [SerializeField]
        private SpaceshipSoundStorage spaceshipSounds;

		/** Настройка частоты звука работающего двигателя*/
		[SerializeField] 
		private float configPitchMultiplier	= 1.0f;
		/** минимальная частотота звука работающего двигателя*/
		[SerializeField]
		private float configMinPitchMultiplier	= 0.3f;
		[SerializeField]
		private float configMaxPitchMultiplier	= 1.0f;

		/** Настройка громкости звука работающего двигателя*/
		[SerializeField]
		private float configVolumeMultiplier = 1.0f;
		/** минимальная частотота звука работающего двигателя*/
		[SerializeField]
		private float configMinVolumeMultiplier	= 0.3f;
		[SerializeField]
		private float configMaxVolumeMultiplier	= 1.0f;
		[SerializeField]
		private float configBumpVolumeMultiplier = 0.01f;

		[SerializeField]
		private float configCrashFuelVolumeMultiplier = 0.01f;

		[SerializeField]
		private float configCrashForceVolumeMultiplier = 0.01f;


		private	SpaceshipBehaviour spaceshipBehaviour;

		// звуки
		private AudioSource engineAudioSource;

		public void Awake()
		{
			spaceshipBehaviour = GetComponent<SpaceshipBehaviour> ();
			engineAudioSource = spaceshipSounds.engineSounds[0];
		}

	    public void Start ()
	    {
	        spaceshipBehaviour.BumpEvent.AddListener (OnBumpedEventHandler);
	    }

	    public void OnDestroy()
	    {
	        spaceshipBehaviour.BumpEvent.RemoveListener (OnBumpedEventHandler);
	    }

		void Update() 
		{
			UpdateEngineSound();
		}

		private void UpdateEngineSound()
		{
            if (Math.Abs (spaceshipBehaviour.EnginePower) < 0.01f) 
			{
				engineAudioSource.volume = 0.0f;
				return;
			}

            engineAudioSource.pitch = Mathf.Clamp(spaceshipBehaviour.ThrottleLevel * configPitchMultiplier, configMinPitchMultiplier, configMaxPitchMultiplier);
            engineAudioSource.volume = Mathf.Clamp(spaceshipBehaviour.ThrottleLevel * configVolumeMultiplier, configMinVolumeMultiplier, configMaxVolumeMultiplier);
		}

        private void OnBumpedEventHandler(BumpInfo bumpInfo)
		{
            if (!bumpInfo.IsLanded && !bumpInfo.IsCrashed)
            {
				float volumeMultiplier = bumpInfo.mcollision.relativeVelocity.magnitude * configBumpVolumeMultiplier;
				volumeMultiplier = Mathf.Clamp(volumeMultiplier, 0.0f, 1.0f);
				
				PlayRandomSound(spaceshipSounds.bumpSounds, volumeMultiplier);
            }

            if (bumpInfo.IsLanded && !bumpInfo.IsCrashed)
            {
                spaceshipSounds.landedSound.Play();
            }

            if (!bumpInfo.IsLanded && bumpInfo.IsCrashed)
            {
				float volumeMultiplier = bumpInfo.mcollision.relativeVelocity.magnitude * 0.01f * spaceshipBehaviour.RemainingFuel * configCrashFuelVolumeMultiplier;
				volumeMultiplier = Mathf.Clamp(volumeMultiplier, 0.0f, 1.0f);
				PlayRandomSound(spaceshipSounds.crashSounds, volumeMultiplier);
            }    
		}

		private void PlayRandomSound(AudioSource[] audioSorces, float volumeMultiplier)
		{
			int rnd = Random.Range (0, audioSorces.Length);
			audioSorces[rnd].volume = volumeMultiplier;
			audioSorces[rnd].Play ();
		}
	}
}