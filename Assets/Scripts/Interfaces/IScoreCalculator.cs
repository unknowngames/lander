namespace Assets.Scripts.Interfaces
{
    public interface IScoreCalculator
    {
        void SetInitialScore(IGameScore gameScore);
        void Update();
        IGameScore Calculate();
    }
}