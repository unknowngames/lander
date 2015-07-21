using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool IsTriggered=false;
    private List<GameObject> objects = new List<GameObject>(10);

    public ReadOnlyCollection<GameObject> Objects
    {
        get { return new ReadOnlyCollection<GameObject>(objects); }
    }

    public void OnTriggerStay(Collider other)
    {
        objects.Add(other.gameObject);
    }

    public void FixedUpdate()
    {
        IsTriggered = (Objects.Count!=0);
        objects.Clear();
    }

    public void Reset()
    {
        IsTriggered = false;
        objects.Clear();
    }
}
