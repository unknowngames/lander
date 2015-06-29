using UnityEngine;

namespace Assets.Scripts
{
    public class Landscape : MonoBehaviour
    {
        [SerializeField]
        private Renderer renderer;

        public Bounds Bounds
        {
            get
            {
                if (renderer != null)
                {
                    return renderer.bounds;
                }
                return new Bounds();
            }
        }

        public bool IsInViewport (Camera camera)
        {
            Vector3 min = Bounds.min;
            Vector3 max = Bounds.max;

            Vector3 minInViewport = camera.WorldToViewportPoint(min);
            Vector3 maxInViewport = camera.WorldToViewportPoint(max);

            if (minInViewport.x > 1.0f || maxInViewport.x < 0.0f)
            {
                return false;
            }
            return true;
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }
    }
}