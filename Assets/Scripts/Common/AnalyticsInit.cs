using UnityEngine;

namespace Assets.Scripts.Common
{
	public class AnalyticsInit : MonoBehaviour 
	{
		[SerializeField]
		private string gameKey;

        [SerializeField]
        private string secretKey;

        [SerializeField]
        private string endPointUrl = "http://api.gameanalytics.com/1";
		
		void Awake () 
		{
			AnalyticsManager.SetGameKey(gameKey);
			AnalyticsManager.SetSecretKey(secretKey);
			AnalyticsManager.SetEndpointUrl(endPointUrl);

            AnalyticsManager.UserID = SystemInfo.deviceUniqueIdentifier;

			if (string.IsNullOrEmpty (AnalyticsManager.SessionID)) 
			{
				string sessionID = System.Guid.NewGuid ().ToString ();
				AnalyticsManager.SessionID = sessionID;
            }
		}
	}
}
