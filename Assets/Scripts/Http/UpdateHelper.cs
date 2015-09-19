using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Http
{
	public class UpdateHelper : MonoBehaviour 
	{
		public float updateRate = 0.5f;
		public delegate void VoidDelegate();
		public event VoidDelegate OnUpdate;
		float lastUpdateTickTime = 0.0f;

		void internalUpdate()
		{
			if(OnUpdate != null) OnUpdate();
		}

		void Update () 
		{
			if(Time.realtimeSinceStartup - lastUpdateTickTime >= updateRate)
			{
				lastUpdateTickTime = Time.realtimeSinceStartup;
				internalUpdate();
			}
		}
	}
}