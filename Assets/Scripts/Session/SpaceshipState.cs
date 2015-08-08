using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Session
{
    public class SpaceshipState : ISpaceshipState
    {
        private SpaceshipState(float remainingFuel)
        {
            RemainingFuel = remainingFuel;
        }

        public float RemainingFuel { get; private set; }

        public static SpaceshipState Create(float remainingFuel)
        {
            return new SpaceshipState(remainingFuel);
        }
    }
}