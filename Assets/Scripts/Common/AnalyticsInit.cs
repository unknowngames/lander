using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Common
{
	public class AnalyticsInit : MonoBehaviour 
	{
		[SerializeField]
		private string gameKey;
		private string secretKey;
		private string endPointUrl = "http://api.gameanalytics.com/1";
		
		void Start () 
		{
			AnalyticsManager.SetGameKey(gameKey);
			AnalyticsManager.SetSecretKey(secretKey);
			AnalyticsManager.SetEndpointUrl(endPointUrl);

			if (string.IsNullOrEmpty (AnalyticsManager.SessionID)) 
			{
				string sessionID = System.Guid.NewGuid ().ToString ();
				AnalyticsManager.SessionID = sessionID;
			}
		}
	}
}
