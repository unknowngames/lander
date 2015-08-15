using Assets.Scripts.Settings;
using UnityEditor;

namespace Assets.Scripts.Editor
{
    public static class GameDifficultlyStorageIntegration
    {
        [MenuItem("Assets/Create/Game difficulty storage")]
        public static void CreateGameDifficultyStorage()
        {
            ScriptableObjectUtility2.CreateAsset<GameDifficultyStorage>();
        }
    }
}