using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public abstract class MenuUI : UIBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}