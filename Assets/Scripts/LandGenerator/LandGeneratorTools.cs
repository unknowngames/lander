using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.LandGenerator
{
    public static class LandGeneratorTools
    {
        public static MeshFilter GeneratePlaneMesh(int width, int length, float cellSize, float height)
        {
            var go = new GameObject("Generated land");

            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = new Material(Shader.Find("Diffuse"));

            Mesh mesh = new Mesh();
            mesh.name = "generated land";
            mf.sharedMesh = mesh;

            float halfWidth = width * cellSize / 2.0f;
            float halfLength = length * cellSize / 2.0f;

            Vector3[] verts = new Vector3[(width + 3) * (length + 1)];
            Vector3[] normals = new Vector3[verts.Length];
            Vector2[] uv = new Vector2[verts.Length];

            int[] mainSurfIndices = new int[width * length * 6];


            for (int i = 0; i <= length + 2; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    int index = i * (width + 1) + j;

                    if (i > 1)
                    {
                        // генерируем вершины, нормали и текстурные координаты для основной поверхности
                        verts[index] = new Vector3(j * cellSize - halfWidth, 0, (length - (i - 2)) * cellSize - halfLength);
                        normals[index] = new Vector3(0, 1, 0);
                        uv[index].x = (float)j / (float)width;
                        uv[index].y = (float)(i - 2) / (float)(length);
                    }
                    else if (i == 1)
                    {
                        // генерируем вершины, нормали и текстурные координаты для внутренней поверхности
                        verts[index] = new Vector3(j * cellSize - halfWidth, 0, (length - (i - 1)) * cellSize - halfLength);
                        normals[index] = new Vector3(0, 0, 1);
                        uv[index].x = 1.0f - ((float)j / (float)width);
                        uv[index].y = 1; // по нормальному должен быть 0, скорее всего косяк в шейдере для почвы
                    }
                    else if (i == 0)
                    {
                        // генерируем вершины, нормали и текстурные координаты для внутренней поверхности
                        verts[index] = new Vector3(j * cellSize - halfWidth, -height, (length) * cellSize - halfLength);
                        normals[index] = new Vector3(0, 0, 1);
                        uv[index].x = 1.0f - ((float)j / (float)width);
                        uv[index].y = 0; // по нормальному должен быть 1, скорее всего косяк в шейдере для почвы
                    }
                }
            }

            // генерируем индексы для основной поверхности
            int currentIndex = 0;
            for (int i = 2; i < length + 2; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int x0 = i * (width + 1) + j;
                    int x1 = x0 + 1;
                    int y0 = (i + 1) * (width + 1) + j;
                    int y1 = y0 + 1;

                    mainSurfIndices[currentIndex] = x0;
                    mainSurfIndices[currentIndex + 1] = x1;
                    mainSurfIndices[currentIndex + 2] = y0;
                    mainSurfIndices[currentIndex + 3] = y0;
                    mainSurfIndices[currentIndex + 4] = x1;
                    mainSurfIndices[currentIndex + 5] = y1;

                    currentIndex += 6;
                }
            }

            currentIndex = 0;
            var innerSurfIndices = new int[(width) * 6 * 2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int x0 = i * (width + 1) + j;
                    int x1 = x0 + 1;
                    int y0 = (i + 1) * (width + 1) + j;
                    int y1 = y0 + 1;

                    innerSurfIndices[currentIndex] = x0;
                    innerSurfIndices[currentIndex + 1] = x1;
                    innerSurfIndices[currentIndex + 2] = y0;
                    innerSurfIndices[currentIndex + 3] = y0;
                    innerSurfIndices[currentIndex + 4] = x1;
                    innerSurfIndices[currentIndex + 5] = y1;

                    currentIndex += 6;
                }
            }

            mesh.subMeshCount = 2;
            mesh.vertices = verts;
            mesh.normals = normals;
            mesh.SetIndices(mainSurfIndices, MeshTopology.Triangles, 0);
            mesh.SetIndices(innerSurfIndices, MeshTopology.Triangles, 1);

            mesh.uv = uv;

            return mf;
        }


        public static void ApplySinWave(ref Vector3[] vertices, ref Vector3[] normals, Vector2 direction, float frequency, float amplitude, float stepness, bool calcNormal)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                var v = vertices[i];

                float s = (direction.x * v.x + direction.y * v.z) * frequency;

                float height = Mathf.Pow(((Mathf.Sin(s) + 1.0f) / 2.0f), stepness) * amplitude;
                v.y += height;
                vertices[i] = v;

                if (calcNormal)
                {
                    float dZ = direction.y * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
                    Vector3 binormal = new Vector3(0, dZ, 1).normalized;

                    float dX = direction.x * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
                    Vector3 tangent = new Vector3(1, dX, 0).normalized;

                    normals[i] = (Vector3.Cross(binormal, tangent).normalized);
                }
            }
        }

        public static void RecalculateNormals(Mesh mesh)
        {
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var indices = mesh.GetIndices(0);

            Dictionary<int, List<Vector3>> neighbourMap = new Dictionary<int, List<Vector3>>();
            //Vector3[] faceNormals = new Vector3[indices.Length / 3];

            for (int i = 0; i < normals.Length; i++)
            {
                neighbourMap[i] = new List<Vector3>();
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                var i1 = indices[i];
                var i2 = indices[i + 1];
                var i3 = indices[i + 2];

                var a = vertices[i2] - vertices[i1];
                var b = vertices[i3] - vertices[i2];

                //a.Normalize();
                //b.Normalize();

                var c = Vector3.Cross(a, b);
                c.Normalize();

                neighbourMap[i1].Add(c);
                neighbourMap[i2].Add(c);
                neighbourMap[i3].Add(c);
            }

            for (int i = 0; i < normals.Length; i++)
            {
                var n = normals[i];
                n.x = n.y = n.z = 0;

                var nbrs = neighbourMap[i];

                for (int j = 0; j < nbrs.Count; j++)
                {
                    n += nbrs[j];
                }

                n.Normalize();
                normals[i] = n;
            }

            mesh.normals = normals;
        }

        public static Mesh CreateCollisionMesh(Mesh source, float collZ, float collExtent)
        {
            var vertices = source.vertices;
            var normals = source.normals;
            var indices = source.GetIndices(0);

            Mesh m = new Mesh();
            m.name = "Generated collision";

            List<int> newVertIndices = new List<int>();

            for (int i = 0; i < vertices.Length; i++)
            {
                var v = vertices[i];

                if (v.z >= collZ - collExtent && v.z <= collZ + collExtent)
                {
                    newVertIndices.Add(i);
                }
            }

            var newVertices = new Vector3[newVertIndices.Count];
            var newNormals = new Vector3[newVertices.Length];
            var newIndices = new List<int>();

            Dictionary<int, int> indexMapping = new Dictionary<int, int>();

            for (int i = 0; i < newVertices.Length; i++)
            {
                var ind = newVertIndices[i];
                newVertices[i] = vertices[ind];
                newNormals[i] = normals[ind];
                indexMapping.Add(ind, i);
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                var i1 = indices[i];
                var i2 = indices[i + 1];
                var i3 = indices[i + 2];

                if (newVertIndices.Contains(i1) && newVertIndices.Contains(i2) && newVertIndices.Contains(i3))
                {
                    newIndices.Add(indexMapping[i1]);
                    newIndices.Add(indexMapping[i2]);
                    newIndices.Add(indexMapping[i3]);
                }
            }

            m.vertices = newVertices;
            m.normals = newNormals;
            m.SetIndices(newIndices.ToArray(), MeshTopology.Triangles, 0);

            return m;
        }
    }
}

