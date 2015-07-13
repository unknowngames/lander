using UnityEngine;
using System.Collections;
using UnityEditor;

public class VertexLightmapEditor : EditorWindow
{
	MeshFilter targetMesh = null;
	Light targetLight = null;
	Color ambientLight = Color.gray;

	[MenuItem("Unknown games/Vertex Lightmap Editor")]
	static void show()
	{
		var window = EditorWindow.GetWindow<VertexLightmapEditor> ();
		window.Show ();
	}

	void OnGUI()
	{
		targetMesh = EditorGUILayout.ObjectField ("Target mesh", targetMesh, typeof(MeshFilter), true) as MeshFilter;
		targetLight = EditorGUILayout.ObjectField ("Target light", targetLight, typeof(Light), true) as Light;
		ambientLight = EditorGUILayout.ColorField ("Ambient light", ambientLight);

		if (targetMesh == null || targetLight == null)
			return;

		if (GUILayout.Button ("Calculate")) 
		{
			calculate(targetLight, targetMesh);
		}
	}

	void calculate(Light light, MeshFilter mf)
	{
		//var mc = mf.gameObject.AddComponent<MeshCollider> ();
		//mc.sharedMesh = mf.sharedMesh;

		//Vector3 lightDir = new Vector3 (1,-0.5f,1).normalized;// light.transform.forward;
		Vector3 lightDir = light.transform.forward;

		var vertices = mf.sharedMesh.vertices;
		var length = vertices.Length;
		Color[] colors = new Color[length];
		var normals = mf.sharedMesh.normals;
	
		var transform = mf.transform;

		for (int i=0; i<length; i++) 
		{
			var v = vertices[i];
			var n = normals[i];

			var tV = transform.TransformPoint(v);
			var tN = transform.TransformVector(n);

			var inShadow = false;

			RaycastHit hit;

			if(Physics.Raycast(tV + tN*0.05f, -lightDir, out hit))
			{
				Debug.Log(hit.collider.name);
				inShadow = true;
			}

			var dot = Vector3.Dot(tN, -lightDir);

			dot = Mathf.Clamp(dot, 0.0f, 1.0f);

			var finalColor = ambientLight + dot * light.intensity * light.color;
			finalColor = inShadow ? ambientLight : ambientLight + finalColor;
		

			if(finalColor.r > 1.0f)
				finalColor.r = 1.0f;
			if(finalColor.g > 1.0f)
				finalColor.g = 1.0f;
			if(finalColor.b > 1.0f)
				finalColor.b = 1.0f;

			colors[i] = finalColor;
			//colors[i] = new Color(tV.x, tV.y, tV.z);
		}

		mf.sharedMesh.colors = colors;

		//GameObject.DestroyImmediate (mc);
	}
}
