using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ElementFramework
{
    public class StackPanel : UIElement
    {
        private Orientation _orientation;
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;

                    PerformLayout();
                }
            }
        }

        public StackPanel()
        {
        }

        public override SizeF GetDesiredSize(SizeF availableSize)
        {
            if (Orientation == Orientation.Vertical)
            {
                float totalHeight = 0;

                foreach (UIElement element in Children)
                {
                    SizeF desiredSize = element.GetDesiredSize(new SizeF(ClientRectangle.Width, float.PositiveInfinity));
                    totalHeight += desiredSize.Height;
                }

                return new SizeF(Width, totalHeight);
            }
            else
            {
                float totalWidth = 0;

                foreach (UIElement element in Children)
                {
                    SizeF desiredSize = element.GetDesiredSize(new SizeF(float.PositiveInfinity, ClientRectangle.Height));
                    totalWidth += desiredSize.Width;
                }

                return new SizeF(totalWidth, Height);
            }
        }

        protected override void OnLayout(LayoutChangedEventArgs e)
        {
            base.OnLayout(e);

            if (Orientation == Orientation.Vertical)
            {
                float top = ClientRectangle.Top;

                foreach (UIElement element in Children)
                {
                    SizeF desiredSize = element.GetDesiredSize(new SizeF(ClientRectangle.Width, float.PositiveInfinity));
                    element.Bounds = new RectangleF(ClientRectangle.Left, top, ClientRectangle.Width, desiredSize.Height);
                    top = element.Bottom;
                }
            }
            else if (Orientation == Orientation.Horizontal)
            {
                float left = ClientRectangle.Left;

                foreach (UIElement element in Children)
                {
                    SizeF desiredSize = element.GetDesiredSize(new SizeF(float.PositiveInfinity, ClientRectangle.Height));
                    element.Bounds = new RectangleF(left, ClientRectangle.Top, desiredSize.Width, ClientRectangle.Height);
                    left = element.Right;
                }
            }
        }

        protected override void OnChildrenChanged(CollectionChangedEventArgs e)
        {
            base.OnChildrenChanged(e);

            if (Parent != null)
                Parent.PerformLayout();
        }
    }
}
