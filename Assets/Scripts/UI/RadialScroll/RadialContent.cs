using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.RadialScroll
{
    public class RadialContent : UIBehaviour, IScrollListContent, IBeginDragHandler, IDragHandler
    {                                                                 
        public OnListItemClickEvent OnContentListItemClick = new OnListItemClickEvent();

        [SerializeField]
        private RectTransform container;
        [SerializeField]
        private float maxAngle;
        [SerializeField]
        private float startAngle;
        [SerializeField]
        private float spacing;
        [SerializeField]
        private float minRadius;

        [SerializeField]
        private float minSwipeAngle;
        [SerializeField]
        private float maxSwipeAngle;

        private float currentAngle;
        private Vector2 position;
        private Vector2 center;
        private List<ListItem> items = new List<ListItem>();
        private bool swipeDone = false;

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
            float angleStep;
            if (itemsCount == 1)
            {
                angleStep = 0.0f;
            }
            else
            {
                angleStep = MaxAngle/(itemsCount - 1);
            }
            float radius = totalWidth*180.0f/(Mathf.PI*MaxAngle);

            if (radius < minRadius)
            {
                radius = minRadius;
            }

            float alpha = 2*Mathf.Asin(totalWidth/(2*itemsCount*radius))*Mathf.Rad2Deg;
            if (alpha > startAngle)
            {
                alpha = 0.0f;
            }
            
            for (int index = 0; index < Items.Count; index++)
            {
                ListItem item = Items[index];

                float itemAngle = (-angleStep * index + startAngle);
                float radItemAngle = itemAngle*Mathf.Deg2Rad;
                float cosAngle = Mathf.Cos(radItemAngle);
                float sinAngle = Mathf.Sin(radItemAngle);

                item.RectTransform.anchoredPosition = new Vector2(cosAngle * radius, sinAngle * radius);
                item.RectTransform.localEulerAngles = new Vector3(0.0f, 0.0f, itemAngle-MaxAngle);
            }                                                         
            
            container.sizeDelta = new Vector2(2 * radius, 2 * radius);
            container.anchoredPosition = new Vector2(0, -radius);
            if (!swipeDone)
            {
                container.localEulerAngles = new Vector3(0.0f, 0.0f, alpha);
            }

            minSwipeAngle = alpha;
            maxSwipeAngle = MaxAngle - alpha;
        }

        private float CalculateTotalWidth()
        {
            float width = 0.0f;

            foreach (ListItem listItem in Items)
            {
                width += listItem.RectTransform.rect.width;
            }

            width += spacing*(Items.Count);

            return width;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            center = container.position;
            position = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 newPosition = eventData.position;

            Vector2 v1 = position - center;
            Vector2 v2 = newPosition - center;

            float dAngle = Mathf.DeltaAngle(Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg, Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg);

            if (Rotate(dAngle))
            {
                position = newPosition;
            }
        }

        private bool Rotate(float deltaAngle)
        {
            return SetRotation(currentAngle + deltaAngle);
        }

        private bool SetRotation(float angle)
        {
            bool wasAdjustment = false;
            float maxBorder = maxSwipeAngle;
            float minBorder = minSwipeAngle;

            if (angle > maxBorder)
            {
                angle = maxBorder;
                wasAdjustment = true;
            }

            if (angle < minBorder)
            {
                angle = minBorder;
                wasAdjustment = true;
            }

            currentAngle = angle;
            container.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            swipeDone = true;

            return !wasAdjustment;
        }
    }
}
