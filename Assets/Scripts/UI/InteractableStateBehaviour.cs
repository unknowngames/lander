using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InteractableStateBehaviour : StateMachineBehaviour
    {
        [SerializeField]
        private bool interactable=false;

        public IInteractable InteractableBehaviour { get; set; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (InteractableBehaviour != null)
            {
                InteractableBehaviour.Interactable = interactable;
            }
        }
    }
}
