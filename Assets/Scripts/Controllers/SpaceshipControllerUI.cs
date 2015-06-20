using Assets.Scripts;
using Assets.Scripts.Interfaces;
using Assets.Scripts.UI;
using UnityEngine.EventSystems;
using UnityEngine;

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

        public void OnRotationClockwiseChanged(float force)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.SetImpulse(force);
            }
        }

        public void OnRotationCounterClockwiseChanged(float force)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.SetImpulse(force);
            }
        }

        public void OnThrottleChanged(float value)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.ThrottleLevel = value;
            }
        }
    }
}             