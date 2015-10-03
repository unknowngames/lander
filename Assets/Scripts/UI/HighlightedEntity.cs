using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class HighlightedEntity : MonoBehaviour
    {
        public abstract void Do();
        public abstract void Stop();
    }
}