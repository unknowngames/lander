namespace Assets.Scripts.Interfaces
{
    public interface ISpaceshipMoveable
    {
        bool AutoStabilize { get; set; }

        void RotationStabilize();
        void CancelRotationStabilize();

        void SetStabilizerThrottleLevel(float impulse);

        float ThrottleLevel { get; set; }
    }
}