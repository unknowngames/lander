namespace Assets.Scripts.Interfaces
{
    public interface ISpaceshipMoveable
    {
        bool AutoStabilize { get; set; }

        void RotationStabilize();

        void SetStabilizerThrottleLevel(float impulse);

        float ThrottleLevel { get; set; }
    }
}