using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElementFramework
{
    public class MouseRoutedEventArgs : RoutedEventArgs
    {
        private MouseButtons _button;
        public MouseButtons Button
        {
            get
            {
                return _button;
            }
            private set
            {
                _button = value;
            }
        }

        private int _clicks;
        public int Clicks
        {
            get
            {
                return _clicks;
            }
            private set
            {
                _clicks = value;
            }
        }

        private int _delta;
        public int Delta
        {
            get
            {
                return _delta;
            }
            private set
            {
                _delta = value;
            }
        }

        private Point _containerLocation;
        internal Point ContainerLocation
        {
            get
            {
                return _containerLocation;
            }
            set
            {
                _containerLocation = value;
            }
        }

        public MouseRoutedEventArgs(MouseEventArgs e)
            : this(null, e)
        {
        }

        public MouseRoutedEventArgs(object source, MouseEventArgs e)
            : base(source)
        {
            Button = e.Button;
            Clicks = e.Clicks;
            ContainerLocation = e.Location;
            Delta = e.Delta;
        }

        public PointF GetPosition(UIElement element)
        {
            return element.PointFromContainer(ContainerLocation);
        }

        public static MouseRoutedEventArgs FromMouseEventArgs(MouseEventArgs e)
        {
            return new MouseRoutedEventArgs(e);
        }
    }
}
