using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Spaceship
{
	[RequireComponent(typeof(ISpaceshipMoveable))]
	[RequireComponent(typeof(SpaceshipBehaviour))]
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

		private ISpaceshipMoveable spaceshipMoveable;
		private	SpaceshipBehaviour spaceshipBehaviour;
		private const int bumpAudioSourcesCount = 3;
		
		// звуки
		private AudioSource engineAudioSource;
		private List<AudioSource> bumpAudioSources;



		public void Awake()
		{
			spaceshipBehaviour = GetComponent<SpaceshipBehaviour> ();
			spaceshipMoveable = GetComponent<ISpaceshipMoveable> ();

			AudioSource[] audioSources = GetComponents<AudioSource>();
			engineAudioSource = audioSources[0];
			bumpAudioSources = new List<AudioSource> ();
			
			for (int i = 1; i <  audioSources.Length; i++) 
			{
				bumpAudioSources.Add(audioSources[i]);
			}
		}                     

		public void Start () 
		{

			spaceshipBehaviour.OnLanded += OnLandedEventHandler;
			spaceshipBehaviour.OnBumped += OnBumpedEventHandler;
			spaceshipBehaviour.OnCrashed += OnCrashedEventHandler;
		}

		public void OnDestroy()
		{
			spaceshipBehaviour.OnLanded -= OnLandedEventHandler;
			spaceshipBehaviour.OnBumped -= OnBumpedEventHandler;
			spaceshipBehaviour.OnCrashed -= OnCrashedEventHandler;
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

		private void OnLandedEventHandler()
		{
			Debug.Log ("OnLandedEventHandler");
		}
		private void OnBumpedEventHandler()
		{
			int rnd = Random.Range(0, bumpAudioSources.Count);
			bumpAudioSources [rnd].Play ();
		}

		private void OnCrashedEventHandler()
		{
			Debug.Log ("OnCrashedEventHandler");
		}
	}
}