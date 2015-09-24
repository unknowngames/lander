using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.RadialScroll
{
    public class RadialContent : UIBehaviour, IScrollListContent, ISwipeGesture
    {
        public OnListItemClickEvent OnContentListItemClick = new OnListItemClickEvent();

        [SerializeField]
        private RectTransform container;
        [SerializeField]
        private float startAngle;
        [SerializeField] 
        private float spacing;
        [SerializeField]
        private float radius;
        
        private List<ListItem> items = new List<ListItem>();

        [SerializeField]
        private float angularDrag;
        private float angularVelocity;
        private float lerpProgress;
        private float currentAngle;
        private bool allowInertionMove;

        protected override void Start()
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
            get { return items; }
        }

        public int Count
        {
            get { return items.Count; }
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

            items.Clear();
        }

        public void Update()
        {
            if (allowInertionMove)
            {
                if (!Mathf.Approximately(angularVelocity, 0.0f))
                {
                    currentAngle = container.eulerAngles.z + angularVelocity;
                    SetRotation(currentAngle);

                    angularVelocity = Mathf.Lerp(angularVelocity, 0.0f, lerpProgress);
                    lerpProgress += angularDrag;
                }
                else
                {
                    angularVelocity = 0.0f;
                    allowInertionMove = false;
                }
            }
            Rebuild();
        }

        private void SetRotation(float angle)
        {
            container.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }

        private void Rebuild()
        {
            int itemsCount = Items.Count;
            if (itemsCount == 0)
            {
                return;
            }

            float stepAngleRad = 2*Mathf.Asin(spacing/(2*radius));

            float anglesSum = stepAngleRad*itemsCount;

            if (anglesSum > Mathf.PI*2.0f)
            {
                stepAngleRad = Mathf.PI * 2.0f / itemsCount;
                radius = spacing / (2.0f * Mathf.Sin(stepAngleRad / 2.0f));
            }

            for (int i = 0; i < itemsCount; i++)
            {
                float itemAngleRad = -stepAngleRad*i + startAngle*Mathf.Deg2Rad;

                float x = radius*Mathf.Cos(itemAngleRad);
                float y = radius*Mathf.Sin(itemAngleRad);

                Vector2 itemPosition = new Vector2(x, y);

                ((RectTransform) Items[i].transform).anchoredPosition = itemPosition;
            }

            container.sizeDelta = new Vector2(2*radius, 2*radius);
            container.anchoredPosition = new Vector2(0, -radius);
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            int itemsCount = Items.Count;

            for (int i = 0; i < itemsCount; i++)
            {
                Items[i].transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
            }
        }

        public void OnSwipe(SwipeGestureData swipeGestureData)
        {
            MoveDirection direction = swipeGestureData.Direction;
            
            if (direction == MoveDirection.Up || direction == MoveDirection.Down)
            {
                return;
            }

            float distance;
            float dAngle;
            switch (swipeGestureData.Phase)
            {
                case ESwipePhase.Start:
                    allowInertionMove = false;
                    currentAngle = container.eulerAngles.z;
                    break;
                case ESwipePhase.Move:
                    allowInertionMove = false;
                    distance = swipeGestureData.Distance;
                    dAngle = Mathf.Asin(distance / radius) * Mathf.Rad2Deg;

                    if (float.IsNaN(dAngle))
                    {
                        return;
                    }

                    if (swipeGestureData.Direction == MoveDirection.Right)
                    {
                        dAngle *= -1;
                    }

                    SetRotation(currentAngle + dAngle);
                    break;
                case ESwipePhase.End:

                    distance = swipeGestureData.DistanceDelta;
                    dAngle = Mathf.Asin(distance / radius) * Mathf.Rad2Deg;

                    if (float.IsNaN(dAngle))
                    {
                        return;
                    }

                    if (swipeGestureData.Direction == MoveDirection.Right)
                    {
                        dAngle *= -1;
                    }

                    angularVelocity = dAngle;
                    allowInertionMove = true;
                    lerpProgress = 0.0f;
                    break;
                case ESwipePhase.Cancel:
                    allowInertionMove = false;
                    break;
            }
        }
    }
}
