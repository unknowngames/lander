using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public abstract class MenuUI : UIBehaviour
    {
        [Serializable]
        public class OnShowEvent : UnityEvent { }
        [Serializable]
        public class OnHideEvent : UnityEvent { }

        public OnShowEvent OnShow = new OnShowEvent();
        public OnHideEvent OnHide = new OnHideEvent();

        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide.Invoke();
        }
    }
}