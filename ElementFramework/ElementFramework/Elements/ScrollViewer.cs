using ElementFramework.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElementFramework
{
    public class ScrollViewer : UIElement
    {
        private const float VerticalScrollBarWidth = 10;
        private const float HorizontalScrollBarHeight = 10;

        private UIElement viewport;
        private ScrollBarElement vScroll;
        private ScrollBarElement hScroll;

        public new UIElementCollection Children
        {
            get
            {
                return viewport.Children;
            }
        }

        private bool _isVerticalScrollVisible = true;
        public bool IsVerticalScrollVisible
        {
            get
            {
                return _isVerticalScrollVisible;
            }
            private set
            {
                _isVerticalScrollVisible = value;
                vScroll.IsVisible = value;
            }
        }

        private bool _isHorizontalScrollVisible = true;
        public bool IsHorizontalScrollVisible
        {
            get
            {
                return _isHorizontalScrollVisible;
            }
            private set
            {
                _isHorizontalScrollVisible = value;
                hScroll.IsVisible = value;
            }
        }

        private ScrollBarVisibility _verticalScrollBarVisibility = ScrollBarVisibility.Auto;
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                return _verticalScrollBarVisibility;
            }
            set
            {
                _verticalScrollBarVisibility = value;
            }
        }

        private ScrollBarVisibility _horizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get
            {
                return _horizontalScrollBarVisibility;
            }
            set
            {
                _horizontalScrollBarVisibility = value;
            }
        }

        public float HorizontalOffset
        {
            get;
            private set;
        }

        public float VerticalOffset
        {
            get;
            private set;
        }

        public float ViewportWidth
        {
            get
            {
                return viewport.Width;
            }
        }

        public float ViewportHeight
        {
            get
            {
                return viewport.Height;
            }
        }

        public float ExtentWidth
        {
            get
            {
                float rightMost = 0;

                foreach (UIElement child in Children)
                {
                    if (child.Right > rightMost)
                        rightMost = child.Right;
                }

                return rightMost;
            }
        }

        public float ExtentHeight
        {
            get
            {
                float bottomMost = 0;

                foreach (UIElement child in Children)
                {
                    if (child.Bottom > bottomMost)
                        bottomMost = child.Bottom;
                }

                return bottomMost;
            }
        }

        public ScrollViewer()
        {
            vScroll = new ScrollBarElement() { Name = "vScroll", Width = VerticalScrollBarWidth, Dock = DockStyle.Right };
            hScroll = new ScrollBarElement() { Name = "hScroll", Height = HorizontalScrollBarHeight, Dock = DockStyle.Bottom };
            viewport = new UIElement() { Name = "viewport", Dock = DockStyle.Fill, ClipToBounds = true };

            base.Children.Add(vScroll);
            base.Children.Add(hScroll);
            base.Children.Add(viewport);

            viewport.ChildrenChanged += viewport_ChildrenChanged;
        }

        protected override void OnMouseDown(MouseRoutedEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnLayout(LayoutChangedEventArgs e)
        {
            base.OnLayout(e);

            switch (HorizontalScrollBarVisibility)
            {
                case ScrollBarVisibility.Visible:
                case ScrollBarVisibility.Disabled:
                    IsHorizontalScrollVisible = true;
                    break;

                case ScrollBarVisibility.Hidden:
                    IsHorizontalScrollVisible = false;
                    break;

                case ScrollBarVisibility.Auto:
                    IsHorizontalScrollVisible = ExtentWidth > ClientRectangle.Width;
                    break;
            }

            switch (VerticalScrollBarVisibility)
            {
                case ScrollBarVisibility.Visible:
                case ScrollBarVisibility.Disabled:
                    IsVerticalScrollVisible = true;
                    break;

                case ScrollBarVisibility.Hidden:
                    IsVerticalScrollVisible = false;
                    break;

                case ScrollBarVisibility.Auto:
                    IsVerticalScrollVisible = ExtentHeight > ClientRectangle.Height;
                    break;
            }

            if (HorizontalScrollBarVisibility == ScrollBarVisibility.Auto && VerticalScrollBarVisibility == ScrollBarVisibility.Auto)
            {
                if (IsHorizontalScrollVisible)
                    IsVerticalScrollVisible = ExtentHeight > ViewportHeight;

                if (IsVerticalScrollVisible)
                    IsHorizontalScrollVisible = ExtentWidth > ViewportWidth;
            }
        }

        protected override void OnRender(RenderEventArgs e)
        {
            base.OnRender(e);
        }

        protected override void OnChildrenChanged(CollectionChangedEventArgs e)
        {
            base.OnChildrenChanged(e);
        }

        private void viewport_ChildrenChanged(object sender, CollectionChangedEventArgs e)
        {
            foreach (UIElement child in e.ItemsRemoved)
                child.Layout -= ChildLayoutUpdated;

            foreach (UIElement child in e.ItemsAdded)
                child.Layout += ChildLayoutUpdated;
        }

        private void ChildLayoutUpdated(object sender, LayoutChangedEventArgs e)
        {
            PerformLayout(new LayoutChangedEventArgs());
        }
    }

    public enum ScrollBarVisibility
    {
        Disabled,
        Auto,
        Hidden,
        Visible,
    }
}
