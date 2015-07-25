namespace Assets.Scripts.Interfaces
{
    public interface ISpaceshipMoveable
    {
        void RotationStabilize();

        void SetStabilizerThrottleLevel(float impulse);

        float ThrottleLevel { get; set; }
    }
}