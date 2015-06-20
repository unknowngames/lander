using Assets.Scripts.Interfaces;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controllers
{
    public abstract class SpaceshipControllerUI : UIBehaviour
    {
        public ISpaceshipMoveable SpaceshipMoveable { get; set; }

        public void OnRotationClockwiseChanged (bool state)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.RotateClockwiseButton = state;
            }
        }

        public void OnRotationCounterClockwiseChanged (bool state)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.RotateCounterClockwiseButton = state;
            }
        }

        public void OnThrottleChanged (float value)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.ThrottleLevel = value;
            }
        }
    }
}