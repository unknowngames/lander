using UnityEngine;
using System.Collections;

public class SetupDepthCamera : MonoBehaviour 
{
	public Shader DepthShader;

	void Start()
	{
		var camera = GetComponent<Camera> ();
		camera.depthTextureMode = DepthTextureMode.Depth;
		camera.SetReplacementShader (DepthShader, "");
	}
}
