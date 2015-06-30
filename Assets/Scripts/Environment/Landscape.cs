using UnityEngine;

namespace Assets.Scripts.Environment
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

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }
    }
}