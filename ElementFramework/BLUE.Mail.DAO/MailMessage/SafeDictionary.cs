using System;
using System.Collections.Generic;
using System.Text;

namespace BLUE.Mail.DAO
{
    public class SafeDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public SafeDictionary()
        {
        }

        public SafeDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        public virtual new TValue this[TKey key]
        {
            get
            {
                return Utilities.Get(this, key);
            }
            set
            {
                Utilities.Set(this, key, value);
            }
        }
    }
}
