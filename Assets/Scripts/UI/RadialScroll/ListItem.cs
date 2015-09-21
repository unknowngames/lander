using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = System.Object;

namespace Assets.Scripts.UI.RadialScroll
{
    [RequireComponent(typeof(RectTransform))]
    public class ListItem : UIBehaviour
    {
        [SerializeField]
        private RectTransform rectTransform;

        public RectTransform RectTransform
        {
            get { return rectTransform ?? (rectTransform = (RectTransform) transform); }
        }
    
        private Object value;

        public Object Value
        {
            set
            {
                this.value = value;
            }
            get
            {
                return value;
            }
        }

        public OnListItemClickEvent OnClick = new OnListItemClickEvent();
        public Button Button;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Button != null)
            {
                Button.onClick.AddListener(Click);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable ();
            if (Button != null)
            {
                Button.onClick.RemoveListener(Click);
            }
        }

        protected void Click()
        {
            OnClick.Invoke(this);
        }
    }
}