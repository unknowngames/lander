namespace Assets.Scripts.Interfaces
{
    public interface ISpaceshipMoveable
    {
        void SetImpulse(float impulse);

        float ThrottleLevel { get; set; }
    }
}