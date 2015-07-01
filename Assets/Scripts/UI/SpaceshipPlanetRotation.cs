using UnityEngine;
using System.Collections;

public class SpaceshipPlanetRotation : MonoBehaviour 
{
    public float Speed = 5.0f;
    public Transform Axis;

	void LateUpdate () 
    {
        transform.Rotate(Axis.transform.forward, Time.deltaTime * Speed);
	}
}
