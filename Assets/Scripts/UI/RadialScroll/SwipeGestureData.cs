using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.RadialScroll
{
    public class SwipeGestureData : BaseEventData
    {
        public SwipeGestureData(EventSystem eventSystem) : base(eventSystem)
        {
        }

        public MoveDirection Direction { get; set; }
        public ESwipePhase Phase { get; set; }
        public float Distance { get; set; }
        public float DistanceDelta { get; set; }
        public float Duration { get; set; }
        public Vector2 Position { get; set; }
    }
}