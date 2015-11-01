namespace Assets.Scripts.Interfaces
{
    public interface IGameSessionStorage
    {
        IGameSession Current { get; }
        bool HasSavedSession { get; }
        void RestoreSavedSession(IFlight flight, string difficultyName);
        void SaveGameSession(IFlight flight);
        void RemoveSavedGame();
    }
}