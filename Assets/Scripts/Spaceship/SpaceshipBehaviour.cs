using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    public class SpaceshipBehaviour : MonoBehaviour, ISpaceship
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Velosity { get; set; }
        public Vector3 AngularVelosity { get; set; }
        public float Mass { get; set; }
        public float RemainingFuel { get; set; }      
        public bool IsPaused { get; set; }
    }
}