using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Settings
{
    public static class GameSettings
    {
        private const string PlayerPrefabKey = "PlayerPrefabKey";

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