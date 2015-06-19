namespace Assets.Scripts.Interfaces
{
    public interface ISpaceshipMoveable
    {
        bool RotateClockwiseButton { get; set; }
        bool RotateCounterClockwiseButton { get; set; }
        float ThrottleLevel { get; set; }
    }
}