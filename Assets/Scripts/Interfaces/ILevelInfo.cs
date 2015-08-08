namespace Assets.Scripts.Interfaces
{
    public interface ILevelInfo
    {
        string Name { get; }
        float Gravitation { get; }
        float AtmosphericDrag { get; }
    }
}