using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.LandGenerator
{
    public class Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public int index;
        public Vector2 uv;

        public Vertex Top;
        public Vertex Bottom;
        public Vertex Left;
        public Vertex Right;

        public Vertex(Vector3 pos, Vector3 norm, int ind)
        {
            position = pos;
            normal = norm;
            index = ind;
        }
    }

    public class MeshData
    {
        public Vertex[] vertices;
        public QuadTreeNode QuadTreeRootNode;

        Dictionary<int, int[]> indexMap = new Dictionary<int, int[]>();

        public int SubMeshCount
        {
            get
            {
                return indexMap.Count;
            }
        }

        public void SetIndices(int submeshId, int[] indices)
        {
            indexMap[submeshId] = indices;
        }

        public int[] GetIndices(int submeshId)
        {
            return indexMap[submeshId];
        }
    }

    public static class LandGeneratorTools
    {
        /// <summary>
        /// выравнивает все вершины плейна, которые больше заданный глубины
        /// </summary>
        /// <param name="planeWIdth"></param>
        /// <param name="planeHeight"></param>
        /// <param name="depth"></param>
        public static void FlattenPlaneMeshByDepthZ(ref Vector3[] vertices, int planeWidth, int planeLength, float depth)
        {
            var vl = vertices.Length;
            for(int i=vl-1; i>= 0; i--)
            {
                if (i < planeWidth)
                    continue;

                var v = vertices[i];

                if (v.z >= depth)
                {
                    v.y = vertices[i + planeWidth].y;
                    vertices[i] = v;
                }
            }
        }

        public static MeshFilter CreatePlaneMeshObject(string name)
        {
            var go = new GameObject(name);

            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = new Material(Shader.Find("Diffuse"));

            return mf;


            //mesh.subMeshCount = 2;
            //mesh.vertices = verts;
            //mesh.normals = normals;
            //mesh.SetIndices(mainSurfIndices, MeshTopology.Triangles, 0);
            //mesh.SetIndices(innerSurfIndices, MeshTopology.Triangles, 1);
            //mesh.uv = uv;
        }

        public static void GeneratePlaneMesh(int width, int length, float cellSize, float height, out MeshData meshData)
        {
            float halfWidth = width * cellSize / 2.0f;
            float halfLength = length * cellSize / 2.0f;

            meshData = new MeshData();
            meshData.vertices = new Vertex[(width + 3) * (length + 1)];
            meshData.QuadTreeRootNode = new QuadTreeNode(Vector2.zero, (float)Mathf.Max(width, length) * cellSize);
            meshData.QuadTreeRootNode.SubdivideRecursively(cellSize);

            int[] mainSurfIndices = new int[width * length * 6];

            Vector2 tempVect = Vector2.zero;


            for (int i = 0; i <= length + 2; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    int index = i * (width + 1) + j;

                    Vertex vtx = null;

                    if (i > 1)
                    {
                        // генерируем вершины, нормали и текстурные координаты для основной поверхности
                        var v = new Vector3(j * cellSize - halfWidth, 0, (length - (i - 2)) * cellSize - halfLength);
                        var n = new Vector3(0, 1, 0);
                        
                        vtx = new Vertex(v,n, index);
                        vtx.uv.x = (float)j / (float)width;
                        vtx.uv.y = (float)(i - 2) / (float)(length);
                        meshData.vertices[index] = vtx;
                    }
                    else if (i == 1)
                    {
                        // генерируем вершины, нормали и текстурные координаты для внутренней поверхности
                        var v = new Vector3(j * cellSize - halfWidth, 0, (length - (i - 1)) * cellSize - halfLength);
                        var n = new Vector3(0, 1, 0);

                        vtx = new Vertex(v, n, index);

                        vtx.uv.x = 1.0f - ((float)j / (float)width);
                        vtx.uv.y = 1; // по нормальному должен быть 0, скорее всего косяк в шейдере для почвы

                        meshData.vertices[index] = vtx;
                    }
                    else if (i == 0)
                    {
                        // генерируем вершины, нормали и текстурные координаты для внутренней поверхности
                        var v = new Vector3(j * cellSize - halfWidth, -height, (length) * cellSize - halfLength);
                        var n = new Vector3(0, 1, 0);
                        

                        vtx = new Vertex(v, n, index);
                        vtx.uv.x = 1.0f - ((float)j / (float)width);
                        vtx.uv.y = 0; // по нормальному должен быть 1, скорее всего косяк в шейдере для почвы

                        meshData.vertices[index] = vtx;
                    }
                }
            }

            // add neighbours
            for(int i=0; i<meshData.vertices.Length; i++)
            {
                var vtx = meshData.vertices[i];

                addNeighbours(vtx, width+1, ref meshData.vertices);
                tempVect.x = vtx.position.x;
                tempVect.y = vtx.position.z;
                var node = meshData.QuadTreeRootNode.GetNodeNearPoint(ref tempVect);

                if (node != null)
                {
                    if (node.Objs == null)
                        node.Objs = new List<object>();
                    node.Objs.Add(vtx);
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

            
            meshData.SetIndices(0, mainSurfIndices);
            meshData.SetIndices(1, innerSurfIndices);
        }

        private static void addNeighbours(Vertex v, int width, ref Vertex[] verts)
        {
            int index = v.index;
            int len = verts.Length;

            // top
            if(index > width)
            {
                v.Top = verts[index - width];
            }

            // bottom
            int indW = index + width;
            if(indW < len)
            {
                v.Bottom = verts[indW];
            }

            // left
            if(index > 0 && (index % width > 0))
            {
                v.Left = verts[index - 1];
            }

            // right
            int indInc = index + 1;
            if((index % width != width-1) && indInc < len)
            {
                v.Right = verts[indInc];
            }
        }


        public static void ApplySinWave(MeshData meshData, Vector2 direction, float frequency, float amplitude, float stepness, bool calcNormal)
        {
            int len = meshData.vertices.Length;
            for (int i = 0; i < len; i++)
            {
                var v = meshData.vertices[i];

                float s = (direction.x * v.position.x + direction.y * v.position.z) * frequency;

                float height = Mathf.Pow(((Mathf.Sin(s) + 1.0f) / 2.0f), stepness) * amplitude;
                v.position.y += height;
                meshData.vertices[i] = v;

                if (calcNormal)
                {
                    float dZ = direction.y * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
                    Vector3 binormal = new Vector3(0, dZ, 1).normalized;

                    float dX = direction.x * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
                    Vector3 tangent = new Vector3(1, dX, 0).normalized;

                    v.normal = (Vector3.Cross(binormal, tangent).normalized);
                }
            }
        }

        public static void RecalculateNormals(MeshData mesh)
        {
            //var vertices = mesh.vertices;
            //var normals = mesh.normals;
            //var indices = mesh.GetIndices(0);

            Dictionary<int, List<Vector3>> neighbourMap = new Dictionary<int, List<Vector3>>();
            //Vector3[] faceNormals = new Vector3[indices.Length / 3];

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                neighbourMap[i] = new List<Vector3>();
            }

            var indices = mesh.GetIndices(0);

            for (int i = 0; i < indices.Length; i += 3)
            {
                var i1 = indices[i];
                var i2 = indices[i + 1];
                var i3 = indices[i + 2];

                var a = mesh.vertices[i2].position - mesh.vertices[i1].position;
                var b = mesh.vertices[i3].position - mesh.vertices[i2].position;

                //a.Normalize();
                //b.Normalize();

                var c = Vector3.Cross(a, b);
                c.Normalize();

                neighbourMap[i1].Add(c);
                neighbourMap[i2].Add(c);
                neighbourMap[i3].Add(c);
            }

            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                var v = mesh.vertices[i];

                v.normal.x = v.normal.y = v.normal.z = 0;

                var nbrs = neighbourMap[i];

                for (int j = 0; j < nbrs.Count; j++)
                {
                    v.normal += nbrs[j];
                }

                v.normal.Normalize();
            }
        }

        public static Mesh MeshDataToUnityMesh(MeshData source)
        {
            var newVerts = new Vector3[source.vertices.Length];
            var newNormals = new Vector3[source.vertices.Length];
            var newUv = new Vector2[source.vertices.Length];

            for (int i = 0; i < source.vertices.Length; i++)
            {
                var v = source.vertices[i];
                var newV = new Vertex(v.position, v.normal, i);
                newVerts[i] = v.position;
                newNormals[i] = v.normal;
                newUv[i] = v.uv;
            }

            Mesh result = new Mesh();
            result.name = "Generated collision";
            result.vertices = newVerts;
            result.normals = newNormals;
            result.uv = newUv;
            result.subMeshCount = source.SubMeshCount;

            for(int i=0; i<source.SubMeshCount; i++)
            {
                result.SetIndices(source.GetIndices(i), MeshTopology.Triangles, i);
            }
            
            return result;
        }

        private static Vertex getStartVertexZ(MeshData mesh, float z)
        {
            Vector2 pnt = new Vector2(0, z);
            var n = mesh.QuadTreeRootNode.GetNodeNearPoint(ref pnt);
            return n.Objs[0] as Vertex;
        }

        public static void PrepareCollisionPlace(MeshData source, float collZ, float collExtent)
        {
            var middleVertex = getStartVertexZ(source, collZ);


            List<int> newVertIndices = new List<int>();
            List<Vertex> newVertices = new List<Vertex>();

            int currentIndex = 0;
            Vertex nextVertex = middleVertex;
            while (nextVertex != null)
            {
                if (nextVertex.Right == null)
                    break;

                nextVertex.Bottom.position.y = nextVertex.position.y;
                nextVertex.Top.position.y = nextVertex.position.y;

                var nextTop = nextVertex;
                while(nextTop.Top != null)
                {
                    // игнорим вертикальные
                    if (nextTop.Bottom.position.x == nextTop.position.x && nextTop.Bottom.position.z == nextTop.position.z)
                    {
                        break;
                    }

                    nextTop.Top.position.y = nextTop.position.y;
                    nextTop = nextTop.Top;
                }
                
                nextVertex = nextVertex.Right;
                currentIndex += 6;
            }


            nextVertex = middleVertex;
            while (nextVertex != null)
            {
                if (nextVertex.Left == null)
                    break;

                nextVertex.Bottom.position.y = nextVertex.position.y;
                nextVertex.Top.position.y = nextVertex.position.y;

                var nextTop = nextVertex;
                while (nextTop.Top != null)
                {
                    // игнорим вертикальные
                    if (nextTop.Bottom.position.x == nextTop.position.x && nextTop.Bottom.position.z == nextTop.position.z)
                    {
                        break;
                    }

                    nextTop.Top.position.y = nextTop.position.y;
                    nextTop = nextTop.Top;
                }

                nextVertex = nextVertex.Left;
                currentIndex += 6;
            }
        }

        public static void CreateCollisionMesh(MeshData source, float collZ, float collExtent, out MeshData result)
        {
            result = null;
            var middleVertex = getStartVertexZ(source, collZ);


            List<int> newVertIndices = new List<int>();
            List<Vertex> newVertices = new List<Vertex>();

            int currentIndex = 0;
            Vertex nextVertex = middleVertex;
            while(nextVertex != null)
            {
                if (nextVertex.Right == null)
                    break;

                newVertices.Add(nextVertex);
                newVertices.Add(nextVertex.Right);
                newVertices.Add(nextVertex.Top);
                newVertices.Add(nextVertex.Top.Right);
                newVertices.Add(nextVertex.Bottom);
                newVertices.Add(nextVertex.Bottom.Right);


                newVertIndices.Add(currentIndex);
                newVertIndices.Add(currentIndex + 2);
                newVertIndices.Add(currentIndex + 1);

                newVertIndices.Add(currentIndex + 2);
                newVertIndices.Add(currentIndex + 3);
                newVertIndices.Add(currentIndex + 1);


                newVertIndices.Add(currentIndex);
                newVertIndices.Add(currentIndex + 1);
                newVertIndices.Add(currentIndex + 4);

                newVertIndices.Add(currentIndex + 4);
                newVertIndices.Add(currentIndex + 1);
                newVertIndices.Add(currentIndex + 5);   

                nextVertex = nextVertex.Right;
                currentIndex += 6;
            }
             
            
            nextVertex = middleVertex;
            while (nextVertex != null)
            {
                if (nextVertex.Left == null)
                    break;

                newVertices.Add(nextVertex);
                newVertices.Add(nextVertex.Left);
                newVertices.Add(nextVertex.Top);
                newVertices.Add(nextVertex.Top.Left);
                newVertices.Add(nextVertex.Bottom);
                newVertices.Add(nextVertex.Bottom.Left);


                newVertIndices.Add(currentIndex);
                newVertIndices.Add(currentIndex + 1);
                newVertIndices.Add(currentIndex + 2);

                newVertIndices.Add(currentIndex + 2);
                newVertIndices.Add(currentIndex + 1);
                newVertIndices.Add(currentIndex + 3);

                newVertIndices.Add(currentIndex);
                newVertIndices.Add(currentIndex + 4);
                newVertIndices.Add(currentIndex + 1);
                
                newVertIndices.Add(currentIndex + 4);
                newVertIndices.Add(currentIndex + 5);
                newVertIndices.Add(currentIndex + 1);
                

                nextVertex = nextVertex.Left;
                currentIndex += 6;
            }

            result = new MeshData();
            result.vertices = newVertices.ToArray();
            result.SetIndices(0, newVertIndices.ToArray());
        }
    }
}

