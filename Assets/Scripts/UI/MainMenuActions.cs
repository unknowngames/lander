using Assets.Scripts.UI.HUD;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuActions : MonoBehaviour
    {
        [SerializeField]
        private SpaceshipPlanetRotation planetRotation;

		[SerializeField]
		private UnityEngine.UI.Text topScoreLabel;

		[SerializeField]
		private Settings.GameDifficultyStorage difficultyStorage;

        [SerializeField]
        private bool reallyLoadLevel = true;

        public void Start()
        {
            planetRotation.DoRotationSlower();
			string topScoreLabelText = "Текущий рекорд: ";
			topScoreLabel.text = topScoreLabelText + Session.GameSessionPlayerPrefsProxy.GetTopScore (difficultyStorage.GetSavedDifficulty ().Name).ToString();
        }

        public void OnStart()
        {
            planetRotation.DoRotationFaster();
            if (reallyLoadLevel)
            {
                Application.LoadLevelAsync (1);
            }
        }
    }
}