using System.Collections;
using UnityEngine;

public class TouchdownTrigger : MonoBehaviour
{
    [SerializeField]
    private Trigger[] triggers;  
    [SerializeField]
    private float touchdownTimeout = 2.0f;

    public bool Landed = false;

    private Coroutine touchdownCoroutine;
    private bool isTouchdownTimerStarted = false;

    private bool IsAllTriggered ()
    {
        bool result = true;

        foreach (Trigger trigger in triggers)
        {
            result = result && trigger.IsTriggered;
        }

        return result;
    }

	public void Update ()
    {
        if (IsAllTriggered() && !isTouchdownTimerStarted)
	    {
	        BeginTouchdownTimer ();
	    }
        else if (!IsAllTriggered())
	    {
	        Landed = false;
	        StopTouchdownTimer ();
	    }
	}

    private void BeginTouchdownTimer()
    {
        StopTouchdownTimer ();
        isTouchdownTimerStarted = true;
        touchdownCoroutine = StartCoroutine(TouchdownCoroutine());
    }

    private void StopTouchdownTimer ()
    {
        if (touchdownCoroutine != null)
        {
            isTouchdownTimerStarted = false;
            StopCoroutine (touchdownCoroutine);
        }
    }

    private IEnumerator TouchdownCoroutine()
    {
        yield return new WaitForSeconds (touchdownTimeout);
        Landed = true;
        StopTouchdownTimer ();
    }
}
