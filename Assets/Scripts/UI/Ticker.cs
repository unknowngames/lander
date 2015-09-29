using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof (RectTransform))]
    public class Ticker : UIBehaviour
    {
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
                    ResetPosition();
                    velocity = value;
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
                    ResetPosition();
                    text.text = value;
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

            Vector3 localPosition = text.transform.localPosition;
            localPosition.x = xPosition;
            text.transform.localPosition = localPosition;
        }

        private void ResetPosition()
        {
            preferredWidth = text.preferredWidth;
            xPosition = RectTransform.rect.width;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            ResetPosition();
        }
    }
}
