using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class SetupDepthCamera : MonoBehaviour 
    {
        public Shader DepthShader;

        public void Start()
        {
            Camera camera = GetComponent<Camera> ();
            camera.depthTextureMode = DepthTextureMode.Depth;
            camera.SetReplacementShader (DepthShader, "");
        }
    }
}
