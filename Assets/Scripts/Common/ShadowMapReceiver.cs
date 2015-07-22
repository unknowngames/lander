using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShadowMapReceiver : MonoBehaviour 
{
	public Camera shadowProjector;
	private Material material;
	Matrix4x4 m;

	void Start()
	{
		var mr = GetComponent<MeshRenderer> ();
		material = mr.sharedMaterial;


		m = Matrix4x4.Ortho (-10, 10, -10, 10, 0.3f, 50);
	}

	void Update()
	{
		if (shadowProjector == null || material == null)
			return;

		shadowProjector.projectionMatrix = m;
		shadowProjector.Render ();
		material.SetMatrix ("_ProjectionMatrix", shadowProjector.projectionMatrix * shadowProjector.worldToCameraMatrix /* * shadowProjector.transform.localToWorldMatrix*/);

	}
}
