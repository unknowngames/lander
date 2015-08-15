using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class PlayerPrefabsStorage : ScriptableObject, IPlayerPrefabStorage
    {
        [SerializeField]
        private PlayerPrefab[] prefabs;

        public IPlayerPrefab[] Prefabs
        {
            get { return prefabs.Cast<IPlayerPrefab>().ToArray(); }
        }

        public IPlayerPrefab this[string key]
        {
            get
            {
                foreach (PlayerPrefab prefab in prefabs)
                {
                    if (prefab.Key.Equals(key))
                    {
                        return prefab;
                    }
                }
                throw new KeyNotFoundException();
            }
        }

        public bool IsExist(string key)
        {
            foreach (PlayerPrefab prefab in prefabs)
            {
                if (prefab.Key.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}