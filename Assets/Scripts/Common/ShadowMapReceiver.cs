using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShadowMapReceiver : MonoBehaviour 
{
	public Camera shadowProjector;
	private Material material;


	void Start()
	{
		var mr = GetComponent<MeshRenderer> ();
		material = mr.sharedMaterial;

		if (shadowProjector == null)
			tryFindShadowProjector ();
	}

	void tryFindShadowProjector()
	{
		var go = GameObject.FindGameObjectWithTag ("ShadowMapCamera");
		shadowProjector = go.GetComponent<Camera> ();
	}

	void Update()
	{
		if (shadowProjector == null)
			tryFindShadowProjector ();

		if (shadowProjector == null || material == null)
			return;

		var pv = shadowProjector.projectionMatrix * shadowProjector.worldToCameraMatrix;
		material.SetMatrix ("_ProjectionMatrix", pv);
	}
}
