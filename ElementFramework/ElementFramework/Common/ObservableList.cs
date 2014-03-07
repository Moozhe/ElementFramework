using System;
using System.Collections.Generic;
using System.Text;

namespace ElementFramework
{
    public class ObservableList<T> : List<T>
    {
        public event EventHandler<CollectionChangedEventArgs> CollectionChanged;

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);

            OnCollectionChanged(new CollectionChangedEventArgs(new object[] { item }, null));
        }

        public new void RemoveAt(int index)
        {
            T item = this[index];

            base.RemoveAt(index);

            OnCollectionChanged(new CollectionChangedEventArgs(null, new object[] { item }));
        }

        public new T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                T oldItem = base[index];

                base[index] = value;

                OnCollectionChanged(new CollectionChangedEventArgs(new object[] { value }, new object[] { oldItem }));
            }
        }

        public new void Add(T item)
        {
            base.Add(item);

            OnCollectionChanged(new CollectionChangedEventArgs(new object[] { item }, null));
        }

        public new void AddRange(IEnumerable<T> items)
        {
            base.AddRange(items);

            List<T> addedItems = new List<T>();
            addedItems.AddRange(items);

            OnCollectionChanged(new CollectionChangedEventArgs(addedItems, null));
        }

        public new void Clear()
        {
            T[] oldItems = this.ToArray();

            base.Clear();

            OnCollectionChanged(new CollectionChangedEventArgs(null, oldItems));
        }

        public new bool Remove(T item)
        {
            if (base.Remove(item))
            {
                OnCollectionChanged(new CollectionChangedEventArgs(null, new object[] { item }));
                return true;
            }

            return false;
        }

        protected virtual void OnCollectionChanged(CollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        public override string ToString()
        {
            return String.Format("Count = {0}", Count);
        }
    }
}
