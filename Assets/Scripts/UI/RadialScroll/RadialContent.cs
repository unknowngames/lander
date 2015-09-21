using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.RadialScroll
{
    public class RadialContent : UIBehaviour, IScrollListContent
    {                                                                 
        public OnListItemClickEvent OnContentListItemClick = new OnListItemClickEvent();

        [SerializeField]
        private RectTransform container;
        [SerializeField]
        private float maxAngle;
        [SerializeField]
        private float spacing;
        [SerializeField]
        private float minRadius;

        private List<ListItem> items = new List<ListItem>();

        public float MaxAngle
        {
            get
            {
                float angle = maxAngle;

                while ((angle >= 360.0f) || (angle < 0.0f))
                {
                    if (angle >= 360.0f)
                    {
                        angle -= 360.0f;
                    }
                    else if (angle < 0.0f)
                    {
                        angle += 360.0f;
                    }
                }

                return angle;
            }
        }


        protected override void Start ()
        {
            base.Start();
            foreach (ListItem item in Items)
            {
                item.OnClick.AddListener(OnContentListItemOnClick);
            }
        }

        private void OnContentListItemOnClick(ListItem item)
        {
            OnContentListItemClick.Invoke(item);
        }

        public List<ListItem> Items
        {
            get
            {
                return items;
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public int Add(ListItem item)
        {
            item.transform.SetParent(container, false);
            items.Add(item);
            item.OnClick.AddListener(OnContentListItemOnClick);
            Rebuild();

            return items.IndexOf(item);
        }

        public void Remove(ListItem item)
        {
            items.RemoveAll(listItem => listItem.Button == item.Button);
            item.OnClick.RemoveListener(OnContentListItemOnClick);
            Rebuild();
        }

        public void RemoveAt(int index)
        {
            if (items.Count > index)
            {
                ListItem item = items[index];
                item.OnClick.RemoveListener(OnContentListItemOnClick);
                items.RemoveAt(index);
                Rebuild();
            }
        }

        public void Clear()
        {
            foreach (ListItem item in items)
            {
                item.OnClick.RemoveListener(OnContentListItemOnClick);
            }

            items.Clear ();
        }

        private void Rebuild()
        {
            int itemsCount = Items.Count;
            if (itemsCount == 0)
            {
                return;
            }

            float totalWidth = CalculateTotalWidth();   
            float radius = totalWidth*180.0f/(Mathf.PI*MaxAngle);

            radius = Mathf.Max(radius, minRadius);

            float radAngleStep = MaxAngle * Mathf.Deg2Rad/itemsCount;


            for (int index = 0; index < Items.Count; index++)
            {
                ListItem item = Items[index];

                float itemAngle = radAngleStep*index;
                float cosAngle = Mathf.Cos(itemAngle);
                float sinAngle = Mathf.Sin(itemAngle);

                item.RectTransform.anchoredPosition = new Vector2(cosAngle*radius, sinAngle*radius);
            }                                                         
            
            container.sizeDelta = new Vector2(2 * radius, 2 * radius);
            container.anchoredPosition = new Vector2(0, -radius);
        }

        private float CalculateTotalWidth()
        {
            float width = 0.0f;

            foreach (ListItem listItem in Items)
            {
                width += listItem.RectTransform.rect.width;
            }

            width += spacing*Items.Count;

            return width;
        }
    }
}
