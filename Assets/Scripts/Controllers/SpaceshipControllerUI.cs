using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;

namespace Assets.Scripts.Controllers
{
    public abstract class SpaceshipControllerUI : MenuUI
    {
        private ISpaceshipMoveable spaceshipMoveable;

        public ISpaceshipMoveable SpaceshipMoveable
        {
            get
            {
                return spaceshipMoveable ??
                       (spaceshipMoveable = Game.PlayerSpaceship.GetComponent<ISpaceshipMoveable> ());
            }
            set
            {
                spaceshipMoveable = value;
            }
        }

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