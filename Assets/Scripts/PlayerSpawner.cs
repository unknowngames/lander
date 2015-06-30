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

        [SerializeField]
        private Transform spawnPosition;

        private SpaceshipBehaviour spaceshipBehaviourInstance;

        public SpaceshipBehaviour CreatePlayer()
        {
            if (spaceshipBehaviourInstance == null)
            {
                spaceshipBehaviourInstance = UnityEngine.Object.Instantiate(spaceshipBehaviourPrefab);            
            }
            
            spaceshipBehaviourInstance.Reset ();

            spaceshipBehaviourInstance.transform.position = spawnPosition.position;
            spaceshipBehaviourInstance.transform.rotation = spawnPosition.rotation;
            
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