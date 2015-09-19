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


        public void OnEnable()
        {
            Toggle.onValueChanged.AddListener(OnValueChanged);
            UpdateImage(Toggle.isOn);
        }

        public void OnDisable()
        {
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