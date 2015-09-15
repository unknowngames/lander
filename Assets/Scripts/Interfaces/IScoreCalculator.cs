﻿namespace Assets.Scripts.Interfaces
{
    public interface IScoreCalculator
    {
        IGameScore Current { get; }
        void SetInitialScore(IGameSession gameScore);
		void Begin();
        void Update();
        IGameSession Calculate();
    }
}