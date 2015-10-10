using UnityEngine;

namespace Assets.Scripts.Common
{
    public abstract class CameraTarget : MonoBehaviour
    {
        public Vector3 Position
        {
            get { return transform.position; }
        }

        public abstract bool DoZoom { get; }
    }
}