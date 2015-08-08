namespace Assets.Scripts.Interfaces
{
    public interface IScoreCalculator
    {
        IGameScore Current { get; }
        void SetInitialScore(IGameScore gameScore);
        void Update();
        IGameScore Calculate();
    }
}