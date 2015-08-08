using System;
using Assets.Scripts.Environment;
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
        private SpawnZone spawnZone;

        [SerializeField]
        private float playerStartImpulsePower = 500;

        private SpaceshipBehaviour spaceshipBehaviourInstance;

        public SpaceshipBehaviour CreatePlayerAndRandomMove()
        {
            Vector3 randomDirection = Vector3.right;
            randomDirection *= playerStartImpulsePower*UnityEngine.Random.Range(-2, 2);

            return CreatePlayer(randomDirection);
        }

        public SpaceshipBehaviour CreatePlayer(Vector3 initialVelosity)
        {
            if (spaceshipBehaviourInstance == null)
            {
                spaceshipBehaviourInstance = UnityEngine.Object.Instantiate(spaceshipBehaviourPrefab);            
            }
            
            spaceshipBehaviourInstance.Reset ();

            spaceshipBehaviourInstance.transform.position = spawnZone.GetRandomPosition();


            spaceshipBehaviourInstance.transform.rotation = Quaternion.identity;
            
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