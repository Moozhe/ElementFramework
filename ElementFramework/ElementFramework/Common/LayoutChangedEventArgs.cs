using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ElementFramework
{
    public class LayoutChangedEventArgs : EventArgs
    {
        public bool BoundsChanged { get; private set; }
        public RectangleF NewBounds { get; private set; }
        public RectangleF OldBounds { get; private set; }

        public LayoutChangedEventArgs()
        {
        }

        public LayoutChangedEventArgs(RectangleF newBounds, RectangleF oldBounds)
        {
            BoundsChanged = true;
            NewBounds = newBounds;
            OldBounds = oldBounds;
        }
    }
}
