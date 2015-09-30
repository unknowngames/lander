using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuHelper : MonoBehaviour
    {
        [SerializeField] private MenuUI startMenu;

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

            if (menu.DelayedShow)
            {
                Show(menu.Delay, menu);
            }
            else
            {
                menu.Show();
            }
            current = menu;
        }

        public void Show(float timeout, MenuUI menu)
        {
            StartCoroutine(ShowIE(timeout, menu));
        }

        private IEnumerator ShowIE(float timeout, MenuUI menu)
        {
            yield return new WaitForSeconds(timeout);

            menu.Show();
        }
    }
}