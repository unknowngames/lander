using Assets.Scripts.Interfaces;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public abstract class SpaceshipControllerUI : UIBehaviour
    {
        public ISpaceshipMoveable SpaceshipMoveable { get; set; }

        public void OnRotationClockwiseChanged(float force)
        {
            Debug.Log("Clockwise " + force);
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.SetImpulse(force);
            }
        }

        public void OnRotationCounterClockwiseChanged(float force)
        {
            Debug.Log("Counter clockwise " + force);
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.SetImpulse(force);
            }
        }

        public void OnThrottleChanged(float value)
        {
            Debug.Log("Throttle " + value);
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.ThrottleLevel = value;
            }
        }
    }
}