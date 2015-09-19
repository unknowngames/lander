using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Settings
{
    public static class GameSettings
    {
        private const string DifficultyKey = "DifficultyKey";
        private const string PlayerPrefabKey = "PlayerPrefabKey";

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

        public static void SetDifficulty(IGameDifficultyStorage storage, IGameDifficulty difficulty)
        {
            if (storage.IsExist(difficulty.Name))
            {
                PlayerPrefsX.SetString(DifficultyKey, difficulty.Name);
            }
        }

        public static void SetPlayerPrefabKey(string key)
        {
            PlayerPrefsX.SetString(PlayerPrefabKey, key);
        }

        public static string GetSavedPlayerPrefabKey()
        {
            if (PlayerPrefsX.HasKey(PlayerPrefabKey))
            {
                string key = PlayerPrefsX.GetString(PlayerPrefabKey);  
                return key;
            }
            
            throw new KeyNotFoundException();
        }
    }
}