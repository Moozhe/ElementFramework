using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ElementFramework
{
    public class CollectionChangedEventArgs : EventArgs
    {
        private IList _itemsAdded;
        public IList ItemsAdded
        {
            get
            {
                if (_itemsAdded == null)
                    _itemsAdded = new object[] { };

                return _itemsAdded;
            }
            private set
            {
                _itemsAdded = value;
            }
        }

        private IList _itemsRemoved;
        public IList ItemsRemoved
        {
            get
            {
                if (_itemsRemoved == null)
                    _itemsRemoved = new object[] { };

                return _itemsRemoved;
            }
            private set
            {
                _itemsRemoved = value;
            }
        }

        public CollectionChangedEventArgs(IList itemsAdded, IList itemsRemoved)
        {
            if (itemsAdded != null)
                ItemsAdded = itemsAdded;

            if (itemsRemoved != null)
                ItemsRemoved = itemsRemoved;
        }
    }
}
