namespace Assets.Scripts.Interfaces
{
    public interface IGameDifficultyStorage
    {
        IGameDifficulty[] Difficulties { get; }
        IGameDifficulty this[string name] { get; }
        IGameDifficulty this[int index] { get; }
        bool IsExist(string name);

        int GetIndex(string name);
        int GetIndex(IGameDifficulty difficulty);

        int DifficultiesCount { get; }
    }
}