using Assets.Scripts;
using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;
using UnityEngine.EventSystems;
using UnityEngine;

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

        protected override void Start()
        {
            base.Start();
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