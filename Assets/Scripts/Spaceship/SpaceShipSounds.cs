using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Spaceship
{
	public class SpaceShipSounds : MonoBehaviour 
	{
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


		private AudioSource engineAudioSource;
		private ISpaceshipMoveable spaceshipMoveable;
		void Start () 
		{
			engineAudioSource = GetComponent<AudioSource> ();
			spaceshipMoveable = Game.PlayerSpaceship.GetComponent<ISpaceshipMoveable> ();
			SpaceshipBehaviour.OnLanded += OnLandedEventHandler;
		}
		void Update() 
		{
			UpdateEngineSound();
		}
		private void UpdateEngineSound()
		{
			if (spaceshipMoveable.ThrottleLevel == 0.0f) 
			{
				engineAudioSource.volume = 0.0f;
				return;
			}
			
			engineAudioSource.pitch = Mathf.Clamp(spaceshipMoveable.ThrottleLevel * configPitchMultiplier, configMinPitchMultiplier, configMaxPitchMultiplier);
			engineAudioSource.volume = Mathf.Clamp(spaceshipMoveable.ThrottleLevel * configVolumeMultiplier, configMinVolumeMultiplier, configMaxVolumeMultiplier);
		}
		private void OnLandedEventHandler(LandedType type)
		{
		}
		void OnDestroy()
		{
			SpaceshipBehaviour.OnLanded -= OnLandedEventHandler;
		}
	}
}