using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Landscape : MonoBehaviour
    {
        [SerializeField]
        private Renderer landscapeRenderer;

        public Bounds Bounds
        {
            get
            {
                if (landscapeRenderer != null)
                {
                    return landscapeRenderer.bounds;
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