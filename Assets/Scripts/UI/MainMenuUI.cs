using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuUI : MenuUI
    {
        public void OnStart()
        {
            Hide();
            Application.LoadLevelAsync (1);
        }
    }
}