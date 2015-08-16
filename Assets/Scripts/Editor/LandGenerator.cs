using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

class MeshInfo
{
    public Vector3[] Vertices;
    public Vector3[] Normals;
}

public class LandGenerator : EditorWindow
{
    int planeWidth = 100;
    int planeLength = 100;
    float planeCellSize = 1.0f;
    float planeHeight = 10;

    Material mainSurfaceMaterial;
    Material innerSurfaceMaterial;
    private Mesh currentMesh = null;
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
            var go = GameObject.Find("Generated land");
            if(go != null)
            {
                currentMesh = null;
                DestroyImmediate(go);
            }

            var mf = generatePlane(planeWidth, planeLength, planeCellSize, planeHeight);
            currentMesh = mf.sharedMesh;
            var renderer = mf.GetComponent<Renderer>();
            renderer.sharedMaterials = new Material[] { mainSurfaceMaterial, innerSurfaceMaterial };
        }

        if (currentMesh == null)
            return;

        drawNormals = GUILayout.Toggle(drawNormals, "Показать нормали");

        if(GUILayout.Button("Пересчитать нормали"))
        {
            recalcNormals(currentMesh);
        }

        showCollision = EditorGUILayout.Foldout(showCollision, "Коллизия");
        if(showCollision)
        {
            EditorGUI.indentLevel++;
            if (GUILayout.Button("Создать меш коллизии"))
            {
                var mesh = createCollision(currentMesh, collisionZ, collisionExtent);

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
        }

        

        showSin = EditorGUILayout.Foldout(showSin, "Sin");

        if(showSin)
        {
            if (GUILayout.Button("Применить Sin"))
            {
                EditorGUI.indentLevel++;
                var verts = currentMesh.vertices;
                var normals = currentMesh.normals;

                for (int i = 0; i < sinApplyCount; i++)
                {
                    applySinWave(ref verts,
                        ref normals,
                        new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                        Random.Range(minSinFrequency, maxSinFrequency),
                        Random.Range(minSinAmplitude, maxSinAmplitude),
                        Random.Range(minSinStepness, maxSinStepness),
                        false
                    );
                }

                currentMesh.vertices = verts;

                if (calcNormalsAfterSin)
                    recalcNormals(currentMesh);
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

    
    public void OnSceneGUI(SceneView scnView)
    {
        if(currentMesh != null && drawNormals)
            drawMeshNormals(currentMesh);
    }

    void drawMeshNormals(Mesh mesh)
    {
        var verts = mesh.vertices;
        var normals = mesh.normals;

        for(int i=0; i<verts.Length; i++)
        {
            var v = verts[i];
            var n = normals[i];
            Handles.DrawLine(v, v + n*0.1f);
        }
    }

    MeshFilter generatePlane(int width, int length, float cellSize, float height)
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

        
        for(int i=0; i <= length+2; i++)
        {
            for (int j=0; j <= width; j++)
            {
                int index = i * (width+1) + j;

                if(i>1)
                {
                    // генерируем вершины, нормали и текстурные координаты для основной поверхности
                    verts[index] = new Vector3(j * cellSize - halfWidth, 0, (length - (i-2)) * cellSize - halfLength);
                    normals[index] = new Vector3(0, 1, 0);
                    uv[index].x = (float)j / (float)width;
                    uv[index].y = (float)(i-2) / (float)(length);
                }
                else if(i == 1)
                {
                    // генерируем вершины, нормали и текстурные координаты для внутренней поверхности
                    verts[index] = new Vector3(j * cellSize - halfWidth, 0, (length - (i - 1)) * cellSize - halfLength);
                    normals[index] = new Vector3(0, 0, 1);
                    uv[index].x = 1.0f - ((float)j / (float)width);
                    uv[index].y = 1; // по нормальному должен быть 0, скорее всего косяк в шейдере для почвы
                }
                else if(i == 0)
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
        for (int i = 2; i < length+2; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x0 = i * (width+1) + j;
                int x1 = x0+1;
                int y0 = (i+1) * (width+1) + j;
                int y1 = y0 + 1;

                mainSurfIndices[currentIndex] = x0;
                mainSurfIndices[currentIndex+1] = x1;
                mainSurfIndices[currentIndex+2] = y0;
                mainSurfIndices[currentIndex+3] = y0;
                mainSurfIndices[currentIndex+4] = x1;
                mainSurfIndices[currentIndex+5] = y1;

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

    void applySinWave(ref Vector3[] vertices, ref Vector3[] normals, Vector2 direction, float frequency, float amplitude, float stepness, bool calcNormal)
    {
        for(int i=0; i< vertices.Length; i++)
        {
            var v = vertices[i];

            float s = (direction.x * v.x + direction.y * v.z) * frequency;

            float height = Mathf.Pow(((Mathf.Sin(s) + 1.0f) / 2.0f), stepness) * amplitude;
            v.y += height;
            vertices[i] = v;

            if(calcNormal)
            {
                float dZ = direction.y * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
                Vector3 binormal = new Vector3(0, dZ, 1).normalized;

                float dX = direction.x * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
                Vector3 tangent = new Vector3(1, dX, 0).normalized;

                normals[i] = (Vector3.Cross(binormal, tangent).normalized);
            }
        }
    }

    Mesh createCollision(Mesh source, float collZ, float collExtent)
    {
        var vertices = source.vertices;
        var normals = source.normals;
        var indices = source.GetIndices(0);

        Mesh m = new Mesh();
        m.name = "Generated collision";

        List<int> newVertIndices = new List<int>();

        for(int i=0; i<vertices.Length; i++)
        {
            var v = vertices[i];
            var absZ = Mathf.Abs(v.z);

            if (absZ >= collZ - collExtent && absZ <= collZ + collExtent)
            {
                newVertIndices.Add(i);
            }
        }

        var newVertices = new Vector3[newVertIndices.Count];
        var newNormals = new Vector3[newVertices.Length];
        var newIndices = new List<int>();

        Dictionary<int, int> indexMapping = new Dictionary<int, int>();

        for(int i=0; i<newVertices.Length; i++)
        {
            var ind = newVertIndices[i];
            newVertices[i] = vertices[ind];
            newNormals[i] = normals[ind];
            indexMapping.Add(ind, i);
        }

        for (int i = 0; i < indices.Length; i+=3)
        {
            var i1 = indices[i];
            var i2 = indices[i+1];
            var i3 = indices[i+2];

            if(newVertIndices.Contains(i1) && newVertIndices.Contains(i2) && newVertIndices.Contains(i3))
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

    void recalcNormals(Mesh mesh)
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

        for (int i=0; i<indices.Length; i+=3)
        {
            var i1 = indices[i];
            var i2 = indices[i+1];
            var i3 = indices[i+2];

            var a = vertices[i2] - vertices[i1];
            var b = vertices[i3] - vertices[i2];

            //a.Normalize();
            //b.Normalize();

            var c = Vector3.Cross(a, b);
            c.Normalize();

            //faceNormals[j] = c;
            //j++;

            neighbourMap[i1].Add(c);
            neighbourMap[i2].Add(c);
            neighbourMap[i3].Add(c);
        }

        for (int i = 0; i < normals.Length; i++)
        {
            var n = normals[i];
            n.x = n.y = n.z = 0;

            var nbrs = neighbourMap[i];

            for(int j=0; j<nbrs.Count; j++)
            {
                n += nbrs[j];
            }

            //for (int j = 0, k = 0; j < indices.Length; j+=3)
            //{
            //    if(i == indices[j] || i == indices[j + 1] || i == indices[j + 2])
            //    {
            //        n += faceNormals[k];
            //    }
            //    k++;
            //}

            n.Normalize();
            normals[i] = n;
        }

        mesh.normals = normals;
    }


}
