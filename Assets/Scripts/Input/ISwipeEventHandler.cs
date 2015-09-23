using UnityEngine.EventSystems;

namespace Assets.Scripts.Input
{
    public interface ISwipeEventHandler : IEventSystemHandler
    {
        void OnSwipe(SwipeEventData data);
    }
}
