
using UnityEngine.UI;

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

        protected override void OnEnable()
        {
            base.OnEnable();

            Slider[] sliders = GetComponentsInChildren<Slider>();
            foreach (Slider slider in sliders)
            {
                slider.value = 0.0f;
            }
        }
    }
}