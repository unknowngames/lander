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
            ListItem newItem = Instantiate(item);
            radialContent.Add(newItem);
        }

        public void OnGUI()
        {
            if (GUILayout.Button("Add"))
            {
                Add();
            }
        }
    }
}
