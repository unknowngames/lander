namespace Assets.Scripts.Interfaces
{
    public interface IGameSessionStorage
    {
        IGameSession Current { get; }
        bool HasSavedSession { get; }
        void RestoreSavedSession(IFlight flight);
        void SaveGameSession(IFlight flight);
        void RemoveSavedGame();
    }
}