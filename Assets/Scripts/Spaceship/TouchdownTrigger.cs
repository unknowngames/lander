using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Environment;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class TouchdownTrigger : MonoBehaviour
    {
        [SerializeField]
        private Trigger[] triggers;  
        [SerializeField]
        private float touchdownTimeout = 2.0f;

        public bool Landed = false;
        public LandingZone Zone { get; private set; }

        private Coroutine touchdownCoroutine;
        private bool isTouchdownTimerStarted = false;

        private bool IsAllTriggered ()
        {
            bool result = true;

            foreach (Trigger trigger in triggers)
            {
                result = result && trigger.IsTriggered;
            }

            if (result)
            {
                List<GameObject> objects = new List<GameObject>(triggers[0].Objects);

                foreach (Trigger trigger in triggers)
                {
                    objects = new List<GameObject>(objects.Intersect(trigger.Objects));
                }

                if (objects.Count != 1)
                {
                    result = false;
                }

                if (result)
                {
                    Zone = objects[0].GetComponent<LandingZone>();

                    result = (Zone!=null);
                }
            }

            return result;
        }

        public void Update ()
        {
            bool isAllTriggered = IsAllTriggered();

            if (isAllTriggered && !isTouchdownTimerStarted)
            {
                BeginTouchdownTimer ();
            }
            else if (!isAllTriggered)
            {
                Landed = false;
                Zone = null;    
                StopTouchdownTimer ();
            }
        }

        public void Reset()
        {
            Landed = false;
            Zone = null;
            StopTouchdownTimer();

            foreach (Trigger trigger in triggers)
            {
                trigger.Reset();
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
            StopTouchdownTimer ();
            ProcessLandedEvent();
        }

        private void ProcessLandedEvent()
        {
            Landed = true; 
        }
    }
}
