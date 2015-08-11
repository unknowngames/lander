using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

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
