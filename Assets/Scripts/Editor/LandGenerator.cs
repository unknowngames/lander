﻿using UnityEngine;
using Assets.Scripts.LandGenerator;
using UnityEditor;

namespace Assets.Scripts.Editor
{
    public class LandGenerator : EditorWindow
    {
        int planeWidth = 100;
        int planeLength = 100;
        float planeCellSize = 1.0f;
        float planeHeight = 10;

        Material mainSurfaceMaterial;
        Material innerSurfaceMaterial;
        private Mesh currentMesh = null;
        private MeshData currentMeshData = null;
        bool drawNormals = false;

        bool showSin = false;
        bool calcNormalsAfterSin = true;
        int sinApplyCount = 1;
        float minSinAmplitude = 0.25f;
        float maxSinAmplitude = 1.0f;

        float minSinFrequency = 0.1f;
        float maxSinFrequency = 1.0f;

        float minSinStepness = 1.0f;
        float maxSinStepness = 2.0f;

        bool showCollision = false;
        float collisionZ = 0;
        float collisionExtent = 1;
        const string GeneratedLandObjectName = "Generated land";

        [MenuItem("Unknown games/Land generator")]
        static void show()
        {
            var window = GetWindow<LandGenerator>();
            window.Show();
            SceneView.onSceneGUIDelegate += window.OnSceneGUI;
        }

        void OnGUI()
        {
            planeWidth = EditorGUILayout.IntField("Ширина", planeWidth);
            planeLength = EditorGUILayout.IntField("Длина", planeLength);
            planeHeight = EditorGUILayout.FloatField("Толщина слоя почвы", planeHeight);
            planeCellSize = EditorGUILayout.FloatField("Размер ячейки", planeCellSize);

            mainSurfaceMaterial = (Material)EditorGUILayout.ObjectField("Материал поверхности", mainSurfaceMaterial, typeof(Material), false);
            innerSurfaceMaterial = (Material)EditorGUILayout.ObjectField("Материал почвы", innerSurfaceMaterial, typeof(Material), false);



            if (GUILayout.Button("Сгенерировать"))
            {
                var go = GameObject.Find(GeneratedLandObjectName);
                if (go != null)
                {
                    currentMesh = null;
                    currentMeshData = null;
                    DestroyImmediate(go);
                }

                var mf = LandGeneratorTools.CreatePlaneMeshObject(GeneratedLandObjectName);
                LandGeneratorTools.GeneratePlaneMesh(planeWidth, planeLength, planeCellSize, planeHeight, out currentMeshData);
                mf.sharedMesh = LandGeneratorTools.MeshDataToUnityMesh(currentMeshData);
                currentMesh = mf.sharedMesh;
                var renderer = mf.GetComponent<Renderer>();
                renderer.sharedMaterials = new Material[] { mainSurfaceMaterial, innerSurfaceMaterial };
            }

            if (currentMesh == null)
                return;

            drawNormals = GUILayout.Toggle(drawNormals, "Показать нормали");

            if (GUILayout.Button("Пересчитать нормали"))
            {
                LandGeneratorTools.RecalculateNormals(currentMeshData);
            }

            showCollision = EditorGUILayout.Foldout(showCollision, "Коллизия");
            if (showCollision)
            {
                EditorGUI.indentLevel++;
                if (GUILayout.Button("Создать меш коллизии"))
                {
                    MeshData collMeshData;
                    LandGeneratorTools.CreateCollisionMesh(currentMeshData, collisionZ, collisionExtent, out collMeshData);
                    var mesh = LandGeneratorTools.MeshDataToUnityMesh(collMeshData);

                    var go = GameObject.Find("Generated land");
                    if (go != null)
                    {
                        var mc = go.GetComponent<MeshCollider>();

                        if (mc == null)
                        {
                            mc = go.AddComponent<MeshCollider>();
                        }

                        mc.sharedMesh = mesh;
                    }
                }

                collisionZ = EditorGUILayout.FloatField("Z Глубина коллизии", collisionZ);
                collisionExtent = EditorGUILayout.FloatField("Ширина коллизии", collisionExtent);
                EditorGUI.indentLevel--;

                if(GUILayout.Button("Выровнять меш после Z глубины"))
                {
                    var v = currentMesh.vertices;
                    LandGeneratorTools.FlattenPlaneMeshByDepthZ(ref v, planeWidth, planeLength, collisionZ);
                    currentMesh.vertices = v;
                }
            }



            showSin = EditorGUILayout.Foldout(showSin, "Sin");

            if (showSin)
            {
                if (GUILayout.Button("Применить Sin"))
                {
                    EditorGUI.indentLevel++;
                    var verts = currentMesh.vertices;
                    var normals = currentMesh.normals;

                    for (int i = 0; i < sinApplyCount; i++)
                    {
                        LandGeneratorTools.ApplySinWave(currentMeshData,
                            new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                            Random.Range(minSinFrequency, maxSinFrequency),
                            Random.Range(minSinAmplitude, maxSinAmplitude),
                            Random.Range(minSinStepness, maxSinStepness),
                            false
                        );
                    }

                    if (calcNormalsAfterSin)
                        LandGeneratorTools.RecalculateNormals(currentMeshData);

                    updateMeshFromMeshdata();

                    EditorGUI.indentLevel--;
                }

                sinApplyCount = EditorGUILayout.IntField("Количество итераций Sin", sinApplyCount);
                calcNormalsAfterSin = EditorGUILayout.Toggle("Пересчитывать нормали после применения Sin", calcNormalsAfterSin);
                minSinAmplitude = EditorGUILayout.FloatField("Мин амплитуда Sin", minSinAmplitude);
                maxSinAmplitude = EditorGUILayout.FloatField("Макс амплитуда Sin", maxSinAmplitude);
                minSinFrequency = EditorGUILayout.FloatField("Мин частота Sin", minSinFrequency);
                maxSinFrequency = EditorGUILayout.FloatField("Макс частота Sin", maxSinFrequency);
                minSinStepness = EditorGUILayout.FloatField("Мин крутизна волны Sin", minSinStepness);
                maxSinStepness = EditorGUILayout.FloatField("Макс крутизна Sin", maxSinStepness);
            }

        }


        void updateMeshFromMeshdata()
        {
            if(currentMeshData != null)
            {
                currentMesh = LandGeneratorTools.MeshDataToUnityMesh(currentMeshData);
            }
        }

        public void OnSceneGUI(SceneView scnView)
        {
            if (currentMesh != null && drawNormals)
                drawMeshNormals(currentMesh);
        }

        void drawMeshNormals(Mesh mesh)
        {
            var verts = mesh.vertices;
            var normals = mesh.normals;

            for (int i = 0; i < verts.Length; i++)
            {
                var v = verts[i];
                var n = normals[i];
                Handles.DrawLine(v, v + n * 0.1f);
            }
        }
    }
}