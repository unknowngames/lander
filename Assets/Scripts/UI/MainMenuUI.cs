using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuUI : MenuUI
    {
        public void OnStart()
        {
            Application.LoadLevelAsync (1);
        }
    }
}