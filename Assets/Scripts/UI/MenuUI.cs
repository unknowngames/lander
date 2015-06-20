using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public abstract class MenuUI : UIBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}