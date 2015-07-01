using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraSmoothFolower : MonoBehaviour
    {
        public Transform Target;
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

        GameObject[] zoomTargets;


        void Start()
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
            if (Game.PlayerSpaceship != null)
            {
                Target = Game.PlayerSpaceship.transform;
            }

            if(Target != null && zoomTargets != null)
            {
                Transform closestTarget = null;
                float closestDistance = float.MaxValue;

                for (int i=0; i<zoomTargets.Length; i++)
                {
                    var zoomTarget = zoomTargets[i];

                    var dist = Vector3.Distance(Target.position, zoomTarget.transform.position);

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

                currentZoomDistance = Mathf.Lerp(currentZoomDistance, targetZoomDistance, Time.deltaTime * zoomSpeed);
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
                var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

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
