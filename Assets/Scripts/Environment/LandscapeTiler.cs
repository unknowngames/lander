using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class LandscapeTiler : MonoBehaviour
    {
        [SerializeField]
        private Landscape landscapeTemplate;

        [Range(2,10)]
        [SerializeField]
        private int partitionCount=3;

        private readonly LandscapeArray landscapes = new LandscapeArray();

        private Camera mainCamera;

        public void Awake ()
        {
            mainCamera = Camera.main;
        }

        public void Start ()
        {
            Init ();
        }

        private void Init ()
        {
            Vector3 position = landscapeTemplate.Position;
            landscapes.PushLeft (landscapeTemplate);

            int il = 1;
            int ir = 1;

            for (int i = 0; i < partitionCount - 1; i++)
            {
                Landscape copy = Instantiate (landscapeTemplate);
                copy.transform.parent = transform;

                if (i % 2 == 0)
                {
                    Vector3 newPosition = new Vector3(position.x - landscapeTemplate.Bounds.size.x * il, position.y, position.z);
                    copy.Position = newPosition;

                    landscapes.PushLeft(copy);
                    il++;
                }
                else
                {
                    Vector3 newPosition = new Vector3(position.x + landscapeTemplate.Bounds.size.x * ir, position.y, position.z);
                    copy.Position = newPosition;

                    landscapes.PushRight(copy);
                    ir++;
                }
            }
        }

        public void Update ()
        {
            float relativePosition = GetXPositionInViewport (landscapes.Center, mainCamera);

            if (relativePosition <= 0.0f)
            {
                landscapes.TileToRight();
            }
            else if (relativePosition >= 1.0f)
            {
                landscapes.TileToLeft ();
            }
        }

        private float GetXPositionInViewport (Vector3 position, Camera camera)
        {
            Vector3 positionInViewport = camera.WorldToViewportPoint (position);
            return positionInViewport.x;
        }

        public void OnDrawGizmos ()
        {
            Color color = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube (landscapes.Bounds.center, landscapes.Bounds.size);
            Gizmos.color = color;
        }
    }
}

