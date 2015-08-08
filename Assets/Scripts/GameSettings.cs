using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
    public static class GameSettings
    {
        private const string DifficultyKey = "DifficultyKey";

        public static void ApplyDifficulty(IGame game, IGameDifficultyStorage storage)
        {
            IGameDifficulty savedDifficulty = GetSavedDifficulty(storage);
            savedDifficulty.Apply(game);
        }

        public static IGameDifficulty GetSavedDifficulty(IGameDifficultyStorage storage)
        {
            if (PlayerPrefsX.HasKey(DifficultyKey))
            {
                string key = PlayerPrefsX.GetString(DifficultyKey);
                if (storage.IsExist(key))
                {
                    return storage[key];
                }
            }
            return storage.Difficulties[0];
        }
    }
}