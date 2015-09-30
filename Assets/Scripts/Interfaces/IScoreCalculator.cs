namespace Assets.Scripts.Interfaces
{
    public interface IScoreCalculator
    {
        IGameScore CurrentScore { get; }
        void SetInitialScore(IGameSession gameScore);
		void Begin();
        void Update();
        IGameSession Calculate();
    }
}