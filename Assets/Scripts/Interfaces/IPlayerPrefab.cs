using Assets.Scripts.Spaceship;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IPlayerPrefab
    {
        string Key { get; }
        Sprite Preview { get; }
        string VisibleName { get; }
        SpaceshipBehaviour Prefab { get; }
    }
}