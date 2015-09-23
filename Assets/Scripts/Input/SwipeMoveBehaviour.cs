using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Input
{
	public class SwipeMoveBehaviour : UIBehaviour, ISwipeEventHandler
    {
        [SerializeField]
        private float brakeForce = 0.1f;

        private float lerpPosition = 0.1f;
        private Vector2 anchoredPosition = Vector2.zero;
        private Vector2 velocity = Vector2.zero;

	    public void OnSwipe(SwipeEventData data)
	    {
            anchoredPosition = ((RectTransform)transform).anchoredPosition + data.MoveVector;
            velocity = data.MoveVector;
	        lerpPosition = 1.0f;
	    }

	    public void Update()
	    {
	        RectTransform rectTransform = ((RectTransform) transform);
            rectTransform.anchoredPosition = anchoredPosition;

	        if (lerpPosition > 0.0f)
	        {
	            lerpPosition -= brakeForce*Time.deltaTime;
	        }
	        else
	        {
	            lerpPosition = 0.0f;
	        }
	    }
    }
}
