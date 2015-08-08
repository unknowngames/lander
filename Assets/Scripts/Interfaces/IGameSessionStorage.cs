namespace Assets.Scripts.Interfaces
{
    public interface IGameSessionStorage
    {
        IGameSession Current { get; }
        bool HasSavedSession { get; }
        void RestoreSavedSession(IGame game);
        void SaveGameSession(IGame game);
        void RemoveSaveGame();
    }
}