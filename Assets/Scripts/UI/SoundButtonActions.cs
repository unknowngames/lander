using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AudioSettings = Assets.Scripts.Settings.AudioSettings;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(AdvancedToggle))]
	public class SoundButtonActions : MonoBehaviour
    {
        [SerializeField]
        private AdvancedToggle advancedToggle;
        private AdvancedToggle AdvancedToggle
        {
            get { return advancedToggle ?? (advancedToggle = GetComponent<AdvancedToggle>()); }
        }

	    public void Start()
	    {
            bool isSoundEnabled = AudioSettings.IsSoundEnabled();

            AdvancedToggle.IsOn = isSoundEnabled;
	    }

        public void OnMuteChanged(bool isSoundEnabled)
	    {
            AudioSettings.SetSoundEnabled(isSoundEnabled);
	    }
	}
}
