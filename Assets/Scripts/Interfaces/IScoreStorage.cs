using Assets.Scripts.Session;

namespace Assets.Scripts.Interfaces
{
    public interface IScoreStorage
    {
        GameScore BestScore { get; }
        GameScore[] All { get; }
        void AddGameScore(GameScore gameScore);
    }
}