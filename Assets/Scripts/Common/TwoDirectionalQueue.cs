using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Common
{
    public class TwoDirectionalQueue<T> : ITwoDirectionalQueue<T>, IEnumerable<T>
    {
        private readonly LinkedList<T> storage = new LinkedList<T> ();

        public void PushLeft (T obj)
        {
            storage.AddFirst(obj);
            OnCollectionChanged();
        }

        public void PushRight (T obj)
        {
            storage.AddLast(obj);
            OnCollectionChanged();
        }

        public T PopLeft ()
        {
            T node = PeekLeft ();
            storage.RemoveFirst();
            OnCollectionChanged();
            return node;
        }

        public T PopRight ()
        {
            T node = PeekRight ();
            storage.RemoveLast ();
            OnCollectionChanged ();
            return node;
        }

        public T PeekLeft ()
        {
            return storage.First.Value;
        }

        public T PeekRight ()
        {
            return storage.Last.Value;
        }

        public T Left
        {
            get
            {
                return storage.First.Value;
            }
        }

        public T Right
        {
            get
            {
                return storage.Last.Value;
            }
        }

        public IEnumerator<T> GetEnumerator ()
        {
            return storage.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        protected virtual void OnCollectionChanged ()
        {
            
        }
    }
}