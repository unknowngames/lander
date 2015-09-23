using UnityEngine.EventSystems;

namespace Assets.Scripts.Input
{
    public interface IBeginSwipeEventHandler : IEventSystemHandler
    {
        void OnSwipe(SwipeEventData data);
    }
}