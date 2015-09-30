using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;

namespace Assets.Scripts.Controllers
{
    public abstract class SpaceshipControllerUI : MenuUI
    {
        protected SpaceshipController SpaceshipController
        {
            get { return spaceshipController ?? (spaceshipController = new SpaceshipController(SpaceshipMoveable)); }
        }

        private ISpaceshipMoveable spaceshipMoveable;
        private SpaceshipController spaceshipController;


        public ISpaceshipMoveable SpaceshipMoveable
        {
            get
            {
                return spaceshipMoveable ??
                       (spaceshipMoveable = PlayerSpawner.PlayerSpaceship.GetComponent<ISpaceshipMoveable>());
            }
        }

        public void OnRotationClockwiseChanged(float force)
        {
            SpaceshipController.ClockwiseRotate(force);
        }

        public void OnRotationCounterClockwiseChanged(float force)
        {
            SpaceshipController.CounterClockwiseRotate(force);
        }

        public void OnThrottleChanged(float value)
        {
            SpaceshipController.ChangeThrottle(value);
        }
    }
}             