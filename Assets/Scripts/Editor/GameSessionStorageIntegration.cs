using Assets.Scripts.Session;
using UnityEditor;

namespace Assets.Scripts.Editor
{
    public static class GameSessionStorageIntegration
    {
        [MenuItem("Assets/Create/Game session storage")]
        public static void GameSessionStorage()
        {
            ScriptableObjectUtility2.CreateAsset<GameSessionStorage>();
        }
    }
}