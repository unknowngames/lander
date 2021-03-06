﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.HUD
{
	public class LandingZoneNavigator : MonoBehaviour
    {
        [SerializeField]
        private RectTransform pointerPrefab;

		[SerializeField]
		private string landingZoneTag = "LandingZone";

		[SerializeField]
		float searchRadius = 500;

		[SerializeField]
		private int maxPointersCount = 5;

		[SerializeField]
		private float radius = 200;

		[SerializeField]
		float fadeOutStartDistance = 300;

		[SerializeField]
		float minDistance = 50;

		private Image[] pointers;

        public void Start()
		{
			pointers = new Image[maxPointersCount];
			RectTransform parent = GetComponent<RectTransform> ();

			for (int i=0; i<maxPointersCount; i++)
			{
			    RectTransform rect = Instantiate(pointerPrefab);

                rect.SetParent(parent, false);
				rect.pivot = new Vector2(0.5f, 0.5f);
				rect.anchorMax = new Vector2(0.5f, 0.5f);
				rect.anchorMin = new Vector2(0.5f, 0.5f);

				rect.localPosition = new Vector2(0.0f, -radius);

                pointers[i] = rect.GetComponent<Image>();
			}
		}

	    public void Update()
		{
			GameObject[] landingZones = GameObject.FindGameObjectsWithTag (landingZoneTag);

			if (landingZones == null || landingZones.Length == 0)
				return;

			List<GameObject> objectsInSearchRadius = new List<GameObject> ();
			float sqrRadius = searchRadius * searchRadius;

			foreach (GameObject zone in landingZones) 
			{
                float dist = (PlayerSpawner.PlayerSpaceship.transform.position - zone.transform.position).sqrMagnitude;

				if(dist <= sqrRadius)
					objectsInSearchRadius.Add(zone);
			}

			if (objectsInSearchRadius.Count > maxPointersCount) 
			{
				objectsInSearchRadius.RemoveRange(maxPointersCount, objectsInSearchRadius.Count-maxPointersCount);
			}
			landingZones = objectsInSearchRadius.ToArray ();

	        if (landingZones.Length == 0)
	        {
	            return;
	        }

			System.Array.Sort (	landingZones, 
			                   delegate (GameObject obj1, GameObject obj2) 
			{
                float dist1 = (PlayerSpawner.PlayerSpaceship.transform.position - obj1.transform.position).sqrMagnitude;
                float dist2 = (PlayerSpawner.PlayerSpaceship.transform.position - obj2.transform.position).sqrMagnitude;

			    if (dist1 > dist2)
			    {
			        return 1;
			    }
			    if (dist1 < dist2)
			    {
			        return -1;
			    }
			    return 0;
			}
			);	

			float sqrFadeOutDistance = fadeOutStartDistance * fadeOutStartDistance;
			float sqrMinDistance = minDistance * minDistance;
			for (int i=0; i<maxPointersCount; i++) 
			{
				Image pointer = pointers[i];
				Color c = pointer.color;

				if(i < landingZones.Length)
				{
					GameObject zone = landingZones[i];

                    Vector3 dir = zone.transform.position - PlayerSpawner.PlayerSpaceship.transform.position;
					float sqrDist = dir.sqrMagnitude;

					dir.z = 0;
					dir.Normalize();

					pointer.rectTransform.localPosition = dir * radius;

					float dot = Vector3.Dot(Vector3.down, dir);
					dot = 1.0f - ((dot + 1.0f) / 2.0f);
					if(dir.x < 0.0f) 
						dot = -dot;
					Quaternion rot = Quaternion.Euler(0,0,180 * dot);
					pointer.rectTransform.rotation = rot;

					if(sqrDist < sqrMinDistance)
					{
						c.a = 0;
					}
					else if(sqrDist > sqrFadeOutDistance)
					{
						c.a = sqrFadeOutDistance / sqrDist;
					}
					else
					{
						c.a = 1;
					}
				}
				else
				{
					c.a = 0;
				}

				pointer.color = c;
			}

		}
	}
}
