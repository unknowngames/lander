using Assets.Scripts.Interfaces;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DifficultyButtonActions : MonoBehaviour
    {
        [SerializeField]
        private Text difficultyText;

        [SerializeField]
        private GameDifficultyStorage gameDifficultyStorage;

        private IGameDifficulty currentDifficulty;
        private int currentDifficultyIndex;

        private IGameDifficultyStorage GameDifficultyStorage
        {
            get { return gameDifficultyStorage; }
        }

        private IGameDifficulty CurrentDifficulty
        {
            get { return currentDifficulty; }
            set
            {
                currentDifficulty = value;
                currentDifficultyIndex = GameDifficultyStorage.GetIndex(currentDifficulty);

                difficultyText.text = currentDifficulty.Name;
            }
        }

        private IGameDifficulty NextDifficulty()
        {
            if (currentDifficultyIndex + 1 >= GameDifficultyStorage.DifficultiesCount)
            {
                return GameDifficultyStorage[0];
            }
            return GameDifficultyStorage[currentDifficultyIndex + 1];
        }

        public void Start()
        {
            CurrentDifficulty = GameDifficultyStorage.GetSavedDifficulty();
        }

        public void OnDifficultyChangeClick()
        {
            CurrentDifficulty = NextDifficulty();

            GameDifficultyStorage.SetDifficulty(CurrentDifficulty);
        }
    }
}