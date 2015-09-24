using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.RadialScroll
{
    public interface ISwipeGesture : IEventSystemHandler
    {
        void OnSwipe(SwipeGestureData swipeGestureData);
    }
}