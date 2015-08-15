using System;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Spaceship;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [Serializable]
    public class ClassicGameDifficulty : IGameDifficulty
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private float gravitationMultiplyer;
        [SerializeField]
        private float atmosphericDragMultiplyer;
        [SerializeField]
        private bool hasAutoStabilize;

        private ClassicGameDifficulty(float gravitationMultiplyer, float atmosphericDragMultiplyer, bool hasAutoStabilize, string name)
        {
            GravitationMultiplyer = gravitationMultiplyer;
            AtmosphericDragMultiplyer = atmosphericDragMultiplyer;
            HasAutoStabilize = hasAutoStabilize;
            Name = name;
        }

        public float GravitationMultiplyer
        {
            get { return gravitationMultiplyer; }
            private set { gravitationMultiplyer = value; }
        }

        public float AtmosphericDragMultiplyer
        {
            get { return atmosphericDragMultiplyer; }
            private set { atmosphericDragMultiplyer = value; }
        }

        public bool HasAutoStabilize
        {
            get { return hasAutoStabilize; }
            private set { hasAutoStabilize = value; }
        }

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public void Apply(IGame game)
        {
            ILevelInfo levelInfo = game.LevelInfo;

            Physics.gravity = new Vector3(0, levelInfo.Gravitation * gravitationMultiplyer, 0);
            
            Rigidbody rigidbody = game.PlayerSpaceship.GetComponent<Rigidbody>();
            rigidbody.drag = levelInfo.AtmosphericDrag * AtmosphericDragMultiplyer;
            rigidbody.angularDrag = levelInfo.AtmosphericDrag * AtmosphericDragMultiplyer;

            SpaceshipMoveableBehaviour moveableBehaviour = game.PlayerSpaceship.GetComponent<SpaceshipMoveableBehaviour>();
            moveableBehaviour.AutoStabilize = hasAutoStabilize;
        }
    }
}