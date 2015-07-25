using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class ShadowMapReceiver : MonoBehaviour 
{
	public Camera shadowProjector;
	public Texture ShadowMap;
	private Material material;


	void Start()
	{
		var mr = GetComponent<MeshRenderer> ();
		material = mr.sharedMaterial;

		if (shadowProjector == null)
			tryFindShadowProjector ();

		var renderer = GetComponent<Renderer> ();

		var mats = renderer.materials;

		foreach (var m in mats) 
		{
			if(m.HasProperty("_ShadowMap"))
			{
				m.SetTexture("_ShadowMap", ShadowMap);
			}
		}
	}

	void tryFindShadowProjector()
	{
		var go = GameObject.FindGameObjectWithTag ("ShadowMapCamera");

		if (go == null)
			return;

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
