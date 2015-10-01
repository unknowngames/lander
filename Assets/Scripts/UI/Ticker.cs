using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof (RectTransform))]
    public class Ticker : UIBehaviour
    {
        [Serializable]
        public class OnRewindEvent : UnityEvent{}

        [SerializeField] 
        public OnRewindEvent RewindEvent=new OnRewindEvent();

        [SerializeField]
        private Text text;

        [SerializeField]
        private float velocity;

        private RectTransform RectTransform
        {
            get { return (RectTransform) transform; }
        }

        public float Velocity
        {
            get { return velocity; }
            set
            {
                if (!Mathf.Approximately(velocity, value))
                {
                    velocity = value;
                    ResetPosition();
                }
            }
        }

        public string Text
        {
            get { return text.text; }
            set
            {
                if (text.text != value)
                {
                    text.text = value;
                    ResetPosition();
                }
            }
        }

        private float preferredWidth;
        private float xPosition;

        public void Update()
        {
            if (!Mathf.Approximately(preferredWidth, text.preferredWidth))
            {
                ResetPosition();
            }

            xPosition -= velocity*Time.deltaTime;
            Move();

            if (xPosition < -preferredWidth)
            {
                ResetPosition();
                RewindEvent.Invoke();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Move()
        {
            RectTransform textTransform = (RectTransform) text.transform;
            Vector2 anchoredPosition = textTransform.anchoredPosition;
            anchoredPosition.x = xPosition;
            textTransform.anchoredPosition = anchoredPosition;
        }

        private void ResetPosition()
        {
            preferredWidth = text.preferredWidth;
            xPosition = RectTransform.rect.width;
            Move();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            ResetPosition();
        }
    }
}
