using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
	public class LandingZoneNavigator : MonoBehaviour 
	{
		public Sprite Pointer;

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

		private UnityEngine.UI.Image[] pointers;

		void Start()
		{
			pointers = new UnityEngine.UI.Image[maxPointersCount];
			RectTransform parent = GetComponent<RectTransform> ();

			for (int i=0; i<maxPointersCount; i++) 
			{
				GameObject pointer = new GameObject("Pointer");
				var rect = pointer.AddComponent<RectTransform>();
				rect.SetParent(parent);
				pointer.AddComponent<CanvasRenderer>();
				var image = pointer.AddComponent<UnityEngine.UI.Image>();

				rect.pivot = new Vector2(0.5f, 0.5f);
				rect.anchorMax = new Vector2(0.5f, 0.5f);
				rect.anchorMin = new Vector2(0.5f, 0.5f);

				rect.localPosition = new Vector2(0.0f, -radius);

				rect.sizeDelta = new Vector2(Pointer.texture.width, Pointer.texture.height);


				image.sprite = Pointer;
				image.color = new Color(1,1,1,0);

				pointers[i] = image;
			}
		}
		
		void Update()
		{
			var landingZones = GameObject.FindGameObjectsWithTag (landingZoneTag);

			if (landingZones == null || landingZones.Length == 0)
				return;

			List<GameObject> objectsInSearchRadius = new List<GameObject> ();
			float sqrRadius = searchRadius * searchRadius;

			foreach (var zone in landingZones) 
			{
				var dist = (GameHelper.PlayerSpaceship.transform.position - zone.transform.position).sqrMagnitude;

				if(dist <= sqrRadius)
					objectsInSearchRadius.Add(zone);
			}

			if (objectsInSearchRadius.Count > maxPointersCount) 
			{
				objectsInSearchRadius.RemoveRange(maxPointersCount, objectsInSearchRadius.Count-maxPointersCount);
			}
			landingZones = objectsInSearchRadius.ToArray ();

			if (landingZones == null || landingZones.Length == 0)
				return;

			System.Array.Sort (	landingZones, 
			                   delegate (GameObject obj1, GameObject obj2) 
			{ 
				float dist1 = (GameHelper.PlayerSpaceship.transform.position - obj1.transform.position).sqrMagnitude;
				float dist2 = (GameHelper.PlayerSpaceship.transform.position - obj2.transform.position).sqrMagnitude;

				if(dist1 > dist2) return 1;
				else if(dist1 < dist2) return -1;
				else return 0;				
			}
			);	

			var sqrFadeOutDistance = this.fadeOutStartDistance * this.fadeOutStartDistance;
			var sqrMinDistance = minDistance * minDistance;
			for (int i=0; i<maxPointersCount; i++) 
			{
				var pointer = pointers[i];
				var c = pointer.color;

				if(i < landingZones.Length)
				{
					var zone = landingZones[i];

					var dir = zone.transform.position - GameHelper.PlayerSpaceship.transform.position;
					var sqrDist = dir.sqrMagnitude;

					dir.z = 0;
					dir.Normalize();

					pointer.rectTransform.localPosition = dir * radius;

					var dot = Vector3.Dot(Vector3.down, dir);
					dot = 1.0f - ((dot + 1.0f) / 2.0f);
					if(dir.x < 0.0f) 
						dot = -dot;
					var rot = Quaternion.Euler(0,0,180 * dot);
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
