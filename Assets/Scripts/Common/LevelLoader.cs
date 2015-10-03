using UnityEngine;

namespace Assets.Scripts.Common
{
    public class LevelLoader : MonoBehaviour
    {
        public void LoadLevel(string levelName)
        {
            Application.LoadLevelAsync(levelName);
        }
    }
}
