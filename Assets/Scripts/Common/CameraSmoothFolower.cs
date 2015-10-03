using UnityEngine;

namespace Assets.Scripts.Common
{
    public class CameraSmoothFolower : MonoBehaviour
    {
        public Transform Target;

        public bool ScaleTarget = false;
        public float FarScale = 2;
        private float currentScale = 1;

        [SerializeField]
        private float distance = 750;

        [SerializeField]
        private float zoomViewDistance = 50;

        [SerializeField]
        private float zoomStartDistance = 50;

        [SerializeField]
        private float zoomSpeed = 10;

        private float currentZoomDistance = 0.0f;
        private float targetZoomDistance = 0.0f;

        [SerializeField]
        private string zoomObjectTag = "ZoomTarget";

        [SerializeField]
        private float heightDamping;
        [SerializeField]
        private float rotationDamping;
        [SerializeField]
        private float height;

        private float FixedDeltaTime;

        private bool isZoomed = false;

        GameObject[] zoomTargets;

#if UNITY_EDITOR
        public bool debugViewDistance = false;
#endif

        public void DoZoom()
        {
            isZoomed = true;
        }
        public void UndoZoom()
        {
            isZoomed = false;
        }


        public void Start()
        {
            zoomTargets = GameObject.FindGameObjectsWithTag(zoomObjectTag);

            if (zoomTargets != null)
                Debug.Log("Zoom targets found: " + zoomTargets.Length);
            else
                Debug.Log("No zoom targets");

            targetZoomDistance = distance;
            currentZoomDistance = distance;
        }


        public void Update ()
        {
            if (Target == null)
            {
                if (PlayerSpawner.PlayerSpaceship != null)
                {
                    Target = PlayerSpawner.PlayerSpaceship.transform;
                }
            }
            if (Target == null)
            {
                return;
            }

            if(zoomTargets != null)
            {
                Transform closestTarget = null;
                float closestDistance = float.MaxValue;

                for (int i=0; i<zoomTargets.Length; i++)
                {
                    GameObject zoomTarget = zoomTargets[i];

                    float dist = Vector3.Distance(Target.position, zoomTarget.transform.position);

                    if(dist <= zoomStartDistance && dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestTarget = zoomTarget.transform;
                    }
                }

                if(closestTarget != null)
                {
                    targetZoomDistance = zoomViewDistance;
                }
                else
                {
                    targetZoomDistance = distance;
                }

#if UNITY_EDITOR
                if (debugViewDistance || isZoomed)
                {
                    targetZoomDistance = zoomViewDistance;
                }
#endif

                currentZoomDistance = Mathf.Lerp(currentZoomDistance, targetZoomDistance, Time.deltaTime * zoomSpeed);
            }

            if (ScaleTarget)
            {
                float normalizedDistance = (currentZoomDistance - zoomViewDistance) / (distance - zoomViewDistance);
                float scale = Mathf.Lerp(1.0f, FarScale, normalizedDistance);    

                Target.transform.localScale = new Vector3(scale,scale,scale);
            }   
        }

        public void FixedUpdate ()
        {                          
            FixedDeltaTime += Time.fixedDeltaTime;
        }

        public void LateUpdate()
        {                                  
            MoveCamera ();
        }

        private void MoveCamera ()
        {
            if (Target != null)
            {
                // Calculate the current rotation angles
                float wantedRotationAngle = Target.eulerAngles.y;
                float wantedHeight = Target.position.y + height;

                float currentRotationAngle = transform.eulerAngles.y;
                float currentHeight = transform.position.y;

                // Damp the rotation around the y-axis
                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * FixedDeltaTime);

                // Damp the height
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * FixedDeltaTime);

                // Convert the angle into a rotation
                Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

                // Set the position of the camera on the x-z plane to:
                // distance meters behind the target
                Vector3 position = Target.position;
                position -= currentRotation * Vector3.forward * currentZoomDistance;

                // Set the height of the camera
                transform.position = new Vector3(position.x, currentHeight, position.z);

                // Always look at the target
                transform.LookAt (Target);
            }
            FixedDeltaTime = 0;
        }
    }
}
