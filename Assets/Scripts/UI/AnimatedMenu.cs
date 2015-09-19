using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class AnimatedMenu : MenuUI, IInteractable
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private CanvasGroup canvasGroup;

        private InteractableStateBehaviour[] interactableStateBehaviours;

        public Animator Animator
        {
            get { return animator ?? (animator = GetComponent<Animator>()); }
        }

        private CanvasGroup CanvasGroup
        {
            get { return canvasGroup ?? (canvasGroup = GetComponent<CanvasGroup>()); }
        }

        public bool Interactable
        {
            get { return CanvasGroup.interactable; }
            set { CanvasGroup.interactable = value; }
        }

        protected override void Awake()
        {         
            base.Awake(); 
       
            interactableStateBehaviours = Animator.GetBehaviours<InteractableStateBehaviour>();
            foreach (InteractableStateBehaviour stateBehaviour in interactableStateBehaviours)
            {
                stateBehaviour.InteractableBehaviour = this;
            }
        }

        [ContextMenu("Show")]  
        public override void Show()
        {
            Animator.SetBool("IsHidden", false);    
            OnShow.Invoke();
        }

        [ContextMenu("Hide")]
        public override void Hide()
        {
            Animator.SetBool("IsHidden", true);
            OnHide.Invoke();
        }          
    }
}