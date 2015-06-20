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

        public void SetImpulse(float impulse)
        {

        }

        public float ThrottleLevel { get; set; }
    }
}