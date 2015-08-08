namespace Assets.Scripts.Interfaces
{
    public interface IGameDifficultyStorage
    {
        IGameDifficulty[] Difficulties { get; }
        IGameDifficulty this[string name] { get; }
        bool IsExist(string name);
    }
}