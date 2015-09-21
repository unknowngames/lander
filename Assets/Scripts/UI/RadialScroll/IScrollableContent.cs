using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Assets.Scripts.UI.RadialScroll
{
    public interface IScrollList
    {
        List<ListItem> Items { get; }
        int Count { get; }
        int Add(ListItem item);
        void Remove(Object value);
        void RemoveAt(int index);
        void Clear();
    }
}