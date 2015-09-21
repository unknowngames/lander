using System.Collections.Generic;

namespace Assets.Scripts.UI.RadialScroll
{
    public interface IScrollListContent
    {
        List<ListItem> Items { get; }
        int Count { get; }
        int Add(ListItem item);
        void Remove(ListItem item);
        void RemoveAt(int index);
        void Clear();
    }
}