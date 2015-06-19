using Assets.Scripts.Interfaces;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controllers
{
    public abstract class SpaceshipControllerUI : UIBehaviour
    {
        public abstract ISpaceshipMoveable SpaceshipMoveable { get; set; }
        public abstract void OnRotationClockwiseChanged(bool state);
        public abstract void OnRotationCounterClockwiseChanged(bool state);
        public abstract void OnThrottleChanged(float value);
    }
}