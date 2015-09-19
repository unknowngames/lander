using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using Assets.Scripts.Http;
using Assets.Scripts.Console;

namespace Assets.Scripts.Common
{
	public enum AnalyticsErrorTypes
	{
		Critical,
		Error,
		Warning,
		Debug,
		Info
	}

    public enum AnalyticResourceFlowType
    {
        Sink,
        Source
    }

	public class AnalyticsManager
	{
		static string game_key = "UnknownGameKey";
		static string secret_key = "UnknownSecretkey";
		static string endpoint_url = "UnknownUrl";

		static string user_id = "UnknownUser";
		static string session_id = "UnknownSession";

		//private static List<AnalyticsEvent> pendingEvents = new List<AnalyticsEvent>();


		protected AnalyticsManager() {}

		public static void SetGameKey(string gameKey)
		{
			game_key = gameKey;
		}

		public static void SetSecretKey(string secretKey)
		{
			secret_key = secretKey;
		}

		public static void SetEndpointUrl(string endpointUrl)
		{
			endpoint_url = endpointUrl;
		}

		public static void SetUserID(string userID)
		{
			user_id = userID;
		}

		public static void SetSessionID(string sessionID)
		{
			session_id = sessionID;
		}

		[ConsoleCommand]
		public static void SendErrorEvent(AnalyticsErrorTypes errorType, string message)
		{
			string category = "error";
			
			
			Jboy.JsonWriter writer = new Jboy.JsonWriter ();
			writer.WriteObjectStart ();
			
			writer.WritePropertyName ("user_id");
			writer.WriteString (user_id);
			
			writer.WritePropertyName ("session_id");
			writer.WriteString (session_id);

			writer.WritePropertyName ("severity");
			writer.WriteString (errorType.ToString());

			writer.WritePropertyName ("message");
			writer.WriteString (message);
			
			writer.WriteObjectEnd ();
			
			
			
			string json_message = writer.ToString ();
			
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] authData = Encoding.Default.GetBytes(json_message + secret_key);
			byte[] authHash = md5.ComputeHash(authData);
			// Transforms as hexa
			string hexaHash = "";
			foreach (byte b in authHash) {
				hexaHash += String.Format("{0:x2}", b);
			}
			byte[] jsonData = Encoding.ASCII.GetBytes(json_message); 
			
			
			string url = endpoint_url + "/" + game_key + "/" + category;
			
			Dictionary<string,string> customHeaders = new Dictionary<string, string> ();
			customHeaders.Add ("Authorization", hexaHash);
			var httpRequest = HttpApi.Request(url, jsonData, customHeaders);
			
			while (httpRequest.isDone == false)
				continue;
			
			Debug.Log (httpRequest.Text);
			
			return;
		}

		public static void SendDesignEvent(string event_id)
		{
			string category = "design";
			
			
			Jboy.JsonWriter writer = new Jboy.JsonWriter ();
			writer.WriteObjectStart ();
			writer.WritePropertyName ("event_id");
			writer.WriteString (event_id);
			
			writer.WritePropertyName ("user_id");
			writer.WriteString (user_id);
			
			writer.WritePropertyName ("session_id");
			writer.WriteString (session_id);
			
			writer.WritePropertyName ("build");
			writer.WriteString ("1.0");
			
			writer.WritePropertyName ("value");
			writer.WriteString ("1.0");
			
			writer.WritePropertyName ("area");
			writer.WriteString ("unknown area");
			
			writer.WriteObjectEnd ();
			
			
			
			string json_message = writer.ToString ();
			
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] authData = Encoding.Default.GetBytes(json_message + secret_key);
			byte[] authHash = md5.ComputeHash(authData);
			// Transforms as hexa
			string hexaHash = "";
			foreach (byte b in authHash) {
				hexaHash += String.Format("{0:x2}", b);
			}
			byte[] jsonData = Encoding.ASCII.GetBytes(json_message); 
			
			
			string url = endpoint_url + "/" + game_key + "/" + category;
			
			Dictionary<string,string> customHeaders = new Dictionary<string, string> ();
			customHeaders.Add ("Authorization", hexaHash);
			var httpRequest = HttpApi.Request(url, jsonData, customHeaders);
			
