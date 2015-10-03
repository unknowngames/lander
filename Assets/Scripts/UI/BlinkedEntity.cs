using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Animator))]
    public class BlinkedEntity : HighlightedEntity
    {
        [SerializeField]
        private Animator animator;

        private Animator Animator
        {
            get { return animator ?? (animator = GetComponent<Animator>()); }
        }
         

        public override void Do()
        {
            animator.SetBool("IsBlinked", true);
        }

        public override void Stop()
        {
            animator.SetBool("IsBlinked", false);
        }
    }
}