using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{          
    public class SpaceshipController
    {
        private ISpaceshipMoveable spaceshipMoveable;

        public SpaceshipController(ISpaceshipMoveable spaceshipMoveable)
        {
            this.spaceshipMoveable = spaceshipMoveable;
        }

        public void ClockwiseRotate(float force)
        {
            if (spaceshipMoveable != null)
            {
                spaceshipMoveable.SetStabilizerThrottleLevel(-force);
            }
        }

        public void CounterClockwiseRotate(float force)
        {
            if (spaceshipMoveable != null)
            {
                spaceshipMoveable.SetStabilizerThrottleLevel(force);
            }
        }

        public void ChangeThrottle(float value)
        {
            if (spaceshipMoveable != null)
            {
                spaceshipMoveable.ThrottleLevel = value;
            }
        }

        public void Stabilize()
        {
            if (spaceshipMoveable != null)
            {
                spaceshipMoveable.RotationStabilize();
            }
        }
    }
}