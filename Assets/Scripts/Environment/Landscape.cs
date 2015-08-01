using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Landscape : LandscapeBase
    {
        [SerializeField]
        private LandscapePart[] landscapeParts;

        public override Bounds Bounds
        {
            get
            {
                if (landscapeParts != null && landscapeParts.Length > 0)
                {
                    Bounds b = landscapeParts[0].Bounds;

                    foreach (LandscapePart part in landscapeParts)
                    {
                        b.Encapsulate(part.Bounds);
                    }
                    return b;
                }
                return new Bounds();
            }
        }
    }
}