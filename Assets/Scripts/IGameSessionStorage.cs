﻿namespace Assets.Scripts
{
    public interface IGameSessionStorage
    {
        bool HasSavedSession { get; }
        void RestoreSavedSession(IGame game);
        void SaveGameSession(IGame game);
        void RemoveSaveGame();
    }
}