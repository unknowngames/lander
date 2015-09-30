using System;
using UnityEngine;
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
        
        [SerializeField]
        private bool delayedShow;
        [SerializeField]
        private float delay;

        [SerializeField]
        private bool resetPositionAtStart;
        [SerializeField]
        private bool hideAtStart;

        public bool DelayedShow
        {
            get { return delayedShow; }
        }

        public float Delay
        {
            get { return delay; }
        }

        public OnShowEvent OnShow = new OnShowEvent();
        public OnHideEvent OnHide = new OnHideEvent();

        protected override void Awake()
        {
            base.Awake();

            if (hideAtStart)
            {
                Hide();
            }

            if (resetPositionAtStart)
            {
                transform.localPosition = Vector3.zero; 
            }
        }
        
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