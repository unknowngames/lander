using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.LandGenerator
{
    public class Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public int index;

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

        public static MeshFilter GeneratePlaneMesh(int width, int length, float cellSize, float height, out MeshData meshData)
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

            meshData = new MeshData();
            meshData.vertices = new Vertex[verts.Length];
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
                        verts[index] = v;
                        var n = new Vector3(0, 1, 0);
                        normals[index] = n;

                        uv[index].x = (float)j / (float)width;
                        uv[index].y = (float)(i - 2) / (float)(length);

                        vtx = new Vertex(v,n, index);
                        meshData.vertices[index] = vtx;
                    }
                    else if (i == 1)
                    {
                        // генерируем вершины, нормали и текстурные координаты для внутренней поверхности
                        var v = new Vector3(j * cellSize - halfWidth, 0, (length - (i - 1)) * cellSize - halfLength);
                        verts[index] = v;
                        var n = new Vector3(0, 1, 0);
                        normals[index] = n;
                        uv[index].x = 1.0f - ((float)j / (float)width);
                        uv[index].y = 1; // по нормальному должен быть 0, скорее всего косяк в шейдере для почвы

                        vtx = new Vertex(v, n, index);
                        meshData.vertices[index] = vtx;
                    }
                    else if (i == 0)
                    {
                        // генерируем вершины, нормали и текстурные координаты для внутренней поверхности
                        var v = new Vector3(j * cellSize - halfWidth, -height, (length) * cellSize - halfLength);
                        verts[index] = v;
                        var n = new Vector3(0, 1, 0);
                        normals[index] = n;
                        uv[index].x = 1.0f - ((float)j / (float)width);
                        uv[index].y = 0; // по нормальному должен быть 1, скорее всего косяк в шейдере для почвы

                        vtx = new Vertex(v, n, index);
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

            mesh.subMeshCount = 2;
            mesh.vertices = verts;
            mesh.normals = normals;
            mesh.SetIndices(mainSurfIndices, MeshTopology.Triangles, 0);
            mesh.SetIndices(innerSurfIndices, MeshTopology.Triangles, 1);

            mesh.uv = uv;

            return mf;
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


        public static void ApplySinWave(ref Vector3[] vertices, ref Vector3[] normals, Vector2 direction, float frequency, float amplitude, float stepness, bool calcNormal)
        {
            int len = vertices.Length;
            for (int i = 0; i < len; i++)
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

        public static Mesh CreateCollisionMesh(MeshData source, float collZ, float collExtent)
        {
            Vector2 pnt = new Vector2(0, collZ);
            var middleNode = source.QuadTreeRootNode.GetNodeNearPoint(ref pnt);
            if(middleNode == null)
            {
                return null;
            }
            var middleVertex = middleNode.Objs[0] as Vertex;

            Mesh m = new Mesh();
            m.name = "Generated collision";

            
            //var vertices = source.vertices;
            //var normals = source.normals;
            //var indices = source.GetIndices(0);;

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
            

            var newVerts = new Vector3[newVertices.Count];
            var newNormals = new Vector3[newVertices.Count];

            for (int i=0; i<newVertices.Count; i++)
            {
                var v = newVertices[i];
                var newV = new Vertex(v.position, v.normal, i);
                newVerts[i] = v.position;
                newNormals[i] = v.normal;
                newVertices[i] = newV;
            }

            /*for (int i = 0; i < vertices.Length; i++)
            {
                var v = vertices[i];

                if (v.z >= collZ - collExtent && v.z <= collZ + collExtent)
                {
                    newVertIndices.Add(i);
                }
            }*/

            

            m.vertices = newVerts;
            m.normals = newNormals;
            m.SetIndices(newVertIndices.ToArray(), MeshTopology.Triangles, 0);
            
            return m;

    
        }
    }
}

