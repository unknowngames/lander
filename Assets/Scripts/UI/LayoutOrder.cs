using UnityEngine;
using UnityEngine.EventSystems;

public class LayoutOrder : UIBehaviour
{
    [SerializeField]
    private int order;

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.SetSiblingIndex(order);
    }
}
