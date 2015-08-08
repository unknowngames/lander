using UnityEngine;

//[ExecuteInEditMode]
namespace Assets.Scripts.Effects
{
    public class ShadowMapReceiver : MonoBehaviour 
    {
        public Camera ShadowProjector;
        public Texture ShadowMap;

        private Material[] mats;

        public void Start()
        {
            if (ShadowProjector == null)
            {
                TryFindShadowProjector ();
            }

            Renderer renderer = GetComponent<Renderer> ();

            mats = renderer.materials;

            foreach (Material m in mats) 
            {
                if(m.HasProperty("_ShadowMap"))
                {
                    m.SetTexture("_ShadowMap", ShadowMap);
                }
            }
        }

        private void TryFindShadowProjector()
        {
            GameObject go = GameObject.FindGameObjectWithTag ("ShadowMapCamera");

            if (go == null)
            {
                return;
            }

            ShadowProjector = go.GetComponent<Camera>();
        }

        public void LateUpdate()
        {
            if (ShadowProjector == null)
            {
                TryFindShadowProjector ();
            }

            if (ShadowProjector == null || mats == null || mats.Length == 0)
            {
                return;
            }

            Matrix4x4 pv = ShadowProjector.projectionMatrix * ShadowProjector.worldToCameraMatrix;

            foreach (Material m in mats) 
            {
                m.SetMatrix ("_ProjectionMatrix", pv);
            }
        }
    }
}
