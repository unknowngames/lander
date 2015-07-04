namespace Assets.Scripts.Interfaces
{
    public interface ISpaceshipMoveable
    {
        void RotationStabilize();

        void SetImpulse(float impulse);

        float ThrottleLevel { get; set; }
    }
}