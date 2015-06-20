using System;
using Assets.Scripts.Spaceship;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerSpawner
    {
        [SerializeField]
        private SpaceshipBehaviour spaceshipBehaviourPrefab;
        private SpaceshipBehaviour spaceshipBehaviourInstance;

        public SpaceshipBehaviour CreatePlayer()
        {
            if (spaceshipBehaviourInstance == null)
            {
                spaceshipBehaviourInstance = UnityEngine.Object.Instantiate (spaceshipBehaviourPrefab);
            }
            spaceshipBehaviourInstance.gameObject.SetActive (true);
            return spaceshipBehaviourInstance;
        }

        public void RemovePlayer()
        {
            if (spaceshipBehaviourInstance != null)
            {
                spaceshipBehaviourInstance.gameObject.SetActive(false);
            }
        }

        public void DestroyPlayer()
        {
            if (spaceshipBehaviourInstance != null)
            {
                UnityEngine.Object.Destroy (spaceshipBehaviourInstance.gameObject);
            }
        }
    }
}