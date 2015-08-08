namespace Assets.Scripts.Interfaces
{
    public interface IGameSessionStorage
    {
        IGameSession Current { get; }
        bool HasSavedSession { get; }
        void RestoreSavedSession();
        void SaveGameSession();
        void RemoveSaveGame();
    }
}