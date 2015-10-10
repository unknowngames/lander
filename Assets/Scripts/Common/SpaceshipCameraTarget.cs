using System.Linq;
using Assets.Scripts.Environment;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Spaceship;
using UnityEngine;

namespace Assets.Scripts.Common
{
    [RequireComponent(typeof(SpaceshipBehaviour))]
    public class SpaceshipCameraTarget : CameraTarget
    {
        [SerializeField]
        private bool isZoomed;
        [SerializeField]
        private SpaceshipBehaviour spaceshipBehaviour;

        [SerializeField]
        private float zoomHeight;

        [SerializeField]
        private float zoomDistanceToLandingPlace;

        private ISpaceship Spaceship
        {
            get
            {
                return spaceshipBehaviour ?? (spaceshipBehaviour = GetComponent<SpaceshipBehaviour>());
            }
        }

        private LandingZone[] landingZones;

        public void Start()
        {
            landingZones = FindObjectsOfType<LandingZone>();
        }

        public override bool DoZoom
        {
            get
            {
                return isZoomed || Spaceship.FlyHeight < zoomHeight || landingZones.Any(zone => Vector3.Distance(zone.transform.position, Position) < zoomDistanceToLandingPlace);
            }
        }
    }
}