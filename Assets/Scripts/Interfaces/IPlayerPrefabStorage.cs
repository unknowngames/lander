namespace Assets.Scripts.Interfaces
{
    public interface IPlayerPrefabStorage
    {
        IPlayerPrefab[] Prefabs { get; }
        IPlayerPrefab this[string key] { get; }
        bool IsExist(string key);
    }
}