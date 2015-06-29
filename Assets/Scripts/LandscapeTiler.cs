using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LandscapeArray
    {
        private List<Landscape> landscapes = new List<Landscape> ();
        private Bounds bounds;

        public Bounds Bounds
        {
            get
            {
                return bounds;
            }
        }

        public LandscapeArray ()
        {
            bounds = new Bounds();
        }

        public void Add (Landscape landscape)
        {
            if (!landscapes.Contains (landscape))
            {
                landscapes.Add(landscape);
                ResetBounds();
            }
        }

        public void Remove (Landscape landscape)
        {
            if (landscapes.Contains(landscape))
            {
                landscapes.Remove(landscape);
                ResetBounds ();
            }
        }

        private void ResetBounds ()
        {   
            Bounds b = new Bounds();
            foreach (Landscape landscape in landscapes)
            {
                b.Encapsulate (landscape.Bounds);
            }
        }
    }

    public class LandscapeTiler : MonoBehaviour
    {
        private LandscapeArray allLandscapes = new LandscapeArray ();
        private List<Landscape> visibleLandscapes = new List<Landscape> ();
        private List<Landscape> cachedLandscapes = new List<Landscape> ();

        private Camera mainCamera;

        public void Awake ()
        {
            mainCamera = Camera.main;
        }

        public void Start ()
        {
            FillVisibleLandscapeAtStart ();
        }

        private void FillVisibleLandscapeAtStart ()
        {
            Landscape[] landscapes = GetComponentsInChildren<Landscape>();
            visibleLandscapes.Clear ();
            foreach (Landscape landscape in landscapes)
            {
                allLandscapes.Add (landscape);

                if (landscape.IsInViewport (mainCamera))
                {
                    visibleLandscapes.Add (landscape);
                }
            }
        }

        public void Update ()
        {
        }
    }
}

