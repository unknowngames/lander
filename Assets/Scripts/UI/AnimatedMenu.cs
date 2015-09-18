using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class AnimatedMenu : MenuUI
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private CanvasGroup canvasGroup;

        public Animator Animator
        {
            get { return animator ?? (animator = GetComponent<Animator>()); }
        }

        public CanvasGroup CanvasGroup
        {
            get { return canvasGroup ?? (canvasGroup = GetComponent<CanvasGroup>()); }
        }

        [ContextMenu("Show")]  
        public override void Show()
        {
            Animator.SetTrigger("DoShow");
        }

        [ContextMenu("Hide")]
        public override void Hide()
        {
            Animator.SetTrigger("DoHide");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Show();
        }
    }
}