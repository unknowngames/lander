using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool IsTriggered=false;

    private bool isHereAnybody;
   
    public void OnTriggerStay(Collider other)
    {
        isHereAnybody = true;
    }

    public void Update()
    {
        IsTriggered = isHereAnybody;
        isHereAnybody = false;
    }
}
