using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuUI : MenuUI
    {
        public GameObject Loading;

        public void OnStart()
        {
            Hide();

            if (Loading != null)
            {
                Loading.SetActive(true);
            }

            Application.LoadLevelAsync (1);
        }
    }
}