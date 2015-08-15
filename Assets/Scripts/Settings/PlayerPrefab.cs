using System;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Spaceship;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class PlayerPrefab : IPlayerPrefab
    {                      
        [SerializeField]
        private string key;

        [SerializeField]
        private Sprite preview;

        [SerializeField]
        private string visibleName;

        [SerializeField]
        private SpaceshipBehaviour prefab;

        public string Key
        {
            get { return key; }
        }

        public Sprite Preview
        {
            get { return Preview; }
        }

        public string VisibleName
        {
            get { return VisibleName; }
        }

        public SpaceshipBehaviour Prefab
        {
            get { return prefab; }
        }
    }
}