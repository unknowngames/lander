using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LandscapeTiler : MonoBehaviour
    {
        [SerializeField]
        private Landscape landscapeTemplate;

        private readonly List<Landscape> landscapes = new List<Landscape>();

        private Camera mainCamera;

        public void Awake ()
        {
            mainCamera = Camera.main;
        }

        public void Start ()
        {
            Landscape copyLeft = Instantiate(landscapeTemplate);
            Landscape copyRight = Instantiate(landscapeTemplate);
            copyLeft.transform.parent = transform;
            copyRight.transform.parent = transform;

            landscapes.Add(copyLeft);
            landscapes.Add(landscapeTemplate);
            landscapes.Add(copyRight);


            Vector3 position = landscapes[1].Position;

            Vector3 newPosition = new Vector3(position.x - landscapeTemplate.Bounds.size.x, position.y, position.z);

            landscapes[0].Position = newPosition;

            newPosition = new Vector3(position.x + landscapeTemplate.Bounds.size.x, position.y, position.z);

            landscapes[2].Position = newPosition;
        }

        private void TileToRight()
        {
            Landscape landscape = landscapes[0];
            landscapes.Remove(landscape);
            landscapes.Insert(2, landscape);

            Vector3 position = landscapes[1].Position;

            Vector3 newPosition = new Vector3(position.x + landscape.Bounds.size.x, position.y, position.z);
            landscape.Position = newPosition;
        }

        private void TileToLeft()
        {
            Landscape landscape = landscapes[2];
            landscapes.Remove(landscape);
            landscapes.Insert(0, landscape);

            Vector3 position = landscapes[1].Position;

            Vector3 newPosition = new Vector3(position.x - landscape.Bounds.size.x, position.y, position.z);
            landscape.Position = newPosition;
        }

        public void Update ()
        {
            ERelativePosition relativePosition = landscapes[1].GetRelativePosition(mainCamera);

            switch (relativePosition)
            {
                case ERelativePosition.Left:
                    TileToRight();
                    break;
                case ERelativePosition.Right:
                    TileToLeft();
                    break;
            }
        }
    }
}

