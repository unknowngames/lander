using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Settings
{
	public class AudioSettings : MonoBehaviour
    {
        private const string AudioVolumeKey = "AudioVolumeKey";

	    public void Start()
        {
            UpdateAudioListener();
	    }

        public static bool IsSoundEnabled()
        {
            bool isSoundEnabled = false;
            if (PlayerPrefsX.HasKey(AudioVolumeKey))
            {
                isSoundEnabled = PlayerPrefsX.GetBool(AudioVolumeKey);
            }
            return isSoundEnabled;
        }

        public static void SetSoundEnabled(bool isSoundEnabled)
	    {
            PlayerPrefsX.SetBool(AudioVolumeKey, isSoundEnabled);
            UpdateAudioListener();
	    }

        private static void UpdateAudioListener()
	    {
            AudioListener.volume = IsSoundEnabled() ? 1.0f : 0.0f;
	    }
    }
}
