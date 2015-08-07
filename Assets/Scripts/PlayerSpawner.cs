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

        [SerializeField]
        private float playerStartImpulsePower = 500;

        private SpaceshipBehaviour spaceshipBehaviourInstance;

        public SpaceshipBehaviour CreatePlayerAndRandomMove()
        {
            Vector3 randomDirection = Vector3.right;
            randomDirection *= UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
            randomDirection *= playerStartImpulsePower;

            return CreatePlayer(randomDirection);
        }

        public SpaceshipBehaviour CreatePlayer(Vector3 initialVelosity)
        {
            if (spaceshipBehaviourInstance == null)
            {
                spaceshipBehaviourInstance = UnityEngine.Object.Instantiate(spaceshipBehaviourPrefab);            
            }
            
            spaceshipBehaviourInstance.Reset ();

            spaceshipBehaviourInstance.transform.position = spawnPosition.position;
            spaceshipBehaviourInstance.transform.rotation = spawnPosition.rotation;
            
            spaceshipBehaviourInstance.gameObject.SetActive (true);
            
			spaceshipBehaviourInstance.SetVelocity(initialVelosity);

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