using System;
using System.Collections.Generic;
using System.Text;

namespace ElementFramework
{
    public abstract class ElementFactory
    {
        public abstract UIElement Create(object obj);
    }
}
