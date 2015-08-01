using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class LandscapePart : LandscapeBase
    {
        [SerializeField]
        private Renderer landscapeRenderer;

        public override Bounds Bounds
        {
            get
            {
                return landscapeRenderer != null ? landscapeRenderer.bounds : new Bounds();
            }
        }
    }
}