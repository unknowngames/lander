using System;

namespace Assets.Scripts
{
    public interface IGameScore
    {
        int ScorePoints { get; }

        int LandingsCount { get; }
    }
}