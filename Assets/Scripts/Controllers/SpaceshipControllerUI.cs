using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;

namespace Assets.Scripts.Controllers
{
    public abstract class SpaceshipControllerUI : MenuUI
    {
        protected SpaceshipController SpaceshipController { get; private set; }        
   
        private ISpaceshipMoveable spaceshipMoveable;


        public ISpaceshipMoveable SpaceshipMoveable
        {
            get
            {
                return spaceshipMoveable ??
                       (spaceshipMoveable = GameHelper.PlayerSpaceship.GetComponent<ISpaceshipMoveable>());
            }
            set
            {
                spaceshipMoveable = value;
            }
        }

        public override void Show()
        {
            base.Show();
            SpaceshipController = new SpaceshipController(SpaceshipMoveable);
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