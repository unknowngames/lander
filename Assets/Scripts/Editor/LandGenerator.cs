using UnityEngine;
using System.Collections;
using UnityEditor;

public class LandGenerator : EditorWindow
{
    private Mesh currentMesh = null;
    bool drawNormals = false;

    [MenuItem("Unknown games/Land generator")]
    static void show()
    {
        var window = GetWindow<LandGenerator>();
        window.Show();
        SceneView.onSceneGUIDelegate += window.OnSceneGUI;
    }

    void OnGUI()
    {
        if(GUILayout.Button("Generate"))
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

        if(GUILayout.Button("Применить Sin"))
        {
            //applySinWave(currentMesh, new Vector2(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f)).normalized, Random.Range(0.25f, 1.0f), 2f, 2.0f);
            applySinWave(currentMesh, new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized, 1.0f, 1.0f, 1.0f);
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
        var oldNormals = mesh.normals;
        Vector2 pos = Vector2.zero;

        for(int i=0; i<verts.Length; i++)
        {
            var v = verts[i];

            pos.x = v.x;
            pos.y = v.z;
            float s = direction.x * pos.x + direction.y * pos.y;
            //s = (Mathf.Sin(s * frequency) + 1.0f) / 2.0f;
            //s = Mathf.Pow(s, stepness);
            //v.y += s * amplitude;

            //s = v.x + v.z;
            float height =  (Mathf.Sin(s) + 1.0f) / 2.0f;
            //height *= amplitude;
            v.y += height;
            verts[i] = v;

            float dZ = Mathf.Cos(s)*direction.y / 2.0f;
            Vector3 binormal = new Vector3(0, dZ, 1).normalized;

            float dX = Mathf.Cos(s)*direction.x / 2.0f;
            //dX *= amplitude;
            Vector3 tangent = new Vector3(1, dX, 0).normalized;

            normals[i] = (Vector3.Cross(binormal, tangent).normalized + oldNormals[i]) / 2.0f;
        }

        mesh.vertices = verts;
        mesh.normals = normals;
    }
}
