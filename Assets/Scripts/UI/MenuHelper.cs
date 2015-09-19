using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuHelper : MonoBehaviour
    {
        [SerializeField]
        private MenuUI startMenu;

        private MenuUI current;

        public void Start()
        {
            if (startMenu != null)
            {
                Show(startMenu);
            }
        }

        public void Show(MenuUI menu)
        {
            if (current != null)
            {
                current.Hide();
            }

            menu.Show();
            current = menu;
        }

        public void Hide(MenuUI menu)
        {
            if (current != null)
            {
                current.Hide();
            }
            current = null;
        }
    }
}