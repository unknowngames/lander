using UnityEngine.EventSystems;

namespace Assets.Scripts.Input
{
    public interface IEndSwipeEventHandler : IEventSystemHandler
    {
        void OnSwipe(SwipeEventData data);
    }
}