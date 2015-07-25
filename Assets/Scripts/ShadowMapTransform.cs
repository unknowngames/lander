using UnityEngine;
using System.Collections;

public class ShadowMapTransform : MonoBehaviour 
{
	Vector3 positionOffset = Vector3.zero;
	Transform target = null;

	void Start () 
	{
		positionOffset = transform.localPosition;
		target = transform.parent;
		transform.parent = null;
	}

	void LateUpdate () 
	{
		transform.position = target.position;
	}
}
