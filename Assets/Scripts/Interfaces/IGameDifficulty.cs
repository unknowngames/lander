using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IGameDifficulty
    {
        string Name { get; }
        Color ColorCode { get; }
        void Apply(IGame game);
    }
}
