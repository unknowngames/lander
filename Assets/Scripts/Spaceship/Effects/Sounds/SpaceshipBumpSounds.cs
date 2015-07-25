using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Spaceship.Effects.Sounds
{          
	public class SpaceshipBumpSounds : MonoBehaviour
    {                                                    
        [SerializeField]
        public AudioSource audioSoruce;

        [SerializeField]
        public AudioClip[] audioClips;

	    [SerializeField]
	    private float configBumpVolumeMultiplier;

        public void OnCollisionEnter(Collision collision)
        {
            OnBump(collision.relativeVelocity.magnitude);
        }       

	    private void OnBump(float relativeSpeed)
	    {
            float volumeMultiplier = relativeSpeed * configBumpVolumeMultiplier;
	        volumeMultiplier = Mathf.Clamp(volumeMultiplier, 0.0f, 1.0f);

            PlayRandomClip(volumeMultiplier);
	    }

	    private void PlayRandomClip(float volumeMultiplier)
		{
            int rnd = Random.Range(0, audioClips.Length);
	        audioSoruce.PlayOneShot(audioClips[rnd], volumeMultiplier);
		}
	}
}