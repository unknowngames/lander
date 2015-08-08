using UnityEngine;

namespace Assets.Scripts.Spaceship.Effects.Sounds
{
    public class SpaceshipCrashSound : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        private SpaceshipBehaviour spaceshipBehaviour;

        public void Awake()
        {
            spaceshipBehaviour.CrashEvent.AddListener(CrashEvent);
        }

        private void CrashEvent()
        {
            float volume = spaceshipBehaviour.Velosity.magnitude * 0.01f * spaceshipBehaviour.RemainingFuel * 0.01f;
            volume = Mathf.Clamp(volume, 0.0f, 1.0f);
            audioSource.PlayOneShot(audioClip, volume);
        }
    }
}
