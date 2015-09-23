using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class SpaceshipKeyboardController : MonoBehaviour
    {
        private SpaceshipController spaceshipController;       

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

        public void Start()
        {
            spaceshipController = new SpaceshipController(SpaceshipMoveable);
        }

        public void Update()
        {
            bool throttleUp = UnityEngine.Input.GetButtonUp("Throttle");
            bool throttleDown = UnityEngine.Input.GetButtonDown("Throttle");

            bool rotationClockwiseUp = UnityEngine.Input.GetButtonUp("RotationClockwise");
            bool rotationClockwiseDown = UnityEngine.Input.GetButtonDown("RotationClockwise");

            bool rotationCounterClockwiseUp = UnityEngine.Input.GetButtonUp("RotationCounterClockwise");
            bool rotationCounterClockwiseDown = UnityEngine.Input.GetButtonDown("RotationCounterClockwise");

            bool jump = UnityEngine.Input.GetButton("Jump");


            if (throttleDown)
            {
                spaceshipController.ChangeThrottle(1.0f);
            }
            else if (throttleUp)
            {
                spaceshipController.ChangeThrottle(0.0f);
            }

            if (rotationClockwiseDown)
            {
                spaceshipController.ClockwiseRotate(1.0f);
            }
            else if (rotationClockwiseUp)
            {
                spaceshipController.ClockwiseRotate(0.0f);
            }

            if (rotationCounterClockwiseDown)
            {
                spaceshipController.CounterClockwiseRotate(1.0f);
            }
            else if (rotationCounterClockwiseUp)
            {
                spaceshipController.CounterClockwiseRotate(0.0f);
            }

            if (jump)
            {
                spaceshipController.Stabilize();
            }                                   
        }
    }
}
