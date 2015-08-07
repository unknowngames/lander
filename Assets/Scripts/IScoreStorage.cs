namespace Assets.Scripts
{
    public interface IScoreStorage
    {
        GameScore BestScore { get; }
        GameScore[] All { get; }
        void AddGameScore(GameScore gameScore);
    }
}