using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SpacechipHighlight : HighlightedEntity
    {
        [SerializeField]
        private CameraSmoothFolower smoothFolower;

        public override void Do()
        {
            smoothFolower.DoZoom();
        }

        public override void Stop()
        {
            smoothFolower.UndoZoom();
        }
    }
}