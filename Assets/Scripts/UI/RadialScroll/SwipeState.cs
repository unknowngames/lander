using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.RadialScroll
{
    public class SwipeState
    {
        public int PointerId { get; set; }
        public ESwipePhase Phase { get; set; }
        private Vector2 Position { get; set; }
        private Vector2 StartPosition { get; set; }
        private BaseEventData LastEventData { get; set; }
        private float Duration { get; set; }
        private float Distance { get; set; }
        private float DistanceDelta { get; set; }

        public SwipeState(PointerEventData pointerData, float startTime)
        {
            PointerId = pointerData.pointerId;
            Phase = ESwipePhase.Start;
            StartPosition = pointerData.position;
            Position = pointerData.position;
            Duration = 0.0f;
            Distance = 0.0f;
            DistanceDelta = 0.0f;
            LastEventData = pointerData;
        }

        public void Move(PointerEventData pointerData, float deltaTime)
        {
            Phase = ESwipePhase.Move;

            Duration += deltaTime;
            Distance = (Position - StartPosition).magnitude;
            DistanceDelta = (Position - pointerData.position).magnitude;
            Position = pointerData.position;

            LastEventData = pointerData;
        }

        public void End(PointerEventData pointerData)
        {
            Phase = ESwipePhase.End;

            LastEventData = pointerData;
        }

        public void Cancel(PointerEventData pointerData)
        {
            Phase = ESwipePhase.Cancel;

            LastEventData = pointerData;
        }

        public SwipeGestureData GetSwipeGestureData()
        {
            SwipeGestureData data = new SwipeGestureData(EventSystem.current);

            data.Direction = DetermineMoveDirection(Position - StartPosition);
            data.Distance = Distance;
            data.DistanceDelta = DistanceDelta;
            data.Duration = Duration;
            data.Phase = Phase;
            data.Position = Position;

            return data;
        }

        private static MoveDirection DetermineMoveDirection(Vector2 vector2)
        {
            return DetermineMoveDirection(vector2, 0.6f);
        }

        private static MoveDirection DetermineMoveDirection(Vector2 vector2, float deadZone)
        {
            if (vector2.sqrMagnitude < deadZone * deadZone)
            {
                return MoveDirection.None;
            }
            if (Mathf.Abs(vector2.x) > (double)Mathf.Abs(vector2.y))
            {
                return vector2.x > 0.0f ? MoveDirection.Right : MoveDirection.Left;
            }
            return vector2.y > 0.0f ? MoveDirection.Up : MoveDirection.Down;
        }
    }
}