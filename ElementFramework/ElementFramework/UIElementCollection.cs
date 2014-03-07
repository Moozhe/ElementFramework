using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ElementFramework
{
    public class UIElementCollection : IList<UIElement>, ICollection<UIElement>, IEnumerable<UIElement>
    {
        public event EventHandler<CollectionChangedEventArgs> CollectionChanged;

        private ObservableList<UIElement> innerList;

        private ElementContainer _container;
        internal ElementContainer Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;

                foreach (UIElement item in innerList)
                    item.Container = value;
            }
        }

        private UIElement _parent;
        internal UIElement Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public UIElementCollection(ElementContainer container, UIElement parent)
        {
            innerList = new ObservableList<UIElement>();
            Container = container;
            Parent = parent;

            innerList.CollectionChanged += new EventHandler<CollectionChangedEventArgs>(innerList_CollectionChanged);
        }

        public int IndexOf(UIElement item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, UIElement item)
        {
            innerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            innerList.RemoveAt(index);
        }

        public UIElement this[int index]
        {
            get
            {
                return innerList[index];
            }
            set
            {
                innerList[index] = value;
            }
        }

        public UIElement this[string name]
        {
            get
            {
                foreach (UIElement item in this)
                {
                    if (item.Name.Equals(name))
                        return item;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public UIElement this[object tag]
        {
            get
            {
                foreach (UIElement item in this)
                {
                    if (item.Tag.Equals(tag))
                        return item;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public void Add(UIElement item)
        {
            innerList.Add(item);
        }

        public void AddRange(IEnumerable<UIElement> collection)
        {
            innerList.AddRange(collection);
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public bool Contains(UIElement item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(UIElement[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(UIElement item)
        {
            return innerList.Remove(item);
        }

        public void Sort()
        {
            innerList.Sort();
        }

        public void Sort(Comparison<UIElement> comparison)
        {
            innerList.Sort(comparison);
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        public IEnumerable<UIElement> GetFrontToBack()
        {
            for (int i = Count - 1; i >= 0; i--)
                yield return innerList[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        public override string ToString()
        {
            return innerList.ToString();
        }

        private void innerList_CollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            foreach (UIElement item in e.ItemsRemoved)
            {
                item.Invalidate();

                item.Container = null;
                item.Parent = null;
            }

            foreach (UIElement item in e.ItemsAdded)
            {
                item.Container = Container;
                item.Parent = Parent;

                item.Invalidate();
            }

            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }
    }
}
