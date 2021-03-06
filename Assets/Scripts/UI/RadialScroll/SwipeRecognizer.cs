﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.RadialScroll
{
    public class SwipeRecognizer : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private static readonly ExecuteEvents.EventFunction<ISwipeGesture> swipeGestureHandler = Execute;

        private static void Execute(ISwipeGesture handler, BaseEventData eventData)
        {
            handler.OnSwipe(ExecuteEvents.ValidateEventData<SwipeGestureData>(eventData));
        }

        public GameObject Handler;
   
        private Dictionary<int, SwipeState> swipeStates = new Dictionary<int, SwipeState>();

        public void OnBeginDrag(PointerEventData eventData)
        {
            int pointerId = eventData.pointerId;
            if (swipeStates.ContainsKey(pointerId))
            {
                if (swipeStates[pointerId].Phase == ESwipePhase.Start)
                {
                }
                else
                {
                    swipeStates.Remove(pointerId);

                    swipeStates.Add(pointerId, new SwipeState(eventData, Time.unscaledTime));
                    SendSwipeEvent(pointerId);
                }
            }
            else
            {    
                swipeStates.Add(pointerId, new SwipeState(eventData, Time.unscaledTime));
                SendSwipeEvent(pointerId);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            int pointerId = eventData.pointerId;
            if (swipeStates.ContainsKey(pointerId))
            {
                swipeStates[pointerId].Move(eventData, Time.unscaledTime);
                SendSwipeEvent(pointerId);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            int pointerId = eventData.pointerId;
            if (swipeStates.ContainsKey(pointerId))
            {
                swipeStates[pointerId].End(eventData);
                SendSwipeEvent(pointerId);

                swipeStates.Remove(pointerId);
            }
        }

        private void SendSwipeEvent(int pointerId)
        {
            SwipeGestureData data = swipeStates[pointerId].GetSwipeGestureData();
            ExecuteEvents.ExecuteHierarchy(Handler, data, swipeGestureHandler);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            int pointerId = eventData.pointerId;
            if (!swipeStates.ContainsKey(pointerId))
            {
                swipeStates.Add(pointerId, new SwipeState(eventData, Time.unscaledTime));
                SendSwipeEvent(pointerId);
            }
            else
            {
                if (swipeStates[pointerId].Phase == ESwipePhase.Start)
                {
                    return;
                }
                swipeStates.Remove(pointerId);

                swipeStates.Add(pointerId, new SwipeState(eventData, Time.unscaledTime));
                SendSwipeEvent(pointerId);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            int pointerId = eventData.pointerId;
            if (swipeStates.ContainsKey(pointerId))
            {
                swipeStates[pointerId].End(eventData);
                SendSwipeEvent(pointerId);

                swipeStates.Remove(pointerId);
            }
        }
    }
}
