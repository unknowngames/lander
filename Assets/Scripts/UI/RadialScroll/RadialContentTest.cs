using UnityEngine;

namespace Assets.Scripts.UI.RadialScroll
{
    public class RadialContentTest : MonoBehaviour
    {
        [SerializeField]
        private RadialContent radialContent;

        [SerializeField]
        private ListItem item;

        [ContextMenu("Add")]
        public void Add()
        {
            for (int i = 0; i < 10; i++)
            {
                ListItem newItem = Instantiate(item);
                radialContent.Add(newItem);
            }
        }
    }
}
