using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Common
{
    public enum AnalyticsHandlerMethod
    {
        None,
        Start,
        OnDestroy,
        OnEnable,
        OnDisable,
        OnApplicationQuit
    }
    public class AnalyticsEventHandler : MonoBehaviour
    {
        [SerializeField]
        private string designEventId;

        [SerializeField]
        private AnalyticsHandlerMethod method = AnalyticsHandlerMethod.None;

        public void ExecuteEvent()
        {
            AnalyticsManager.SendDesignEvent(designEventId);
        }

		public void ExecuteDesignEvent(string eventId)
		{
			AnalyticsManager.SendDesignEvent(eventId);
		}

        void Start()
        {
            if(method == AnalyticsHandlerMethod.Start)
            {
                ExecuteEvent();
            }
        }

        void OnDestroy()
        {
            if (method == AnalyticsHandlerMethod.OnDestroy)
            {
                ExecuteEvent();
            }
        }

        void OnApplicationQuit()
        {
            if (method == AnalyticsHandlerMethod.OnApplicationQuit)
            {
                ExecuteEvent();
            }
        }

        void OnEnable()
        {
            if (method == AnalyticsHandlerMethod.OnEnable)
            {
                ExecuteEvent();
            }
        }

        void OnDisable()
        {
            if (method == AnalyticsHandlerMethod.OnDisable)
            {
                ExecuteEvent();
            }
        }
    }
}
