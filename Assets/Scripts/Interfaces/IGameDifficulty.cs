namespace Assets.Scripts.Interfaces
{
    public interface IGameDifficulty
    {
        string Name { get; }
        void Apply(IGame game);
    }
}
