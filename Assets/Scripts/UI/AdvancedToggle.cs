using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Toggle))]
    public class AdvancedToggle : UIBehaviour
    {
        [SerializeField]
        private Toggle toggle;

        public Toggle Toggle
        {
            get { return toggle ?? (toggle = GetComponent<Toggle>()); }
        }

        [SerializeField]
        public Image image;
    
        [SerializeField]
        public Sprite onSprite;
        [SerializeField]
        public Sprite offSprite;

        public bool IsOn
        {
            get { return Toggle.isOn; }
            set { Toggle.isOn = value; }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Toggle.onValueChanged.AddListener(OnValueChanged);
            UpdateImage(Toggle.isOn);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            UpdateImage(value);
        }

        private void UpdateImage(bool value)
        {
            image.sprite = value ? onSprite : offSprite;
        }
    }
}