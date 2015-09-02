using UnityEngine;
using System.Collections;

namespace Assets.Scripts.LandGenerator
{
    [System.Serializable]
    class SinInfo
    {
        public int IterationCount = 1;

        public float MinAmplitude = 1;
        public float MaxAmplitude = 2;

        public float MinFrequency = 1;
        public float MaxFrequency = 2;

        public float MinStepness = 1;
        public float MaxStepness = 2;
    }

    public class SimpleLandGenerator : MonoBehaviour
    {
        public int Width = 100;
        public int Length = 100;
        public float Height = 10;
        public float CellSize = 1;

        public Material MainSurface;
        public Material InnerSurface;
        public Texture ShadowMap;

        public float CollisionZ = 0;
        public float CollizionExtent = 1;

        [SerializeField]
        private SinInfo[] sinSettings;

        void Awake()
        {
            MeshData meshData;
            LandGeneratorTools.GeneratePlaneMesh(Width, Length, CellSize, Height, out meshData);

            foreach (var si in sinSettings)
            {
                for(int i=0; i<si.IterationCount; i++)
                {
                    LandGeneratorTools.ApplySinWave(meshData, new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                            Random.Range(si.MinFrequency, si.MaxFrequency),
                            Random.Range(si.MinAmplitude, si.MaxAmplitude),
                            Random.Range(si.MinStepness, si.MaxStepness), false);
                }                
            }
       

            LandGeneratorTools.RecalculateNormals(meshData);

            var mesh = LandGeneratorTools.MeshDataToUnityMesh(meshData);

            var meshFilter = LandGeneratorTools.CreatePlaneMeshObject("Generated land");
            meshFilter.sharedMesh = mesh;

            MeshData collisionMeshData;
            LandGeneratorTools.CreateCollisionMesh(meshData, CollisionZ, CollizionExtent, out collisionMeshData);

            var collMesh = LandGeneratorTools.MeshDataToUnityMesh(collisionMeshData);

            var mc = meshFilter.gameObject.AddComponent<MeshCollider>();
            mc.sharedMesh = collMesh;

            var r =meshFilter.gameObject.GetComponent<Renderer>();
            r.sharedMaterials = new Material[] { MainSurface, InnerSurface };

            r.transform.rotation = transform.rotation;

            var smr = r.gameObject.AddComponent<Effects.ShadowMapReceiver>();
            smr.ShadowMap = ShadowMap;
        }
    }
}
    
