using UnityEngine;

namespace Assets.Scripts.Environment
{
    public abstract class LandscapeBase : MonoBehaviour
    {
        public abstract Bounds Bounds { get; }

        public virtual Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }
    }
}