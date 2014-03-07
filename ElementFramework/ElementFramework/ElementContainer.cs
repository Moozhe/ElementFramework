using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ElementFramework
{
    public class ElementContainer : Control
    {
        private UIElementCollection _children;
        [Browsable(false)]
        public UIElementCollection Children
        {
            get { return _children; }
            private set { _children = value; }
        }

        #region Input Properties
        private Point _cursorPosition;
        internal Point CursorPosition
        {
            get
            {
                return _cursorPosition;
            }
            private set
            {
                if (_cursorPosition != value)
                {
                    LastCursorPosition = _cursorPosition;
                    _cursorPosition = value;
                }
            }
        }

        internal Point LastCursorPosition
        {
            get;
            private set;
        }

        private UIElement _focusedElement;
        internal UIElement FocusedElement
        {
            get
            {
                return _focusedElement;
            }
            private set
            {
                if (_focusedElement != value)
                {
                    if (_focusedElement != null)
                        _focusedElement.IsFocused = false;

                    _focusedElement = value;

                    if (_focusedElement != null)
                        _focusedElement.IsFocused = true;
                }
            }
        }

        private UIElement _mouseOverElement;
        internal UIElement MouseOverElement
        {
            get
            {
                return _mouseOverElement;
            }
            private set
            {
                if (_mouseOverElement != value)
                {
                    if (_mouseOverElement != null)
                        _mouseOverElement.IsMouseOver = false;

                    _mouseOverElement = value;

                    if (_mouseOverElement != null)
                        _mouseOverElement.IsMouseOver = true;
                }
            }
        }

        private UIElement _mouseCaptureElement;
        internal UIElement MouseCaptureElement
        {
            get
            {
                return _mouseCaptureElement;
            }
            private set
            {
                _mouseCaptureElement = value;
            }
        }
        #endregion
        
        #region Hidden Properties
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }
        #endregion

        public ElementContainer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.Selectable |
                          ControlStyles.OptimizedDoubleBuffer, true);

            this.DoubleBuffered = true;
            this.BackColor = Color.White;

            Children = new UIElementCollection(this, null);
            Children.CollectionChanged += Children_CollectionChanged;
        }

        public void PerformRender(object sender, RenderEventArgs e)
        {
            OnRender(e);
        }

        public UIElement GetChildAt(Point point)
        {
            foreach (UIElement child in Children.GetFrontToBack())
            {
                UIElement element = child.InputHitTest(child.PointFromContainer(point));

                if (element != null)
                    return element;
            }

            return null;
        }

        internal bool SetMouseCapture(UIElement element)
        {
            if (MouseCaptureElement == null)
            {
                MouseCaptureElement = element;
                return true;
            }

            return false;
        }

        internal void ReleaseMouseCapture(UIElement element)
        {
            if (MouseCaptureElement == element)
                MouseCaptureElement = null; 
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            foreach (UIElement child in Children)
            {
                if (child.Dock == DockStyle.Fill)
                {
                    child.Bounds = ClientRectangle;
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                PerformRender(this, new RenderEventArgs(e.Graphics));

                base.OnPaint(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OnPaint Exception");
            }
        }

        protected virtual void OnRender(RenderEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            foreach (UIElement element in Children)
            {
                element.PerformRender(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            MouseRoutedEventArgs args = new MouseRoutedEventArgs(e);

            foreach (UIElement element in Children.GetFrontToBack())
            {
                UIElement hitTest = element.InputHitTest(args.GetPosition(element));

                while (hitTest != null)
                {
                    //if (hitTest.HitTest(args.GetPosition(hitTest)))
                        hitTest.MouseDownInternal(args);

                    if (args.Handled)
                        break;
                    else
                        hitTest = hitTest.Parent;
                }

                if (args.Handled)
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            MouseRoutedEventArgs args = new MouseRoutedEventArgs(e);

            if (MouseCaptureElement != null)
            {
                MouseCaptureElement.MouseUpInternal(args);
                return;
            }

            foreach (UIElement element in Children.GetFrontToBack())
            {
                UIElement hitTest = element.InputHitTest(args.GetPosition(element));

                while (hitTest != null)
                {
                    //if (hitTest.HitTest(args.GetPosition(hitTest)))
                        hitTest.MouseUpInternal(args);

                    if (args.Handled)
                        break;
                    else
                        hitTest = hitTest.Parent;
                }

                if (args.Handled)
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            CursorPosition = e.Location;

            MouseRoutedEventArgs args = new MouseRoutedEventArgs(e);

            if (MouseCaptureElement != null)
            {
                MouseCaptureElement.MouseMoveInternal(args);
                return;
            }

            foreach (UIElement element in Children.GetFrontToBack())
            {
                UIElement hitTest = element.InputHitTest(args.GetPosition(element));

                MouseOverElement = hitTest;

                while (hitTest != null)
                {
                    //if (hitTest.HitTest(args.GetPosition(hitTest)))
                        hitTest.MouseMoveInternal(args);

                    if (args.Handled)
                        break;
                    else
                        hitTest = hitTest.Parent;
                }

                if (args.Handled)
                    break;
            }
        }

        protected void UpdateMouseOver()
        {
            MouseRoutedEventArgs args = new MouseRoutedEventArgs(new MouseEventArgs(MouseButtons.None, 0, CursorPosition.X, CursorPosition.Y, 0));


        }

        protected void UpdateMouseOver(MouseRoutedEventArgs e)
        {
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        private void Children_CollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            PerformLayout();
        }
    }
}
