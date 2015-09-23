using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Input
{
    public class SwipeInputModule : StandaloneInputModule
    {
        private class SwipeData
        {
            public Vector2 Position { get; set; }
            public GameObject Handler { get; set; }
        }

        private static readonly ExecuteEvents.EventFunction<ISwipeEventHandler> SwipeHandler = Execute;

        private static void Execute(ISwipeEventHandler handler, BaseEventData eventData)
        {
            handler.OnSwipe(ExecuteEvents.ValidateEventData<SwipeEventData>(eventData));
        }

        private SwipeEventData swipeEventData;
        private Dictionary<int, SwipeData> pointersDictionary = new Dictionary<int, SwipeData>();

        public override void Process()
        {
            base.Process();
            ProcessSwipeEvent();
        }

        private void ProcessSwipeEvent()
        {
            MouseState mouseState = GetMousePointerEventData();
            MouseButtonEventData eventData = mouseState.GetButtonState(PointerEventData.InputButton.Left).eventData;

            ProcessPointer(eventData.buttonData, eventData.PressedThisFrame(), eventData.ReleasedThisFrame());

            for (int index = 0; index < UnityEngine.Input.touchCount; ++index)
            {
                bool pressed;
                bool released;
                PointerEventData pointerEventData = GetTouchPointerEventData(UnityEngine.Input.GetTouch(index), out pressed, out released);

                if (released)
                {
                    RemovePointerData(pointerEventData);
                }

                ProcessPointer(pointerEventData, pressed, released);
            }
        }

        private void ProcessPointer(PointerEventData pointer, bool pressed, bool released)
        {
            if (pressed && !pointersDictionary.ContainsKey(pointer.pointerId))
            {
                RaycastResult raycastResult = pointer.pointerCurrentRaycast;
                GameObject eventHandler = ExecuteEvents.GetEventHandler<ISwipeEventHandler>(raycastResult.gameObject);

                SwipeData data = new SwipeData
                {
                    Position = pointer.position,
                    Handler = eventHandler
                };
                pointersDictionary.Add(pointer.pointerId, data);
                return;
            }

            if (released)
            {
                pointersDictionary.Remove(pointer.pointerId);
                return;
            }

            foreach (KeyValuePair<int, SwipeData> pair in pointersDictionary)
            {
                int pointerId = pair.Key;
                SwipeData swipeData = pair.Value;

                if (pointerId == pointer.pointerId)
                {
                    Vector2 delta = pointer.position - swipeData.Position;

                    if (delta.sqrMagnitude > 0.6f)
                    {
                        ProcessSwipe(delta, swipeData.Handler);
                    }

                    pointersDictionary[pointer.pointerId].Position = pointer.position;
                    return;
                }
            }
        }

        private void ProcessSwipe(Vector2 delta, GameObject handler)
        {
            SwipeEventData eventData = GetSwipeEventData(delta.x, delta.y, 0.6f);
            ExecuteEvents.ExecuteHierarchy(handler, eventData, SwipeHandler);
        }

        protected virtual SwipeEventData GetSwipeEventData(float x, float y, float moveDeadZone)
        {
            if (swipeEventData == null)
            {
                swipeEventData = new SwipeEventData(eventSystem);
            }
            swipeEventData.Reset();
            swipeEventData.MoveVector = new Vector2(x, y);
            swipeEventData.MoveDir = DetermineMoveDirection(x, y, moveDeadZone);
            return swipeEventData;
        }
	}
}
