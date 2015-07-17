using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool IsTriggered=false;

    public void OnTriggerEnter(Collider other)
    {
        IsTriggered = true;
    }

    public void OnTriggerExit(Collider other)
    {
        IsTriggered = false;
    }

    public void OnTriggerStay(Collider other)
    {

    }  
}
