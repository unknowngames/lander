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
    private Mesh currentMesh = null;
    bool drawNormals = false;
    List<MeshInfo> history = new List<MeshInfo>();

    float minSinAmplitude = 0.25f;
    float maxSinAmplitude = 1.0f;

    float minSinFrequency = 0.1f;
    float maxSinFrequency = 1.0f;

    float minSinStepness = 1.0f;
    float maxSinStepness = 2.0f;

    [MenuItem("Unknown games/Land generator")]
    static void show()
    {
        var window = GetWindow<LandGenerator>();
        window.Show();
        SceneView.onSceneGUIDelegate += window.OnSceneGUI;
    }

    void OnGUI()
    {
        //if(GUILayout.Button("Сбросить"))
        //{
        //    history.Clear();
        //}



        if(GUILayout.Button("Сгенерировать"))
        {
            var go = GameObject.Find("Generated land");
            if(go != null)
            {
                currentMesh = null;
                DestroyImmediate(go);
            }

            currentMesh = generatePlane(100,100, 1.0f);
        }

        if (currentMesh == null)
            return;

        drawNormals = GUILayout.Toggle(drawNormals, "Показать нормали");

        if(GUILayout.Button("Пересчитать нормали"))
        {
            recalcNormals(currentMesh);
        }

        if(GUILayout.Button("Применить Sin"))
        {
            applySinWave(currentMesh, 
                new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f, 1.0f)).normalized, 
                Random.Range(minSinFrequency, maxSinFrequency),
                Random.Range(minSinAmplitude, maxSinAmplitude),
                Random.Range(minSinStepness, maxSinStepness));
            //applySinWave(currentMesh, new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized, 1.0f, 1.0f, 1.0f);

            recalcNormals(currentMesh);
        }

        minSinAmplitude = EditorGUILayout.FloatField("Мин амплитуда Sin", minSinAmplitude);
        maxSinAmplitude = EditorGUILayout.FloatField("Макс амплитуда Sin", maxSinAmplitude);
        minSinFrequency = EditorGUILayout.FloatField("Мин частота Sin", minSinFrequency);
        maxSinFrequency = EditorGUILayout.FloatField("Макс частота Sin", maxSinFrequency);
        minSinStepness = EditorGUILayout.FloatField("Мин крутизна волны Sin", minSinStepness);
        maxSinStepness = EditorGUILayout.FloatField("Макс крутизна Sin", maxSinStepness);
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

    Mesh generatePlane(int width, int height, float cellSize)
    {
        var go = new GameObject("Generated land");

        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = new Material(Shader.Find("Diffuse"));

        Mesh mesh = new Mesh();
        mesh.name = "generated land";
        mf.sharedMesh = mesh;

        float halfWidth = width / 2.0f;
        float halfHeight = height / 2.0f;

        Vector3[] verts = new Vector3[(width + 1) * (height + 1)];
        Vector3[] normals = new Vector3[verts.Length];
        int[] indexes = new int[width * height * 6];

        for(int i=0; i <= height; i++)
        {
            for(int j=0; j <= width; j++)
            {
                int index = i * (width+1) + j;

                verts[index] = new Vector3(j * cellSize - halfWidth, 0, (height - i) * cellSize - halfHeight);
                normals[index] = new Vector3(0, 1, 0);
            }
        }

        int currentIndex = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x0 = i * (width+1) + j;
                int x1 = x0+1;
                int y0 = (i+1) * (width+1) + j;
                int y1 = y0 + 1;

                indexes[currentIndex] = x0;
                indexes[currentIndex+1] = x1;
                indexes[currentIndex+2] = y0;
                indexes[currentIndex+3] = y0;
                indexes[currentIndex+4] = x1;
                indexes[currentIndex+5] = y1;

                currentIndex += 6;
            }
        }

        mesh.vertices = verts;
        mesh.normals = normals;
        mesh.SetIndices(indexes, MeshTopology.Triangles, 0);

        return mesh;
    }

    void applySinWave(Mesh mesh, Vector2 direction, float frequency, float amplitude, float stepness)
    {
        var verts = mesh.vertices;
        var normals = mesh.normals;

        for(int i=0; i<verts.Length; i++)
        {
            var v = verts[i];

            float s = (direction.x * v.x + direction.y * v.z) * frequency;

            float height = Mathf.Pow(((Mathf.Sin(s) + 1.0f) / 2.0f), stepness) * amplitude;
            v.y += height;
            verts[i] = v;

            float dZ = direction.y * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
            Vector3 binormal = new Vector3(0, dZ, 1).normalized;

            float dX = direction.x * frequency * amplitude * Mathf.Pow(2, -stepness) * stepness * Mathf.Cos(s) * Mathf.Pow(Mathf.Sin(s) + 1.0f, stepness);
            Vector3 tangent = new Vector3(1, dX, 0).normalized;

            normals[i] = (Vector3.Cross(binormal, tangent).normalized);
        }

        mesh.vertices = verts;
        mesh.normals = normals;
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
