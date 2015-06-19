using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{
    public class SpaceshipClassicControllerUI : SpaceshipControllerUI
    {
        public override ISpaceshipMoveable SpaceshipMoveable { get; set; }

        //Эта функция подписывается на UnityEvent от нужного контрола
        public override void OnRotationClockwiseChanged(bool state)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.RotateClockwiseButton = state;
            }
        }

        //Эта функция подписывается на UnityEvent от нужного контрола
        public override void OnRotationCounterClockwiseChanged(bool state)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.RotateCounterClockwiseButton = state;
            }
        }

        //Эта функция подписывается на UnityEvent от нужного контрола
        public override void OnThrottleChanged(float value)
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.ThrottleLevel = value;
            }
        }
    }
}
