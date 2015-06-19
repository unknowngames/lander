using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Spaceship
{
    [RequireComponent(typeof(SpaceshipBehaviour))]
    public class SpaceshipMoveableBehaviour : MonoBehaviour, ISpaceshipMoveable
    {
        [SerializeField] private ISpaceship spaceship;

        public ISpaceship Spaceship
        {
            get { return spaceship ?? (spaceship = GetComponent<ISpaceship>()); }
        }

        public bool RotateClockwiseButton { get; set; }
        public bool RotateCounterClockwiseButton { get; set; }
        public float ThrottleLevel { get; set; }
    }
}