			httpRequest.OnRequestDone += HandleOnRequestDone;
			
			return;
		}

        public static void SendResourceEvent(AnalyticResourceFlowType flowType, string itemId, string itemType, float amount, string resourceCurrency)
        {
            string category = "resource";


            Jboy.JsonWriter writer = new Jboy.JsonWriter();
            writer.WriteObjectStart();
            writer.WritePropertyName("flowType");
            string flow = flowType == AnalyticResourceFlowType.Sink ? "sink" : "source";
            writer.WriteString(flow);

            writer.WritePropertyName("itemType");
            writer.WriteString(itemType);

            writer.WritePropertyName("itemId");
            writer.WriteString(itemId);

            writer.WritePropertyName("amount");
            writer.WriteString(amount.ToString());

            writer.WritePropertyName("resourceCurrency");
            writer.WriteString(resourceCurrency);

            writer.WriteObjectEnd();



            string json_message = writer.ToString();

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] authData = Encoding.Default.GetBytes(json_message + secret_key);
            byte[] authHash = md5.ComputeHash(authData);
            // Transforms as hexa
            string hexaHash = "";
            foreach (byte b in authHash)
            {
                hexaHash += String.Format("{0:x2}", b);
            }
            byte[] jsonData = Encoding.ASCII.GetBytes(json_message);


            string url = endpoint_url + "/" + game_key + "/" + category;

            Dictionary<string, string> customHeaders = new Dictionary<string, string>();
            customHeaders.Add("Authorization", hexaHash);
            var httpRequest = HttpApi.Request(url, jsonData, customHeaders);

            httpRequest.OnRequestDone += HandleOnRequestDone;

            return;
        }

		[ConsoleCommand]
		public static void GameAnalyticsTest()
		{
			//NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
//			foreach (NetworkInterface adapter in nics)
//			{
//				PhysicalAddress address = adapter.GetPhysicalAddress();
//				if (address.ToString() != "" && user_id == "")
//				{
//					byte[] bytes = Encoding.UTF8.GetBytes(address.ToString());
//					SHA1CryptoServiceProvider SHA = new SHA1CryptoServiceProvider();
//					user_id = BitConverter.ToString(SHA.ComputeHash(bytes)).Replace("-", "");
//				}
//			}

			string category = "design";


			Jboy.JsonWriter writer = new Jboy.JsonWriter ();
			writer.WriteObjectStart ();
			writer.WritePropertyName ("event_id");
			writer.WriteString ("pickup:rocket_launcher");

			writer.WritePropertyName ("user_id");
			writer.WriteString (user_id);

			writer.WritePropertyName ("session_id");
			writer.WriteString ("session_1");

			writer.WritePropertyName ("build");
			writer.WriteString ("1.0");

			writer.WritePropertyName ("value");
			writer.WriteString ("1.0");

			writer.WritePropertyName ("area");
			writer.WriteString ("auction House");

			writer.WriteObjectEnd ();



			string json_message = writer.ToString ();
			
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] authData = Encoding.Default.GetBytes(json_message + secret_key);
			byte[] authHash = md5.ComputeHash(authData);
			// Transforms as hexa
			string hexaHash = "";
			foreach (byte b in authHash) {
				hexaHash += String.Format("{0:x2}", b);
			}
			byte[] jsonData = Encoding.ASCII.GetBytes(json_message); 


			string url = endpoint_url + "/" + game_key + "/" + category;

			Dictionary<string,string> customHeaders = new Dictionary<string, string> ();
			customHeaders.Add ("Authorization", hexaHash);
			var httpRequest = HttpApi.Request(url, jsonData, customHeaders);

			httpRequest.OnRequestDone += HandleOnRequestDone;

			return;

//			HttpForm form = new HttpForm ();
//			form.Headers.Add("Authorization", hexaHash);
//			form.Method = HttpRestMethods.POST;
//			form.MimeType = "application/x-www-form-urlencoded";
//			form.AddBinaryData (jsonData);
//			form.BinaryFileName = "";
		}

		static void HandleOnRequestDone (HttpRequest request)
		{
			Debug.Log (request.Text);
		}
	}
	
	public struct AnalyticsEvent
	{
		public int callMethod;
		public string category;
		public string eventId;
	}
}