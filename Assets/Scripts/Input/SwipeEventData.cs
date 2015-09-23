using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Input
{
    public class SwipeEventData : BaseEventData
    {
        public Vector2 MoveVector { get; set; }
        public MoveDirection MoveDir { get; set; }

        public SwipeEventData(EventSystem eventSystem) : base(eventSystem)
        {
        }
    }
}