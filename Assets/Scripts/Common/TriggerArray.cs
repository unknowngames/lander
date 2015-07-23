using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class TriggerArray : MonoBehaviour
    {
        [SerializeField]
        private Trigger[] triggers;
        public bool IsTriggered { get; private set; }
        public GameObject[] Objects { get; private set; }

        private bool IsAllTriggered()
        {
            bool result = false;

            foreach (Trigger trigger in triggers)
            {
                result = result || trigger.IsTriggered;
            }

            if (result)
            {
                List<GameObject> objects = new List<GameObject>();

                foreach (Trigger trigger in triggers)
                {
                    objects.AddRange(trigger.Objects);
                }
            }

            return result;
        }
        
        public void Update ()
        {
            if (IsAllTriggered())
            {
                IsTriggered = true;
            }
            else
            {
                IsTriggered = false;
            }
        }

        public void Reset()
        {
            foreach (Trigger trigger in triggers)
            {
                trigger.Reset();
            }
        }
    }
}
