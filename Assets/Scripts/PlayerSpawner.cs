using System;
using Assets.Scripts.Environment;
using Assets.Scripts.Spaceship;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerSpawner : MonoBehaviour
    {
        public static PlayerSpawner Current { get; private set; }

        public static SpaceshipBehaviour PlayerSpaceship
        {
            get { return Current.spaceshipBehaviourInstance; }
        }

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
            int direction = UnityEngine.Random.Range(0, 1) == 0 ? 1 : -1;

            randomDirection *= playerStartImpulsePower * UnityEngine.Random.Range(1.0f, 3.0f) * direction;

            return CreatePlayer(randomDirection);
        }

        public SpaceshipBehaviour CreatePlayer(Vector3 initialVelosity)
        {
            if (PlayerSpaceship == null)
            {
                spaceshipBehaviourInstance = Instantiate(spaceshipBehaviourPrefab);            
            }
            
            PlayerSpaceship.Reset ();

            PlayerSpaceship.transform.position = spawnZone.GetPosition();

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, -initialVelosity);  
            PlayerSpaceship.transform.rotation = rotation;
            
            PlayerSpaceship.gameObject.SetActive (true);
            
			PlayerSpaceship.SetVelocity(initialVelosity);

            return PlayerSpaceship;
        }

        public void RemovePlayer()
        {
            if (PlayerSpaceship != null)
            {                
                PlayerSpaceship.gameObject.SetActive(false);
            }
        }

        public void DestroyPlayer()
        {
            if (PlayerSpaceship != null)
            {
                UnityEngine.Object.Destroy (PlayerSpaceship.gameObject);
            }
        }

        protected void OnEnable()
        {
            if (Current == null)
            {
                Current = this;
            }
            else
            {
                Debug.LogWarning("Multiple PlayerSpawner in scene... this is not supported");
            }
        }

        protected void OnDisable()
        {
            if (Current == this)
            {
                Current = null;
            }
        }
    }
}