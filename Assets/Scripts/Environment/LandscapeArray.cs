using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class LandscapeArray : TwoDirectionalQueue<Landscape>
    {
        public Bounds Bounds { get; private set; }

        public Vector3 Center
        {
            get
            {
                return Bounds.center;
            }
        }

        public void RecalculateBounds ()
        {                                  
            if (Left != null)
            {
                Bounds b = Left.Bounds;
                foreach (Landscape landscape in this)
                {
                    b.Encapsulate(landscape.Bounds);
                }
                Bounds = b;
            }
            else
            {
                Bounds = new Bounds();
            }
        }

        public void TileToLeft()
        {
            Landscape landscape = PopRight();
            Landscape left = PeekLeft();

            PushLeft (landscape);

            Vector3 position = left.Position;

            Vector3 newPosition = new Vector3(position.x - landscape.Bounds.size.x, position.y, position.z);
            landscape.Position = newPosition;

            RecalculateBounds ();
        }

        public void TileToRight()
        {
            Landscape landscape = PopLeft ();
            Landscape right = PeekRight ();

            PushRight(landscape);

            Vector3 position = right.Position;

            Vector3 newPosition = new Vector3(position.x + landscape.Bounds.size.x, position.y, position.z);
            landscape.Position = newPosition;

            RecalculateBounds();
        }

        protected override void OnCollectionChanged ()
        {
            base.OnCollectionChanged ();
            RecalculateBounds ();
        }
    }
}