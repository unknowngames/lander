using Assets.Scripts.Settings;
using UnityEditor;

namespace Assets.Scripts.Editor
{
    public static class PlayerPrefabsStorageIntegration
    {
        [MenuItem("Assets/Create/Player prefabs storage")]
        public static void CreateGameDifficultyStorage()
        {
            ScriptableObjectUtility2.CreateAsset<PlayerPrefabsStorage>();
        }
    }
}