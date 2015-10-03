using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ObjectHighlight : HighlightedEntity
    {
        [SerializeField]
        private CameraSmoothFolower smoothFolower;

        public override void Do()
        {
            smoothFolower.Target = transform;
        }

        public override void Stop()
        {
            smoothFolower.Target = null;
        }
    }
}