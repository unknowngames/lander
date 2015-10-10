using UnityEngine;

namespace Assets.Scripts.Common
{
    public class CameraController : MonoBehaviour
    {                          
        [SerializeField]
        private CameraTarget cameraTarget;

        [SerializeField]
        private float targetLerpSpeed = 1.0f;

        [SerializeField]
        private float distanceLerpSpeed = 1.0f;

        [SerializeField]
        private float heightOffset = 0.0f;
        [SerializeField]
        private float heightDampingSpeed = 1.0f;

        [SerializeField]
        private float unZoomDistanceToTarget = 150.0f;  
        [SerializeField]
        private float zoomDistanceToTarget = 50.0f;

        private float targetLerpProgress = 0.0f;
        private float lerpFromDistance;
        private float currentDistance;
        private Vector3 realTarget;
        private Vector3 lerpFromTarget;

        private float fixedDeltaTime;

        public bool HasTarget
        {
            get { return cameraTarget != null; }
        }

        public CameraTarget TargetTransform
        {
            get { return cameraTarget; }
        }

        public void SetTarget(CameraTarget newTarget, bool isDirect = false)
        {
            cameraTarget = newTarget;
            lerpFromTarget = realTarget;

            targetLerpProgress = isDirect ? 1.0f : 0.0f;

            MoveCamera(isDirect);
        }

        public void FixedUpdate()
        {
            fixedDeltaTime += Time.fixedDeltaTime;
        }

        public void LateUpdate()
        {
            if (HasTarget)
            {
                MoveCamera();
            }

            fixedDeltaTime = 0;
        }

        private void MoveCamera(bool isDirect = false)
        {
            Vector3 lerpedTarget = GetLerpedTarget();
            float height = GetLerpedHeight(lerpedTarget, isDirect);
            float distance = GetLerpedDistance(isDirect);
            LookAt(lerpedTarget, height, distance);
        }

        private float GetLerpedDistance(bool isDirect = false)
        {
            float distance = cameraTarget.DoZoom ? zoomDistanceToTarget : unZoomDistanceToTarget;

            currentDistance = isDirect ? distance : Mathf.Lerp(currentDistance, distance, distanceLerpSpeed * fixedDeltaTime);

            return currentDistance;
        }

        private Vector3 GetLerpedTarget()
        {
            realTarget = Vector3.Lerp(lerpFromTarget, cameraTarget.Position, targetLerpProgress);

            if (targetLerpProgress < 1.0f)
            {
                targetLerpProgress += targetLerpProgress * Time.deltaTime;
            }

            return realTarget;
        }

        private float GetLerpedHeight(Vector3 lookAtPoint, bool isDirect = false)
        {
            float wantedHeight = lookAtPoint.y + heightOffset;

            float currentHeight = transform.position.y;

            currentHeight = isDirect ? wantedHeight : Mathf.Lerp(currentHeight, wantedHeight, heightDampingSpeed*fixedDeltaTime);

            return currentHeight;
        }
        
        private void LookAt(Vector3 lookAtPoint, float height, float distance)
        {                
            Vector3 position = new Vector3(lookAtPoint.x, height, lookAtPoint.z);
            position -= Vector3.forward * distance;

            transform.position = position;

            transform.LookAt(lookAtPoint);
        }
    }
}