using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class ShadowMapReceiver : MonoBehaviour 
{
	public Camera shadowProjector;
	public Texture ShadowMap;

	private Material[] mats;

	void Start()
	{
		if (shadowProjector == null)
			tryFindShadowProjector ();

		var renderer = GetComponent<Renderer> ();

		mats = renderer.materials;

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

		if (shadowProjector == null || mats == null || mats.Length == 0)
			return;

		var pv = shadowProjector.projectionMatrix * shadowProjector.worldToCameraMatrix;

		foreach (var m in mats) 
		{
			m.SetMatrix ("_ProjectionMatrix", pv);
		}
	}
}
