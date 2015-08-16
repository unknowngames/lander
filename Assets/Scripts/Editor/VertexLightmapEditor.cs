using UnityEngine;
using System.Collections;
using UnityEditor;

public class VertexLightmapEditor : EditorWindow
{
	MeshFilter targetMesh = null;
	Light targetLight = null;
	Color ambientLight = Color.gray;
    bool calcShadow = true;

	[MenuItem("Unknown games/Vertex Lightmap Editor")]
	public static void CreateWindow()
	{
		VertexLightmapEditor window = GetWindow<VertexLightmapEditor> ();
		window.Show ();
	}

    public void OnGUI()
	{
		targetMesh = EditorGUILayout.ObjectField ("Target mesh", targetMesh, typeof(MeshFilter), true) as MeshFilter;
		targetLight = EditorGUILayout.ObjectField ("Target light", targetLight, typeof(Light), true) as Light;
		ambientLight = EditorGUILayout.ColorField ("Ambient light", ambientLight);
        calcShadow = EditorGUILayout.Toggle("Расчет тени", calcShadow);

        if (targetMesh == null || targetLight == null)
        {
            return;
        }

		if (GUILayout.Button ("Calculate")) 
		{
            Calculate(targetLight, targetMesh);
		}

		if (GUILayout.Button ("Save mesh to file")) 
		{
			string path = EditorUtility.SaveFilePanelInProject ("Save mesh", "mesh", "asset", "save me please");
			
			if(string.IsNullOrEmpty(path) == false)
			{
				AssetDatabase.CreateAsset(targetMesh.sharedMesh, path);
				AssetDatabase.SaveAssets();
			}
		}
	}

    private void Calculate(Light light, MeshFilter mf)
	{
		//var mc = mf.gameObject.AddComponent<MeshCollider> ();
		//mc.sharedMesh = mf.sharedMesh;

		//Vector3 lightDir = new Vector3 (1,-0.5f,1).normalized;// light.transform.forward;

		Mesh mesh = mf.sharedMesh;
		Vector3 lightDir = light.transform.forward;

		Vector3[] vertices = mesh.vertices;
		int length = vertices.Length;
		Color[] colors = new Color[length];
		Vector3[] normals = mesh.normals;
	
		Transform transform = mf.transform;

		for (int i=0; i<length; i++) 
		{
			Vector3 v = vertices[i];
			Vector3 n = normals[i];

			Vector3 tV = transform.TransformPoint(v);
			Vector3 tN = transform.TransformVector(n);

			bool inShadow = false;

            if(calcShadow)
            {
                RaycastHit hit;

                if (Physics.Raycast(tV + tN * 0.01f, -lightDir, out hit))
                {
                    Debug.Log(hit.collider.name);
                    inShadow = true;
                }
            }

			float dot = Vector3.Dot(tN, -lightDir);

			dot = Mathf.Clamp(dot, 0.0f, 1.0f);

			Color finalColor = ambientLight + dot * light.intensity * Color.white;
			finalColor = inShadow ? ambientLight : ambientLight + finalColor;
		

			/*if(finalColor.r > 1.0f)
				finalColor.r = 1.0f;
			if(finalColor.g > 1.0f)
				finalColor.g = 1.0f;
			if(finalColor.b > 1.0f)
				finalColor.b = 1.0f;*/

			colors[i] = finalColor;
			//colors[i] = new Color(tV.x, tV.y, tV.z);
		}

		mesh.colors = colors;

		EditorUtility.SetDirty (mesh);

		//GameObject.DestroyImmediate (mc);
	}
}
