using UnityEditor;

namespace Assets.Scripts.Editor
{
    public static class GameDifficultlyStorageIntegration
    {
        [MenuItem("Assets/Create/GameDifficultyStorage")]
        public static void CreateGameDifficultyStorage()
        {
            ScriptableObjectUtility2.CreateAsset<GameDifficultyStorage>();
        }
    }
}