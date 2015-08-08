using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class ShadowMapTransform : MonoBehaviour 
    {
        Vector3 positionOffset = Vector3.zero;
        Transform target = null;

        public void Awake ()
        {                                           
            positionOffset = transform.localPosition;
            target = transform.parent;
            transform.parent = null;
        }

        public void LateUpdate() 
        {
            transform.position = target.position + positionOffset;
        }
    }
}
