using UnityEngine;

namespace Assets.Scripts
{
    public enum ERelativePosition
    {
        InViewport,
        Left,
        Right
    }

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

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }


        public ERelativePosition GetRelativePosition(Camera camera)
        {
            Vector3 min = Bounds.min;
            Vector3 max = Bounds.max;

            Vector3 minInViewport = camera.WorldToViewportPoint(min);
            Vector3 maxInViewport = camera.WorldToViewportPoint(max);

            if (minInViewport.x > 1.0f)
            {
                return ERelativePosition.Right;
            }
               
            if (maxInViewport.x < 0.0f)
            {
                return ERelativePosition.Left;
            }

            return ERelativePosition.InViewport;
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }
    }
}