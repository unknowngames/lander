using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CameraTarget))]
    public class ObjectHighlight : HighlightedEntity
    {
        [SerializeField]
        private CameraController cameraController;

        private CameraTarget cachedTarget;

        public override void Do()
        {
            cachedTarget = cameraController.TargetTransform;
            cameraController.SetTarget(GetComponent<CameraTarget>());
        }

        public override void Stop()
        {
            cameraController.SetTarget(cachedTarget);
        }
    }
}