using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class SpaceButtonAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private UIBehaviour uiBehaviour;

        public void SetExpanded(bool value)
        {
            animator.SetBool("expanded", value);
        }
    }
}
