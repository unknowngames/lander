
namespace Assets.Scripts.Controllers
{
    public class SpaceshipClassicControllerUI : SpaceshipControllerUI
    {              
        public void OnStabilizeClick()
        {
            if (SpaceshipMoveable != null)
            {
                SpaceshipMoveable.RotationStabilize ();
            }
        }
    }
}